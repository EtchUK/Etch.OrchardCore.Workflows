using OrchardCore.Security.Permissions;
using System.Collections.Generic;

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

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ExportWorkflows };
        }

        #endregion Implementation
    }
}
