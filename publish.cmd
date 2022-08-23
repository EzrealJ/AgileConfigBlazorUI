set output=.\release
if exist "%output%" rd /S /Q "%output%"
dotnet publish .\src\AgileConfig.BlazorUI\AgileConfig.BlazorUI.csproj -c Release -o %output%\dist
XCopy %output%\dist\wwwroot %output%\dist /E/H/C/I
rd /S /Q %output%\dist\wwwroot
7z a %output%/dist.7z %output%/dist/*
dotnet publish .\src\AgileConfig.BlazorUI.SimpleHost\AgileConfig.BlazorUI.SimpleHost.csproj -c Release -o %output%\simple
7z a %output%/simple.7z %output%/simple/*
dotnet publish .\src\AgileConfig.WindowsApp\AgileConfig.WindowsApp.csproj -c Release --self-contained -r win-x64 -o %output%\winapp
7z a %output%/winapp.7z %output%/winapp/*

