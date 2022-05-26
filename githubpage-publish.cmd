set output=.\docs
if exist "%output%" rd /S /Q "%output%"
dotnet publish .\src\AgileConfig.BlazorUI\AgileConfig.BlazorUI.csproj -c Release -o %output%
XCopy %output%\wwwroot %output% /E/H/C/I
rd /S /Q %output%\wwwroot