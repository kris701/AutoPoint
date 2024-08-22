<p align="center">
    <img src="https://github.com/user-attachments/assets/f516f36e-e9e1-4c35-bea9-4b5b9f671e94" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/AutoPoint/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/AutoPoint/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/AutoPoint)
![Nuget](https://img.shields.io/nuget/dt/AutoPoint)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/AutoPoint/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/AutoPoint)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

# AutoPoint

This is a project to make it easier to syncronize API endpoints across multiple languages.
Say you have a API and a frontend in two different languages, making sure that the frontend is pointing to the correct endpoints, especially when renaming things, can be quite difficult.
This project tries to make it more static, by generating a static reference file in each language based on an API definition.

As an example, take the definition:
```json
{
  "Includes": [],
  "Branch": {
    "name": "simple",
    "Nodes": [
    ]
  }
}
```
If you then write into a terminal the following

`autopoint -t api.json -p CSharpProducer`

It will output a generated C# file with the static names as follows:
```csharp
// This document is auto generated!
public static class simple {
	public const string Name = "simple";
}
```

The currently available producers are:
* `CSharpProducer`, to make C# code
* `JavaScriptProducer`, to make JS code


This package is available as a tool on the [NuGet Package Manager](https://www.nuget.org/packages/AutoPoint/), so you can install it by writing `dotnet tool install AutoPoint` in a terminal.