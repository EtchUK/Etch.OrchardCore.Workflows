# Etch.OrchardCore.Workflows

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) provides useful Workflow tasks and events.

## Build Status

_Internal module that isn't currently publically available._

## Orchard Core Reference

This module is referencing the beta 3 build of Orchard Core ([`1.0.0-beta3-71077`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-beta3-71077)).

## Installing

This module is available on our private NuGet feed, search "Etch.OrchardCore.Workflows" within NuGet package manager in order to install the module within your Orchard Core site project. Your project must be configured to use our private NuGet feed otherwise no results will be returned.

If you want to give the module a try you can[download the source](https://github.com/etchuk/Etch.OrchardCore.InjectScripts/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.Workflows.

Once the module is in a usable state we'll make it available via NuGet.

## Features

This module provides a set features which operate independantly of each other.

### Export Workflows

Adds "Export Workflows" option in the admin menu for users with the `Export workflow data` permission.

The "Export Workflows" page displays a list of workflows similar to what is seen when accessing the main "Workflows" route, selecting "Export" on one of these will take the user to a "Preview" page showing them how many instances the chosen workflow has and a preview of the Outputs the latest one has.

Clicking 'Download export' will then return a CSV with columns for any `Output` ever specified in any Instance of this Workflow Type.

### Validation

This feature adds useful validation tasks for Workflows.

#### Validate Matching Fields

Allows the user to specify multiple field names which will all be validated for having matching content. The task provides `Valid` and `Invalid` outcomes as well as `Done` and will update the `ModelState` in the same way as the stock Orchard Core validate tasks.

#### Validate Multiple Fields

Allows the user to specify multiple field names which will all be validated for not having empty content. The task provides `Valid` and `Invalid` outcomes as well as `Done` and will update the `ModelState` in the same way as the stock Orchard Core validate tasks.

#### Validate Required When Checked

Validates that multiple fields have a value when a defined checkbox field has been checked. The task provides `Valid` and `Invalid` outcomes as well as `Done` and will update the `ModelState` in the same way as the stock Orchard Core validate tasks.

### Form Output

Provides a task `Set Form Outputs` which will send all entries from the form to the `Output` of the workflow with the same names as their form `input`s.

The following options are available:

#### Prefix

Prepend the entered string to the output keys.

#### Ignored

Allows the user to specify fields which should not be added to the `Output`. e.g.: form validation token.
