using OrchardCore.Workflows.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.Export.Services
{
    public interface IExportService
    {
        Task<Stream> GetExportFileAsStreamAsync(IEnumerable<Workflow> instances);
        IDictionary<string, string> GetOutput(Workflow instance);
    }
}
