using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.Export
{
    public class Permissions : IPermissionProvider
    {
        #region Permissions

        public static readonly Permission ExportWorkflows = new Permission("ExportWorkflows", "Export workflow data");

        #endregion Permissions

        #region Implementation

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new []{ ExportWorkflows }
                }
            };
        }

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ExportWorkflows }.AsEnumerable());
        }

        #endregion Implementation
    }
}
