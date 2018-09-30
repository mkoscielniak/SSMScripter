@echo off
start powershell -Version 2.0 -NoLogo -NoExit -ExecutionPolicy Bypass -Command ^

$console = (Get-Host).UI.RawUI;^
$console.WindowTitle = 'SSMScripter Builder Console';^

$buffer = $console.BufferSize;^
$buffer.Width = 120;^
$buffer.Height = 3000;^
$console.BufferSize = $buffer;^

$maxWindowSize = $console.MaxWindowSize;^
$maxWindowSize.Width = 120;^
$maxWindowSize.Height = 83;^

$size = $console.WindowSize;^
$size.Width = 120;^
$size.Height = 50;^
$console.WindowSize = $size;^

Clear-Host;^

function run([string] $task) {^
	.\Externals\Invoke-Build\Invoke-Build $task^
}^

'-' * 119;^
Write-Host ' SSMScripter Builder Console' -foreground red;^
'-' * 119;^
Write-Host 'Type "run task-name"';^
Write-Host 'Available tasks:';^
run ?;^
