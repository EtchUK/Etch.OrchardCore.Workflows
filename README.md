# Etch.OrchardCore.Workflows

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) provides useful Workflow tasks and events.

## Build Status

_Internal module that isn't currently publically available._

## Orchard Core Reference

This module is referencing the beta 3 build of Orchard Core ([`1.0.0-beta3-71077`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-beta3-71077)).

## Installing

This module is available on our private NuGet feed, search "Etch.OrchardCore.Workflows" within NuGet package manager in order to install the module within your Orchard Core site project. Your project must be configured to use our private NuGet feed otherwise no results will be returned.

## Features

This module provides a set features which operate independantly of each other.

### Export Workflows

When enabled this feature will make a new "Export Workflows" option available in the admin menu for users with the `Export workflow data` permission (also provided by this feature).

The "Export Workflows" page displays a list of workflows similar to what is seen when accessing the main "Workflows" route, selecting "Export" on one of these will take the user to a "Preview" page showing them how many instances the chosen workflow has and a preview of the Outputs the latest one has.

Clicking 'Download export' will then return a CSV with columns for any `Output` ever specified in any Instance of this Workflow Type.

### Validation

This feature adds useful validation tasks for Workflows.

#### Validate Multiple Fields

Allows the user to specify multiple field names which will all be validated for not having empty content. The task provides `Valid` and `Invalid` outcomes as well as `Done` and will update the `ModelState` in the same way as the stock Orchard Core validate tasks.
