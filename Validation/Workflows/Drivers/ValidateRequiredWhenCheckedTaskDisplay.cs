using Etch.OrchardCore.Workflows.Validation.Workflows.Activities;
using Etch.OrchardCore.Workflows.Validation.Workflows.ViewModels;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Workflows.Validation.Workflows.Drivers
{
    public class ValidateRequiredWhenCheckedTaskDisplay : ActivityDisplayDriver<ValidateRequiredWhenCheckedTask, ValidateRequiredWhenCheckTaskViewModel>
    {
        #region Implementation

        protected override void EditActivity(ValidateRequiredWhenCheckedTask activity, ValidateRequiredWhenCheckTaskViewModel model)
        {
            model.CheckboxField = activity.CheckboxField;
            model.ErrorMessage = activity.ErrorMessage;
            model.ToValidate = activity.ToValidate;
        }

        protected override void UpdateActivity(ValidateRequiredWhenCheckTaskViewModel model, ValidateRequiredWhenCheckedTask activity)
        {
            activity.CheckboxField = model.CheckboxField;
            activity.ErrorMessage = model.ErrorMessage;
            activity.ToValidate = model.ToValidate;
        }

        #endregion Implementation
    }
}
