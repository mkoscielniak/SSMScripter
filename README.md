# SSMScripter 

## Description

SSMScripter is addin (plugin) for Microsoft SQL Management Studio which gives you ability 
to script database objects directly from SQL text editor or result grid into new editor window.
It is also capable to run external program in currently used database context.

## Features:

* Works with:
	* SSMS 2012
	* SSMS 2014
	* SSMS 16
	* SSMS 17
	* SSMS 18
* SSMScripterScript
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
	* Script by cursor position or provided text selection
* SSMScripterRun
	* Open external program with additional arguments provided by database context:
		* `$(Server)` - db server name ex INSTANCE/SQL2018
		* `$(Database)` - db name
		* `$(User)` - connected user name or domain user name
		* `$(Password)` - user password if provided
	* Binds to `Ctrl+F12`
	* Run from Object Explorer tree elements or SQL editor windows
* Its configurable from SSMS Options window (SSMScripter section)

## Known issues
Default key binding may not work after installation in SSMS 18. Please enter Options in SSMS and manually bind desired keys to commands.

## Installation

* Download release build (https://github.com/mkoscielniak/SSMScripter/releases)
	* SSMS 2012 - SSMScripter12
	* SSMS 2014 - SSMScripter14
	* SSMS 16 - SSMScripter16
	* SSMS 17 - SSMScripter17
	* SSMS 18 - SSMScripter18
* Turn off SSMS
* Unpack build content into (intermediate folders may not exists). If default SSMS installation path was changed take that into account:
	* SSMS 2012 - `c:\ProgramData\Microsoft\SQL Server Management Studio\11.0\Addins\`
	* SSMS 2014 - `c:\ProgramData\Microsoft\SQL Server Management Studio\12.0\Addins\`
	* SSMS 16 - `c:\Program Files (x86)\Microsoft SQL Server\130\Tools\Binn\ManagementStudio\Extensions\SSMScripter\`
	* SSMS 17 - `c:\Program Files (x86)\Microsoft SQL Server\140\Tools\Binn\ManagementStudio\Extensions\SSMScripter\`
	* SSMS 18 - `c:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Extensions\SSMScripter\`	
* Make sure all unpacked files are not blocked by Windows security (if so unblock them in file Properties)
* Run SSMS
* In SSMS 16 and 17 answer "No" in warning dialog about incorrectly loaded "ScriptCommandPackage" and then restart SSMS instance (warning appears only during first run after instalation)