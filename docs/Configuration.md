## DotNet.AzureMaps.Blazor Documentation - Configuration

### [<- Go Back](ReadMe.md)

### [App.Razor](../src/MainLib/Sandbox/Components/App.razor)
- Add the Azure Maps SDK scripts to the `head`.
- Then add any optional Azure Maps SDK [scripts](https://github.com/Azure-Samples/AzureMapsCodeSamples/tree/main/Static/lib/azure-maps) (i.e. azure-maps-animations.min.js, etc.)
- Add the `marqdouj-azuremaps-blazor.js` to the `body` after the `_framework/blazor.web.js` script.
- If required, then add the anonymous authentication script.
- If required, then add the SasToken authentication script.

### [AzMapsSetup.cs](../src/MainLib/Sandbox/AzMapsSetup.cs)
`AzMapsSetup.cs` contains examples of all the supported authentication methods.

NOTE: For SasToken authentication, you can provide a SasTokenUrl instead
of configuring the GetSasToken callback in App.Razor.

### [Program.cs](../src/MainLib/Sandbox/Program.cs)
This is where you configure the authentication and global map settings.
```csharp
//Using AzMapsSetup.cs
builder.Services.ConfigureMarqdoujAzMaps(builder.Configuration);
```
