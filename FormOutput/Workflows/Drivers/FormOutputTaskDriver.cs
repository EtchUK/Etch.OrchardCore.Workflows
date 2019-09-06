using Etch.OrchardCore.Workflows.FormOutput.Workflows.Activities;
using Etch.OrchardCore.Workflows.FormOutput.Workflows.ViewModels;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Workflows.FormOutput.Workflows.Drivers
{
    public class FormOutputTaskDriver : ActivityDisplayDriver<FormOutputTask, FormOutputTaskViewModel>
    {
        protected override void EditActivity(FormOutputTask activity, FormOutputTaskViewModel model)
        {
            model.Ignored = activity.Ignored;
            model.Prefix = activity.Prefix;
        }

        protected override void UpdateActivity(FormOutputTaskViewModel model, FormOutputTask activity)
        {
            activity.Ignored = model.Ignored;
            activity.Prefix = model.Prefix;
        }
    }
}
