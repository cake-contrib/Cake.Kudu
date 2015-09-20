@echo off
IF NOT EXIST "Tools" (md "Tools")
nuget install Cake -ExcludeVersion -OutputDirectory "Tools" -PreRelease -Source "https://www.myget.org/F/cake"
Tools\Cake\Cake.exe -version
Tools\Cake\Cake.exe deploy.cake -verbosity=Verbose
