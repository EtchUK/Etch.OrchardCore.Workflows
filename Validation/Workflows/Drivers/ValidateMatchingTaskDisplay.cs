using Etch.OrchardCore.Workflows.Validation.Workflows.Activities;
using Etch.OrchardCore.Workflows.Validation.Workflows.ViewModels;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Workflows.Validation.Workflows.Drivers
{
    public class ValidateMatchingTaskDisplay : ActivityDisplayDriver<ValidateMatchingTask, ValidateMatchingTaskViewModel>
    {
        #region Implementation

        protected override void EditActivity(ValidateMatchingTask activity, ValidateMatchingTaskViewModel model)
        {
            model.ErrorMessage = activity.ErrorMessage;
            model.ToValidate = activity.ToValidate;
        }

        protected override void UpdateActivity(ValidateMatchingTaskViewModel model, ValidateMatchingTask activity)
        {
            activity.ErrorMessage = model.ErrorMessage;
            activity.ToValidate = model.ToValidate;
        }

        #endregion Implementation
    }
}
