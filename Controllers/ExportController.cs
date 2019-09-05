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
using System.Collections.Generic;
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
                PreviewOutput = GetPreviewOutput(preview),
                WorkflowTypeId = id.Value
            };

            return View(model);
        }

        #endregion Preview

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

        private IDictionary<string, string> GetPreviewOutput(Workflow workflow)
        {
            if (workflow?.State == null)
            {
                return null;
            }
            var output = workflow.State.Value<JObject>("Output");
            if (output == null)
            {
                return null;
            }
            return output.ToObject<IDictionary<string, string>>();
        }

        #endregion Private Methods
    }
}
