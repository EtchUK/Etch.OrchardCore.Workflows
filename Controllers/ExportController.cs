using CsvHelper;
using Etch.OrchardCore.Workflows.Export;
using Etch.OrchardCore.Workflows.Export.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using OrchardCore.Workflows.Indexes;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using YesSql.Services;

namespace Etch.OrchardCore.Workflows.Controllers
{
    [Admin]
    [Feature(Constants.Features.Export)]
    public class ExportController : Controller
    {
        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly ISession _session;
        private readonly ISiteService _siteService;

        #region Properties

        private dynamic New { get; }

        #endregion Properties

        #endregion Dependencies

        #region Constructor

        public ExportController(
            IAuthorizationService authorizationService,
            ISession session,
            IShapeFactory shapeFactory,
            ISiteService siteService)
        {
            _authorizationService = authorizationService;
            _session = session;
            New = shapeFactory;
            _siteService = siteService;
        }

        #endregion Constructor

        #region Actions

        #region Index

        public async Task<IActionResult> Index(PagerParameters pagerParameters)
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);

            var query = _session.Query<WorkflowType, WorkflowTypeIndex>()
                .OrderBy(x => x.Name);

            var count = await query.CountAsync();

            var workflowTypes = await query
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize)
                .ListAsync();
            var workflowTypeIds = workflowTypes.Select(x => x.WorkflowTypeId).ToList();
            var workflowInstances = (await _session.QueryIndex<WorkflowIndex>(x => x.WorkflowTypeId.IsIn(workflowTypeIds))
                .ListAsync())
                .GroupBy(x => x.WorkflowTypeId)
                .ToDictionary(x => x.Key);

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count);

            var model = new WorkflowExportListViewModel
            {
                WorkflowTypes = workflowTypes
                .Select(x => new WorkflowTypeEntry
                {
                    WorkflowType = x,
                    Id = x.Id,
                    Name = x.Name,
                    WorkflowCount = workflowInstances.ContainsKey(x.WorkflowTypeId) ? workflowInstances[x.WorkflowTypeId].Count() : 0
                })
                .ToList(),
                Pager = pagerShape
            };
            return View(model);
        }

        #endregion Index

        #region Preview

        public async Task<IActionResult> Preview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowType = await _session.GetAsync<WorkflowType>(id.Value);

            if (workflowType == null)
            {
                return NotFound();
            }

            var workflowInstancesQuery = _session.Query<Workflow, WorkflowIndex>(x => x.WorkflowTypeId == workflowType.WorkflowTypeId)
                .OrderByDescending(x => x.CreatedUtc);

            var instancesCount = await workflowInstancesQuery.CountAsync();
            var preview = await workflowInstancesQuery.FirstOrDefaultAsync();

            var model = new PreviewWorkflowExportViewModel
            {
                Name = workflowType.Name,
                InstanceCount = instancesCount,
                PreviewOutput = GetOutput(preview),
                WorkflowTypeId = id.Value
            };

            return View(model);
        }

        #endregion Preview

        #region Export

        [HttpPost]
        public async Task<IActionResult> Export(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowType = await _session.GetAsync<WorkflowType>(id.Value);

            if (workflowType == null)
            {
                return NotFound();
            }

            var query = _session.Query<Workflow, WorkflowIndex>(x => x.WorkflowTypeId == workflowType.WorkflowTypeId)
                .OrderByDescending(x => x.CreatedUtc);

            var instances = await query.ListAsync();

            var rows = instances.Select(x => GetOutput(x)).ToList();

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream);

            var csvWriter = new CsvWriter(streamWriter);

            var headers = rows.SelectMany(x => x.Keys).Distinct();

            foreach (var header in headers)
            {
                csvWriter.WriteField(header);
            }
            await csvWriter.NextRecordAsync();

            foreach (var row in rows)
            {
                foreach (var header in headers)
                {
                    csvWriter.WriteField(row.ContainsKey(header) ? row[header] ?? "" : "");
                }
                await csvWriter.NextRecordAsync();
            }

            await csvWriter.FlushAsync();

            await streamWriter.FlushAsync();

            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, "text/csv", $"{workflowType.Name}-Export-{DateTime.UtcNow:dd-MM-yyyy-HHmm}.csv");
        }

        #endregion Export

        #endregion Actions

        #region Events

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ExportWorkflows))
            {
                context.Result = Unauthorized();
            }
            else
            {
                await next();
            }
        }

        #endregion Events

        #region Private Methods

        private IDictionary<string, string> GetOutput(Workflow workflow)
        {
            var result = (IDictionary<string, string>)new Dictionary<string, string>();
            if (workflow == null)
            {
                return result;
            }
            result.Add(ExportConstants.CreatedAtUTCColumnName, workflow.CreatedUtc.ToString());
            if (workflow.State == null)
            {
                return result;
            }
            var output = workflow.State.Value<JObject>("Output");
            if (output == null)
            {
                return result;
            }
            var outputDict = output.ToObject<IDictionary<string, string>>();
            // Merge dictionaries
            return new[] { result, outputDict }.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(group => group.Key, group => group.First()); ;
        }

        #endregion Private Methods
    }
}
