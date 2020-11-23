dotnet nuget push "src\DotNetEasyRandom\bin\Release\HaemmerElectronics.SeppPenner.DotNetEasyRandom.*.nupkg" -s "github" --skip-duplicate
dotnet nuget push "src\DotNetEasyRandom\bin\Release\HaemmerElectronics.SeppPenner.DotNetEasyRandom.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE