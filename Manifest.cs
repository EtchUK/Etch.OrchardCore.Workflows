using Etch.OrchardCore.Workflows;
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK Ltd.",
    Category = "Workflows",
    Description = "Provides useful workflow tasks and events",
    Name = "Etch Workflows",
    Version = "0.0.1",
    Website = "https://etchuk.com"
)]

[assembly: Feature(
    Id = Constants.Features.Export,
    Name = "Workflow Export",
    Category = "Workflows",
    Dependencies = new[] { "OrchardCore.Workflows" },
    Description = "Provides export of data from workflows"
)]