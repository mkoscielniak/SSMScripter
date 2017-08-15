# SSMScripter 

## Description

SSMScripter is addin (plugin) for Microsoft SQL Management Studio which gives you ability 
to script database objects directly from SQL text editor or result grid into new editor window.

## Features:

* Works with:
	* SSMS 2012
	* SSMS 2014
	* SSMS 2016
	* SSMS 2017
* Scripts database objects:
	* stored procedures
	* functions
	* views
	* tables
	* triggers
* Binds to `F12` (like Visual Studio "Go to definition..." command)
* Adds `Script...` command into context menu in SSMS 2012/2014:
	* sql text editor
	* result grid

## Installation

* Download release build
	* SSMS 2012 - SSMScripter12
	* SSMS 2014 - SSMScripter14
	* SSMS 2016 - SSMScripter16
	* SSMS 2017 - SSMScripter17
* Turn off SSMS
* Unpack build content into (intermediate folders may not exists):
	* SSMS 2012 - `c:\ProgramData\Microsoft\SQL Server Management Studio\11.0\Addins\`
	* SSMS 2014 - `c:\ProgramData\Microsoft\SQL Server Management Studio\12.0\Addins\`
	* SSMS 2016 - `c:\Program Files (x86)\Microsoft SQL Server\130\Tools\Binn\ManagementStudio\Extensions\SSMScripter\`
	* SSMS 2017 - `c:\Program Files (x86)\Microsoft SQL Server\140\Tools\Binn\ManagementStudio\Extensions\SSMScripter\`
* Make sure all unpacked files are not blocked by Windows security (if so unblock them in file Properties)
* Run SSMS
* Answer "No" in warning dialog about incorrectly loaded "ScriptCommandPackage" and then restart SSMS instance (warning appears only during first run after instalation in 2016/2017 versions)