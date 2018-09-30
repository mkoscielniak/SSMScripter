Set-BuildHeader {
	param($Path)	
	'-' * 119
	Write-Build White "Task $Path : $(Get-BuildSynopsis $Task)"
	Write-Build DarkGray "$($Task.InvocationInfo.ScriptName):$($Task.InvocationInfo.ScriptLineNumber)"
	'-' * 119		
}

Set-Alias MSBuild (Resolve-MSBuild)

$base_path = (Resolve-Path .)

function Clean-Build ($ver) {
	Remove-Item $base_path\SSMScripter\bin\$ver -Recurse -Force -ErrorAction 0
	Remove-Item $base_path\SSMScripter\obj\$ver -Recurse -Force -ErrorAction 0
}

function Build-Project($ver) {
	exec { MSBuild $base_path\SSMScripter\SSMScripter$ver.csproj /t:Build /p:Configuration=Release /v:quiet /nologo }
}

# Synopsis: Shows instruction
task Info {
	Write-Host Gray 'Type "run task-name"'
}

# Synopis: Cleans 2012 version
task Clean12 {
	Clean-Build 12
}

# Synopis: Cleans 2014 version
task Clean14 {
	Clean-Build 14
}

# Synopis: Cleans 2016 version
task Clean16 {
	Clean-Build 16
}

# Synopis: Cleans 2017 version
task Clean17 {
	Clean-Build 17
}

# Synopsis: Builds 2012 version
task Build12 {
	Build-Project 12
}

# Synopsis: Builds 2014 version
task Build14 {
	Build-Project 14
}

# Synopsis: Builds 2016 version
task Build16 {
	Build-Project 16
}

# Synopsis: Builds 2017 version
task Build17 {
	Build-Project 17
}