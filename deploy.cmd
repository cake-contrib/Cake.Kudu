@echo off
IF NOT EXIST "Tools" (md "Tools")
IF NOT EXIST "Tools\Addins" (md "Tools\Addins")
nuget install Cake -ExcludeVersion -OutputDirectory "Tools" -PreRelease -Source "https://www.myget.org/F/cake"
nuget install KuduSync.NET -ExcludeVersion -OutputDirectory "Tools" -Source "https://www.nuget.org/api/v2/"
nuget install Cake.Kudu -ExcludeVersion -OutputDirectory "Tools\Addins" -PreRelease -Source "https://www.myget.org/f/wcomab"
Tools\Cake\Cake.exe -version
Tools\Cake\Cake.exe deploy.cake -verbosity=Verbose
