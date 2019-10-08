using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.Email;
using OrchardCore.Liquid;
using OrchardCore.Templates.Services;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.TemplateEmail.Workflows.Activities
{
    public class TemplateEmailTask : TaskActivity
    {
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        private readonly ILogger<TemplateEmailTask> _logger;
        private readonly ISmtpService _smtpService;
        private readonly TemplatesManager _templatesManager;

        public TemplateEmailTask(
            IWorkflowExpressionEvaluator expressionEvaluator,
            ILiquidTemplateManager liquidTemplateManager,
            IStringLocalizer<TemplateEmailTask> localizer,
            ILogger<TemplateEmailTask> logger,
            ISmtpService smtpService,
            TemplatesManager templatesManager
        )
        {
            _expressionEvaluator = expressionEvaluator;
            _logger = logger;
            _smtpService = smtpService;
            _templatesManager = templatesManager;

            T = localizer;
        }

        private IStringLocalizer T { get; }

        public override LocalizedString DisplayText => T["Template Email Task"];
        public override string Name => nameof(TemplateEmailTask);
        public override LocalizedString Category => T["Messaging"];

        public WorkflowExpression<string> Sender
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        // TODO: Add support for the following format: Jack Bauer<jack@ctu.com>, ...
        public WorkflowExpression<string> Recipients
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> Subject
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public string TemplateName
        {
            get => GetProperty(() => string.Empty);
            set => SetProperty(value);
        }

        public WorkflowExpression<string> Body
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public bool IsBodyHtml
        {
            get => GetProperty(() => true);
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T["Done"], T["Failed"]);
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var senderTask = _expressionEvaluator.EvaluateAsync(Sender, workflowContext);
            var recipientsTask = _expressionEvaluator.EvaluateAsync(Recipients, workflowContext);
            var subjectTask = _expressionEvaluator.EvaluateAsync(Subject, workflowContext);
            var body = await _expressionEvaluator.EvaluateAsync(Body, workflowContext);

            if (!string.IsNullOrEmpty(TemplateName))
            {
                var template = (await _templatesManager.GetTemplatesDocumentAsync()).Templates[TemplateName];

                body = template.Content
                    .Replace("{{ Body }}", body)
                    .Replace("{{Body}}", body)
                    .Replace("{{ body }}", body)
                    .Replace("{{body}}", body);
            }
            
            await Task.WhenAll(senderTask, recipientsTask, subjectTask);

            var message = new MailMessage
            {
                Subject = subjectTask.Result.Trim(),
                Body = body.Trim(),
                IsBodyHtml = IsBodyHtml
            };

            message.To = recipientsTask.Result.Trim();

            if (!string.IsNullOrWhiteSpace(senderTask.Result))
            {
                message.From = senderTask.Result.Trim();
            }

            var result = await _smtpService.SendAsync(message);
            workflowContext.LastResult = result;

            if (!result.Succeeded)
            {
                return Outcomes("Failed");
            }

            return Outcomes("Done");
        }
    }
}
