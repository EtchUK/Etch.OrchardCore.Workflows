using Etch.OrchardCore.Workflows.TemplateEmail.Workflows.Activities;
using Etch.OrchardCore.Workflows.TemplateEmail.Workflows.ViewModels;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Etch.OrchardCore.Workflows.TemplateEmail.Workflows.Drivers
{
    public class TemplateEmailTaskDriver : ActivityDisplayDriver<TemplateEmailTask, TemlateEmailTaskViewModel>
    {
        #region Overrides

        protected override void EditActivity(TemplateEmailTask activity, TemlateEmailTaskViewModel model)
        {
            model.SenderExpression = activity.Sender.Expression;
            model.RecipientsExpression = activity.Recipients.Expression;
            model.SubjectExpression = activity.Subject.Expression;
            model.Body = activity.Body.Expression;
            model.IsBodyHtml = activity.IsBodyHtml;
            model.TemplateName = activity.TemplateName;
        }

        protected override void UpdateActivity(TemlateEmailTaskViewModel model, TemplateEmailTask activity)
        {
            activity.Sender = new WorkflowExpression<string>(model.SenderExpression);
            activity.Recipients = new WorkflowExpression<string>(model.RecipientsExpression);
            activity.Subject = new WorkflowExpression<string>(model.SubjectExpression);
            activity.Body = new WorkflowExpression<string>(model.Body);
            activity.IsBodyHtml = model.IsBodyHtml;
            activity.TemplateName = model.TemplateName;
        }

        #endregion
    }
}
