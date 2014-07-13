properties {
	$NugetExe = Resolve-Path "Nuget.exe"
	$BaseDir = Resolve-Path "..\"
	$SolutionFile = "$BaseDir\DefiantCode.Components.sln"
	$BuildOutput = "$BaseDir\DefiantCode.Data.Azure\bin\Debug"
	$ProjectPath = "$BaseDir\DefiantCode.Data.Azure\DefiantCode.Data.AzureStorage.csproj"
	
	$ArchiveDir = "$BaseDire\Archive"
	$NuGetPackageName = "DefiantCode.Data.AzureStorage"
}

. .\common.ps1

function OnArchiving {
}
