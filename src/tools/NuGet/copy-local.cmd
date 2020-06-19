@echo off

setlocal

echo Use the publish.cmd batch file from now on for local publishing.
goto :end

rem TODO: TBD: the R# xUnit testing saga is as of this moment still unresolved
rem TODO: TBD: specifically concerning treatment of user furnished xUnit settings
rem TODO: TBD: working to move past this using Visual Studio Test Explorer, console runner, etc
rem TODO: TBD: although it would be very good to have that capability in sound working order from the jetbrains folks sooner than later

:set_vars
rem set local_dir=C:\Dev\NuGet\packages
set local_dir=G:\Dev\NuGet\local\packages
set nupkg_ext=.nupkg

rem Default list delimiter is Comma (,).
:redefine_delim
if "%delim%" == "" (
    set delim=,
)
rem Redefine the delimiter when a Dot (.) is discovered.
rem Anticipates potentially accepting Delimiter as a command line arg.
if "%delim%" == "." (
    set delim=
    goto :redefine_delim
)

:set_projects
set projects=
rem Setup All Projects
set all_projects=Ellumination.Collections.ImmutableBitArray
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations.Attributes
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations.Analyzers
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations.Generators
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations.BuildTime
set all_projects=%all_projects%%delim%Ellumination.CodeAnalysis.Verification
set all_projects=%all_projects%%delim%Ellumination.CodeAnalysis.Verifiers.CodeFixes
set all_projects=%all_projects%%delim%Ellumination.CodeAnalysis.Verifiers.Diagnostics
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations
set all_projects=%all_projects%%delim%Ellumination.Collections.Enumerations.Tests
set all_projects=%all_projects%%delim%Ellumination.Collections.Stacks
set all_projects=%all_projects%%delim%Ellumination.Collections.Queues
set all_projects=%all_projects%%delim%Ellumination.Collections.Deques
rem Setup Collections Projects
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Stacks
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Queues
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Deques
rem Setup Enumerations Projects
set enum_projects=Ellumination.Collections.ImmutableBitArray
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.Attributes
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.Analyzers
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.Generators
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.BuildTime
set enum_projects=%enum_projects%%delim%Ellumination.CodeAnalysis.Verification
set enum_projects=%enum_projects%%delim%Ellumination.CodeAnalysis.Verifiers.CodeFixes
set enum_projects=%enum_projects%%delim%Ellumination.CodeAnalysis.Verifiers.Diagnostics
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.Tests
rem Setup Bit Array Projects
set bit_array_projects=Ellumination.Collections.ImmutableBitArray

:parse_args

if "%1" == "" (
    goto :end_args
)

:set_dry_run
if "%1" == "--dry" (
    set dry=true
    goto :next_arg
)
if "%1" == "--dry-run" (
    set dry=true
    goto :next_arg
)

:set_config
rem echo set_config = %1
if "%1" == "--config" (
    set config=%2
    shift
    goto :next_arg
)

:add_enum_projects
rem echo add_enum_projects = %1
if "%1" == "--enums" (
    if "%projects%" == "" (
        set projects=%enum_projects%
    ) else (
        set projects=%projects%%delim%%enum_projects%
    )
	goto :next_arg
)

if "%1" == "--enumerations" (
    if "%projects%" == "" (
        set projects=%enum_projects%
    ) else (
        set projects=%projects%%delim%%enum_projects%
    )
	goto :next_arg
)

:add_bit_array_projects
rem echo add_bit_array_projects = %1
if "%1" == "--bits" (
    if "%projects%" == "" (
        set projects=%bit_array_projects%
    ) else (
        set projects=%projects%%delim%%bit_array_projects%
    )
	goto :next_arg
)

if "%1" == "--bit-array" (
    if "%projects%" == "" (
        set projects=%bit_array_projects%
    ) else (
        set projects=%projects%%delim%%bit_array_projects%
    )
	goto :next_arg
)

:add_collections_projects
rem echo add_collections_projects = %1
if "%1" == "--collections" (
    rem Prepare to publish Collections Projects.
    if "%projects%" == "" (
        set projects=%collections_projects%
    ) else (
        set projects=%projects%%delim%%collections_projects%
    )
    goto :next_arg
)

:add_all_projects
rem echo add_all_projects = %1
if "%1" == "--all" (
    set projects=%all_projects%
	goto :next_arg
)

:add_project
rem echo add_project = %1
if "%1" == "--project" (
    if "%projects%" == "" (
        set projects=%2
    ) else (
        set projects=%projects%%delim%%2
    )
    shift
    goto :next_arg
)

:next_arg
shift
goto :parse_args

:end_args

:verify_args

:verify_projects
if "%projects%" == "" (
    rem In which case, there is nothing else to do.
    echo Nothing to process.
    goto :end
)

:verify_config
if "%config%" == "" (
    set config=Release
)

:process_projects
rem Do the main areas here.
pushd ..\..

rem This is an interesting package for local consumption, but I do not think
rem it should be published in a broader context, at least not yet.
if not "%projects%" == "" (
    echo Processing '%config%' configuration for '%projects%' ...
)
:next_project
if not "%projects%" == "" (
    for /f "tokens=1* delims=%delim%" %%p in ("%projects%") do (
        rem Processing now as a function of input arguments.
        call :copy_local %%p
        set projects=%%q
    )
    goto :next_project
)

popd

goto :end

:copy_local
pushd %1\bin\%config%
rem Because Package may exist in the %config%\%TargetFramework% ...
set package_path=%1.*%nupkg_ext%
rem We need to scan beyond the wildcard package path here and get to specifics.
for /r %%x in ("%package_path%") do (
    if not exist "%%x" (
        goto :copy_not_found
    )
    if "%dry%" == "true" (
        echo Set to copy '%%x' to '%local_dir%' ...
    ) else (
        echo Wet run '%%x' ...
        rem if not exist "%local_dir%" mkdir "%local_dir%"
        rem rem Make sure that we do not inadvertently overwrite already existing package versions.
        rem if not exist "%local_dir%\%%x" echo Copying '%%x' package to local directory '%local_dir%'...
        rem if not exist "%local_dir%\%%x" xcopy /Y "%%x" "%local_dir%"
    )
    goto :end_copy
)
:copy_not_found
echo Package '%package_path%' not found.
:end_copy
popd
exit /b

:end

endlocal

pause
