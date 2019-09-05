using CsvHelper;
using Newtonsoft.Json.Linq;
using OrchardCore.Workflows.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.Export.Services
{
    public class ExportService : IExportService
    {
        #region Implementation

        public async Task<Stream> GetExportFileAsStreamAsync(IEnumerable<Workflow> instances)
        {
            var rows = instances.Select(x => GetOutput(x)).ToList();
            var headers = GetHeaders(rows);

            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var csvWriter = new CsvWriter(streamWriter);

            await WriteHeadersAsync(csvWriter, headers);
            await WriteRowsAsync(csvWriter, headers, rows);

            await csvWriter.FlushAsync();
            await streamWriter.FlushAsync();

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public IDictionary<string, string> GetOutput(Workflow workflow)
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
                         .ToDictionary(group => group.Key, group => group.First());
        }

        #endregion Implementation

        #region Private Methods

        private IList<string> GetHeaders(IList<IDictionary<string, string>> rows)
        {
            return rows.SelectMany(x => x.Keys).Distinct().ToList();
        }

        private async Task WriteHeadersAsync(CsvWriter csvWriter, IList<string> headers)
        {
            foreach (var header in headers)
            {
                csvWriter.WriteField(header);
            }
            await csvWriter.NextRecordAsync();
        }

        private async Task WriteRowsAsync(CsvWriter csvWriter, IList<string> headers, List<IDictionary<string, string>> rows)
        {
            foreach (var row in rows)
            {
                foreach (var header in headers)
                {
                    csvWriter.WriteField(row.ContainsKey(header) ? row[header] ?? "" : "");
                }
                await csvWriter.NextRecordAsync();
            }
        }

        #endregion Private Methods
    }
}
