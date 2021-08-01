using Etch.OrchardCore.Workflows;
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK Ltd.",
    Category = "Workflows",
    Description = "Provides useful workflow tasks and events",
    Name = "Etch Workflows",
    Version = "1.0.0",
    Website = "https://etchuk.com"
)]

[assembly: Feature(
    Id = Constants.Features.Export,
    Name = "Workflow Export",
    Category = "Workflows",
    Dependencies = new[] { "OrchardCore.Workflows" },
    Description = "Provides export of data from workflows"
)]

[assembly: Feature(
    Id = Constants.Features.FormOutput,
    Name = "Form Output",
    Category = "Workflows",
    Dependencies = new[] { "OrchardCore.Workflows" },
    Description = "Provides a task for writing all form fields into Output properties."
)]

[assembly: Feature(
    Id = Constants.Features.TemplateEmail,
    Name = "Workflow Template Email",
    Category = "Workflows",
    Dependencies = new[] { "OrchardCore.Workflows", "OrchardCore.Email", "OrchardCore.Templates" },
    Description = "Provides email task that can use template."
)]

[assembly: Feature(
    Id = Constants.Features.Validation,
    Name = "Validation",
    Category = "Workflows",
    Dependencies = new[] { "OrchardCore.Workflows", "OrchardCore.Forms" },
    Description = "Adds useful validation tasks for forms"
)]