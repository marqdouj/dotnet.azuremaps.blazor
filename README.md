# DotNet.AzureMaps.Blazor

## Summary
Contains a script and Blazor component (`MapsContainer`) that manages JSInterop between the component and the Azure Maps Web SDK. 
The component allows for management of multiple maps within it's `ChildContent`.
Formatting and styling is easily managed by the user within the ChildContent.

## NOTE: 
This library supersedes the [DotNet.AzureMaps](https://www.nuget.org/packages/Marqdouj.DotNet.AzureMaps/) NuGet package.

## UI Companion Library
- [NuGet](https://www.nuget.org/packages/Marqdouj.DotNet.AzureMaps.Blazor.UI/) package.
- [README](README_UI.md)

## Prerequisites
- [Azure Maps account](https://learn.microsoft.com/en-us/azure/azure-maps/quick-demo-map-app#create-an-azure-maps-account).
If you don't have an azure account, you can create a [free account](https://azure.microsoft.com).

## Demo
A demo of this library, and some of my other `DotNet` libraries, can be found [here](https://github.com/marqdouj/dotnet.demo).

## Customization (JS Interop)
If there is some functionality not yet supported or you need to customize existing support
you can use your own custom [Blazor JS Interop](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/).
An example of one of the methods to do this is in the demo.

## Documentation
- [Go to Docs](docs/ReadMe.md)

## Setup
- See the [Configuration](docs/Configuration.md) section in the docs.

## Release Notes
### 10.0.1
- Update NuGet packages.

### 10.0.0
- Initial release.
