#Common NuGet/Archiving logic, not meant ot be executed directly.

$framework = '4.0'

task default -depends Pack

task Init {
	cls
}

task Clean -depends Init {
	
	if (Test-Path $ArchiveDir) {
		ri $ArchiveDir -Recurse
	}
	
}

task Build -depends Init,Clean {
	exec { msbuild $SolutionFile }
}

#This function can be overriden to add additional logic to the archive process.
function OnArchiving {
}

task Pack -depends Build {

	exec { & "$NugetExe" pack $ProjectPath }
}

task Publish -depends Pack {
	$PackageName = gci "$NuGetPackageName.*.nupkg"
	exec { & "$NugetExe" push $PackageName }
}