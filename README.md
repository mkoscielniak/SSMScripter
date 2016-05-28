SSMScripter 
===========

Description
-------------
SSMScripter is addin (plugin) for Microsoft SQL Management Studio which gives you ability 
to script database objects directly from SQL text editor or result grid into new editor window.

Main features:

* Scripts database objects:
	* stored procedures
	* functions
	* views
	* tables
	* triggers
* Adds `Script...` command into:
	* sql text editor context menu
	* result grid context menu
* Binds to `F12` (like VS "Go to definition..." command)
* Works with:
	* SSMS 2012
	* SSMS 2014

Installation
-------------

* Download release build
	* SSMS 2012 - SSMScripter12
	* SSMS 2014 - SSMScripter14
* Turn off SSMS
* Unpack build content into (intermediate folders may not exists):
	* SSMS 2012 - `c:\ProgramData\Microsoft\SQL Server Management Studio\11.0\Addins\`
	* SSMS 2014 - `c:\ProgramData\Microsoft\SQL Server Management Studio\12.0\Addins\`
* Run SSMS