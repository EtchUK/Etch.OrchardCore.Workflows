using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Workflows.Controllers
{
    [Admin]
    [Feature(Constants.Features.Export)]
    public class ExportController : Controller
    {
        #region Actions

        #region Index

        public IActionResult Index()
        {
            return View();
        }

        #endregion Index

        #endregion Actions
    }
}
