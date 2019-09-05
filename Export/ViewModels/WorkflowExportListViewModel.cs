using OrchardCore.Workflows.ViewModels;
using System.Collections.Generic;

namespace Etch.OrchardCore.Workflows.Export.ViewModels
{
    public class WorkflowExportListViewModel
    {
        public IList<WorkflowTypeEntry> WorkflowTypes { get; set; }
        public dynamic Pager { get; set; }
    }
}
