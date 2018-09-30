
<#
.Synopsis
	Example build script with a conditional task.

.Description
	The Conditional task is typical for Debug|Release configuration scenarios.
	Such values as $Configuration are normally defined as parameters.
	In this example the task Conditional is invoked for Release.

	The default task depends on the Conditional and tests whether it is called.

	Another task TestScriptCondition tests the task ScriptCondition where the
	condition is defined as a script block which is evaluated on invocation.

	The script also shows how to use script variables shared between tasks.

.Example
	# Debug configuration
	Invoke-Build . Conditional.build.ps1 -Configuration Debug

.Example
	# Release configuration
	Invoke-Build . Conditional.build.ps1 -Configuration Release
#>

param(
	$Configuration = 'Release'
)

. .\Shared.ps1

$BeforeConditional = 'To do.'
$AfterConditional = 'To do.'
$Conditional = 'To do.'

# Test of the default parameter value (called from .build.ps1)
task TestDefaultParameter {
	equals $Configuration Release
}

# These tasks are referenced by the Conditional
task BeforeConditional { $script:BeforeConditional = 'Done.' }
task AfterConditional { $script:AfterConditional = 'Done.' }

# This task is called if the configuration is Release
task Conditional -If ($Configuration -eq 'Release') BeforeConditional, { $script:Conditional = 'Done.' }, AfterConditional

# The default task tests whether the Conditional is called depending on the configuration.
task . Conditional, {
	switch($Configuration) {
		'Debug' {
			equals $BeforeConditional 'To do.'
			equals $AfterConditional 'To do.'
			equals $Conditional 'To do.'
			'Tested Debug.'
		}
		'Release' {
			equals $BeforeConditional 'Done.'
			equals $AfterConditional 'Done.'
			equals $Conditional 'Done.'
			'Tested Release.'
		}
		default {
			throw 'Invalid Configuration.'
		}
	}
}

# Unlike the Conditional, this tasks defines the If condition as a script block
$ScriptCondition = $false
$ScriptConditionCount = 0
task ScriptCondition -If { $ScriptCondition } {
	++$script:ScriptConditionCount
}

# These tasks call the ScriptCondition
task ScriptConditionUser1 ScriptCondition
task ScriptConditionUser2 ScriptCondition
task ScriptConditionUser3 ScriptCondition

# This task tests the ScriptCondition
task TestScriptCondition @(
	# calls ScriptCondition indirectly, it is not invoked due to its condition evaluated to false
	'ScriptConditionUser1'
	{
		assert ($script:ScriptConditionCount -eq 0) 'Conditional task is not yet called.'
		$script:ScriptCondition = $true
	}
	# calls ScriptCondition indirectly again and now it is invoked, its condition is true now
	'ScriptConditionUser2'
	{
		assert ($script:ScriptConditionCount -eq 1) 'Conditional task is called once.'
	}
	# calls ScriptCondition indirectly again, it is not invoked, tasks are invoked once
	'ScriptConditionUser3'
	{
		assert ($script:ScriptConditionCount -eq 1) 'Conditional task is still called once.'
	}
)

# The If script fails.
task ScriptConditionFails -If { throw 'If fails.' } { throw }

# Test errors.
task ConditionalErrors (job ScriptConditionFails -Safe), {
	Test-Error ScriptConditionFails "If fails.*At *\Conditional.build.ps1*'If fails.'*"
}
