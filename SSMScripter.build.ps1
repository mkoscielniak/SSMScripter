Set-BuildHeader {
	param($Path)	
	'-' * 119
	Write-Build White "Task $Path : $(Get-BuildSynopsis $Task)"
	Write-Build DarkGray "$($Task.InvocationInfo.ScriptName):$($Task.InvocationInfo.ScriptLineNumber)"
	'-' * 119		
}

$MSBuild  = (Resolve-MSBuild 'x86')
$base_path = (Resolve-Path .)

function Remove-PathIfExists ($path) {
	if(Test-Path $path) {
		Remove-Item $path -Recurse -Force -ErrorAction 0
	}
}

function Clean-Build ($ver) {
	Remove-PathIfExists $base_path\SSMScripter\bin\$ver	
	Remove-PathIfExists $base_path\SSMScripter\obj\$ver
	Remove-PathIfExists $base_path\SSMScripter.VSPackage\bin\$ver
	Remove-PathIfExists $base_path\SSMScripter.VSPackage\obj\$ver
}

function Init-BuildDir($ver) {
	Remove-PathIfExists $base_path\Build\$ver
	New-Item -ItemType directory $base_path\Build\$ver	
}

function Build-ProjectLib($ver) {
	exec { & $MSBuild $base_path\SSMScripter\SSMScripter$ver.csproj /t:Build /p:Configuration=Release /v:quiet /nologo }	
}

function Copy-ProjectLibBuild($ver) {
	Init-BuildDir $ver
	Copy-Item $base_path\SSMScripter\bin\$ver\Release\SSMScripter$ver.dll $base_path\Build\$ver
	Copy-Item $base_path\SSMScripter\bin\$ver\Release\SSMScripter$ver.AddIn $base_path\Build\$ver
}

function Build-ProjectPkg($ver) {
	exec { & $MSBuild $base_path\SSMScripter.VSPackage\SSMScripter$ver.VSPackage.csproj /t:Build /p:Configuration=Release /v:quiet /nologo }	
}

function Copy-ProjectPkgBuild($ver) {
	Init-BuildDir $ver
	Copy-Item $base_path\SSMScripter.VSPackage\bin\$ver\Release\SSMScripter$ver.dll $base_path\Build\$ver
	Copy-Item $base_path\SSMScripter.VSPackage\bin\$ver\Release\SSMScripter$ver.VSPackage.dll $base_path\Build\$ver
	Copy-Item $base_path\SSMScripter.VSPackage\bin\$ver\Release\SSMScripter$ver.VSPackage.pkgdef $base_path\Build\$ver
}

function Release-ProjectBuild($ver) {
	$verinf = (Get-Item $base_path\Build\$ver\SSMScripter$ver.dll).VersionInfo
	$verstr = ("{0}.{1}" -f $verinf.FileMajorPart, $verinf.FileMinorPart)	
	Compress-Archive $base_path\Build\$ver\*  $base_path\Build\SSMScripter$($ver)_v$verstr.zip -Force
}

# Synopsis: Shows instruction
task Info {
	Write-Host Gray 'Type "run task-name"'	
}

# Synopsis: Builds all versions
task Build Build12, Build14, Build16, Build17, Build18, {
}

# Synopsis: Cleans all versions
task Clean Clean12, Clean14, Clean16, Clean17, Clean18, {
}

# Synopsis: Rebuild all versions
task Rebuild Clean, Build, {
}

# Synopsis: Release all versions
task Release Rebuild, {
	Release-ProjectBuild 12
	Release-ProjectBuild 14
	Release-ProjectBuild 16
	Release-ProjectBuild 17
	Release-ProjectBuild 18
}

# Synopsis: Cleans 12 version
task Clean12 {
	Clean-Build 12
}

# Synopsis: Cleans 14 version
task Clean14 {
	Clean-Build 14
}

# Synopsis: Cleans 16 version
task Clean16 {
	Clean-Build 16
}

# Synopsis: Cleans 17 version
task Clean17 {
	Clean-Build 17
}

# Synopsis: Cleans 18 version
task Clean18 {
	Clean-Build 18
}

# Synopsis: Builds 12 version
task Build12 {
	Build-ProjectLib 12
	Copy-ProjectLibBuild 12
}

# Synopsis: Builds 14 version
task Build14 {
	Build-ProjectLib 14
	Copy-ProjectLibBuild 14
}

# Synopsis: Builds 16 version
task Build16 {
	Build-ProjectPkg 16
	Copy-ProjectPkgBuild 16
}

# Synopsis: Builds 17 version
task Build17 {
	Build-ProjectPkg 17
	Copy-ProjectPkgBuild 17
}

# Synopsis: Builds 18 version
task Build18 {
	Build-ProjectPkg 18
	Copy-ProjectPkgBuild 18
}