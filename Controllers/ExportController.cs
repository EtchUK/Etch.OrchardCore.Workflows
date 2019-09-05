using Etch.OrchardCore.Workflows.Export;
using Etch.OrchardCore.Workflows.Export.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.Admin;
using OrchardCore.Modules;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.Controllers
{
    [Admin]
    [Feature(Constants.Features.Export)]
    public class ExportController : Controller
    {
        #region Dependencies

        private readonly IAuthorizationService _authorizationService;

        #endregion Dependencies

        #region Constructor

        public ExportController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        #endregion Constructor

        #region Actions

        #region Index

        public IActionResult Index()
        {
            var model = new WorkflowExportListViewModel();
            return View(model);
        }

        #endregion Index

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
    }
}
