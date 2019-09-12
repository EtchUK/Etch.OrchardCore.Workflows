using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Etch.OrchardCore.Workflows.TemplateEmail.Workflows.ViewModels
{
    public class TemlateEmailTaskViewModel
    {
        public string SenderExpression { get; set; }

        [Required]
        public string RecipientsExpression { get; set; }
        public string SubjectExpression { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        
        public string TemplateName { get; set; }
    }
}
