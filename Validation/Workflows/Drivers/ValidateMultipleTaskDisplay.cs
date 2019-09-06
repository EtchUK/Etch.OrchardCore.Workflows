using Etch.OrchardCore.Workflows.Validation.Workflows.Activities;
using Etch.OrchardCore.Workflows.Validation.Workflows.ViewModels;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Workflows.Validation.Workflows.Drivers
{
    public class ValidateMultipleTaskDisplay : ActivityDisplayDriver<ValidateMultipleTask, ValidateMultipleTaskViewModel>
    {
        #region Implementation

        protected override void EditActivity(ValidateMultipleTask activity, ValidateMultipleTaskViewModel model)
        {
            model.ErrorMessage = activity.ErrorMessage;
            model.ToValidate = activity.ToValidate;
        }

        protected override void UpdateActivity(ValidateMultipleTaskViewModel model, ValidateMultipleTask activity)
        {
            activity.ErrorMessage = model.ErrorMessage;
            activity.ToValidate = model.ToValidate;
        }

        #endregion Implementation
    }
}
