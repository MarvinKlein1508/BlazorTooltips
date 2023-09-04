# BlazorTooltips
A simple to use blazor component to implement bootstrap tooltips in both Blazor server and Blazor WebAssembly apps. Based on Bootstrap.

See a live [demo](https://marvinklein1508.github.io/BlazorTooltips/) right here on github.

## What's the difference between the default bootstrap tooltips?
The component is designed to manage the disposing of the tooltips. Once the component is not rendered anymore, it will be automatically disposed and hide the tooltip completely automatically.

With the vanilla implementation of Bootstrap tooltips you'll end up with an open tooltip after the page has navigated the user to a different component.

Moreover it provides you with nice intellisense, so you'll don't need to remember every attribute.

## Requirements
The bootstrap.min.js must be included in your project.

In blazor server you can add it within your `_Host.cshtml` file

```html
<script src="/js/bootstrap.js"></script>
<script src="_framework/blazor.server.js"></script>
```

In Blazor WebAssembly you'll need to add it to your `index.html`

```html
<script src="/js/bootstrap.js"></script>
<script src="_framework/blazor.webassembly.js"></script>
```


## Installation

You can install from Nuget using the following command:

`Install-Package BlazorTooltips`

Or via the Visual Studio package manger.

## Basic usage
Start by add the following using statement to your root `_Imports.razor`.

    @using BlazorTooltips


You can wrap the tooltip component around any HTML or blazor component. For example:
```csharp
<Tooltip Title="Default Tooltip">
    <button type="button" class="btn btn-primary">Default Tooltip</button>
</Tooltip>
```

By default all tooltips will be shown on hover with a fade animation and placed on top. You can change this behaviour if you want like this:
```csharp
<Tooltip Title="Advanced tooltip" 
         Placement="TooltipPlacement.Bottom" 
         Animation="false">
    <button type="button" class="btn btn-primary">Advanced tooltip</button>
</Tooltip>
```

The component also provides native support for HTML tooltips. If you don't want to show HTML within your tooltips you can disable this feature by setting `Html` to `false`

```csharp
<Tooltip Title="<strong><i>Tooltip with HTML</i></strong>" Html="false">
    <button type="button" class="btn btn-primary">Tooltip with HTML</button>
</Tooltip>
```

You can override almost any available option from this component by passing it down to the corresponding parameter. 
