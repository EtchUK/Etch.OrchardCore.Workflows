using System.Collections.Generic;

namespace Etch.OrchardCore.Workflows.Export.ViewModels
{
    public class PreviewWorkflowExportViewModel
    {
        public string Name { get; internal set; }
        public int InstanceCount { get; internal set; }
        public IDictionary<string, string> PreviewOutput { get; set; }
        public int WorkflowTypeId { get; set; }
    }
}
