using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using System.Collections.Generic;

namespace Etch.OrchardCore.Workflows.FormOutput.Workflows.Activities
{
    public class FormOutputTask : TaskActivity
    {
        #region Constants

        private const string OutcomeDone = "Done";

        #endregion Constants

        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Properties

        public IStringLocalizer T { get; set; }

        #endregion Properties

        #endregion Dependencies

        #region Constructor

        public FormOutputTask(
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<FormOutputTask> stringLocalizer
            )
        {
            _httpContextAccessor = httpContextAccessor;
            T = stringLocalizer;
        }

        #endregion Constructor

        #region Implementation

        #region Properties

        #region Public

        public override string Name => nameof(FormOutputTask);

        public override LocalizedString Category => T["Primitives"];

        #endregion Public

        #region Input

        public string Prefix
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        #endregion Input

        #endregion Properties

        #region Actions

        public override bool CanExecute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return _httpContextAccessor.HttpContext?.Request?.Form != null;
        }

        public override ActivityExecutionResult Execute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var form = _httpContextAccessor.HttpContext?.Request?.Form;

            if (form == null)
            {
                return Outcomes(OutcomeDone);
            }

            foreach (var field in form.Keys)
            {
                var outputKey = field;
                if (!string.IsNullOrWhiteSpace(Prefix))
                {
                    outputKey = Prefix + outputKey;
                }
                workflowContext.Output[outputKey] = string.Join(", ", form[field].ToArray());
            }

            return Outcomes(OutcomeDone);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T[OutcomeDone]);
        }

        #endregion Actions

        #endregion Implementation
    }
}
