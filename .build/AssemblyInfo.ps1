
function Help {

	"Sets the AssemblyVersion and AssemblyFileVersion of AssemblyInfo.cs files`n"
	".\SetAssemblyVersion.ps1 [VersionNumber] -path [SearchPath]`n"
	"   [VersionNumber]     The version number to set, for example: 1.1.9301.0"
	"   [SearchPath]        The path to search for AssemblyInfo files.`n"
}

function Update-SourceVersion {

	[CmdletBinding(
		SupportsShouldProcess=$true
	)]
	
	Param 
	(	
		[string]$file,
		[string]$version
	)
	
	PROCESS {

		$NewVersion = 'AssemblyVersion("' + $version + '")';
		$NewFileVersion = 'AssemblyFileVersion("' + $version + '")';

		$assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
		$fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
		$assemblyVersion = 'AssemblyVersion("' + $version + '")';
		$fileVersion = 'AssemblyFileVersion("' + $version + '")';

		if($file)
		{
			(Get-Content $file) | ForEach-Object  { 
				% {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
				% {$_ -replace $fileVersionPattern, $fileVersion }
			} | Out-File $file -encoding UTF8 -force
		}
	}
}

function Update-AllAssemblyInfoFiles {

	[CmdletBinding(
		SupportsShouldProcess=$true
	)]
	
	Param ($version)
	
	PROCESS {

		foreach ($file in "Properties/AssemblyInfo.cs", "Properties/AssemblyInfo.vb" ) 
		{
			get-childitem $path -recurse |? {$_.Name -eq $file} | ForEach-Object { Update-SourceVersion $file $version ; }
		}
	}
}


function Get-NewVersionNumber {

	[CmdletBinding(
	SupportsShouldProcess=$true
	)]

	Param ($BuildNumber)

	PROCESS {

		$Segments = $BuildNumber.Split(".")

		$Major = $Segments[0]
		$Minor = $Segments[1]

		$TodaysDate = (Get-Date) -as [DateTime]
		$BeginDate = (Get-Date -Date '1/1/2000') -as [DateTime]

		$Build =  ($TodaysDate - $BeginDate).Days

		$h = ([int](Get-Date -UFormat "%H") * 60 * 60)
		$m = ([int](Get-Date -UFormat "%M") * 60)
		$s = ([int](Get-Date -UFormat "%S"))
		$Revision = [math]::ceiling(($h + $m + $s) / 2)

		return [string]::Format("{0}.{1}.{2}.{3}", $Major, $Minor, $Build, $Revision)
		
	}
}
