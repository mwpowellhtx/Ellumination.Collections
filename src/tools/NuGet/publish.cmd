@echo off

setlocal

:set_vars
set nuget_api_key=%my_nuget_api_key%

rem We do not publish the API key as part of the script itself.
if /i "%nuget_api_key%" equ "" (
    echo You are prohibited from making these sorts of changes.
    goto :end
)

rem Default list delimiter is Comma (,).
:redefine_delim
if /i "%delim%" equ "" (
    set delim=,
)
rem Redefine the delimiter when a Dot (.) is discovered.
rem Anticipates potentially accepting Delimiter as a command line arg.
if /i "%delim%" equ "." (
    set delim=
    goto :redefine_delim
)

rem Go ahead and pre-seed the Projects up front.
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
set all_projects=%all_projects%%delim%Ellumination.Collections.Bidirectionals
set all_projects=%all_projects%%delim%Ellumination.Collections.Stacks
set all_projects=%all_projects%%delim%Ellumination.Collections.Queues
set all_projects=%all_projects%%delim%Ellumination.Collections.Deques
set all_projects=%all_projects%%delim%Ellumination.Collections.Variants
set all_projects=%all_projects%%delim%Ellumination.Collections.Sets
set all_projects=%all_projects%%delim%Ellumination.Collections.ConcurrentList
rem Setup Variants Projects
set variants_projects=Ellumination.Collections.Variants
rem Setup Collections Projects
set collections_projects=Ellumination.Collections.Bidirectionals
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Stacks
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Queues
set collections_projects=%collections_projects%%delim%Ellumination.Collections.Deques
rem Setup Stacks Projects
set stacks_projects=Ellumination.Collections.Stacks
rem Setup Queues Projects
set queues_projects=Ellumination.Collections.Queues
rem Setup Deques Projects
set deques_projects=Ellumination.Collections.Deques
rem Setup Bit Array Projects
set bit_array_projects=Ellumination.Collections.ImmutableBitArray
rem Setup Enumerations Projects
set enum_projects=Ellumination.Collections.ImmutableBitArray
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations
set enum_projects=%enum_projects%%delim%Ellumination.Collections.Enumerations.Tests
rem Setup Enumeration Tooling Projects
set enum_tooling_projects=Ellumination.Collections.Enumerations.Attributes
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.Collections.Enumerations.Analyzers
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.Collections.Enumerations.Generators
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.Collections.Enumerations.BuildTime
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.CodeAnalysis.Verification
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.CodeAnalysis.Verifiers.CodeFixes
set enum_tooling_projects=%enum_tooling_projects%%delim%Ellumination.CodeAnalysis.Verifiers.Diagnostics
rem Setup Bidirectional Projects
set bidi_projects=Ellumination.Collections.Bidirectionals
rem Setup All Collections Core Projects
set collections_core_projects=Ellumination.Collections.Core
set collections_core_projects=%collections_core_projects%%delim%Ellumination.Collections.Sets
set collections_core_projects=%collections_core_projects%%delim%Ellumination.Collections.ConcurrentList
rem Setup Core Projects
set core_projects=Ellumination.Collections.Core
rem Setup Set Projects
set set_projects=Ellumination.Collections.Sets
rem Setup ConcurrentList Projects
set concurrent_list_projects=Ellumination.Collections.ConcurrentList

:parse_args

:no-pause
if /i "%1" equ "--no-pause" (
    set no_pause=1
    goto :next_arg
)

:set_drive_letter
if /i "%1" equ "--drive-letter" (
    set publish_local_drive=%2
    shift
    goto :next_arg
)
if /i "%1" equ "--drive" (
    set publish_local_drive=%2
    shift
    goto :next_arg
)

:set_destination
if /i "%1" equ "--nuget" (
    set destination=nuget
    goto :next_arg
)
if /i "%1" equ "--local" (
    set destination=local
    goto :next_arg
)

:set_dry_run
if /i "%1" equ "--dry" (
    set dry=true
    goto :next_arg
)
if /i "%1" equ "--dry-run" (
    set dry=true
    goto :next_arg
)
if /i "%1" equ "--wet" (
    set dry=false
    goto :next_arg
)
if /i "%1" equ "--wet-run" (
    set dry=false
    goto :next_arg
)

:set_config
if /i "%1" equ "--config" (
    set config=%2
    shift
    goto :next_arg
)

:add_bit_array_projects
if /i "%1" equ "--bits" (
    if /i "%projects%" equ "" (
        set projects=%bit_array_projects%
    ) else (
        set projects=%projects%%delim%%bit_array_projects%
    )
    goto :next_arg
)
if /i "%1" equ "--bit-array" (
    if /i "%projects%" equ "" (
        set projects=%bit_array_projects%
    ) else (
        set projects=%projects%%delim%%bit_array_projects%
    )
    goto :next_arg
)

:add_enum_projects
if /i "%1" equ "--enums" (
    if /i "%projects%" equ "" (
        set projects=%enum_projects%
    ) else (
        set projects=%projects%%delim%%enum_projects%
    )
    goto :next_arg
)
if /i "%1" equ "--enumerations" (
    if /i "%projects%" equ "" (
        set projects=%enum_projects%
    ) else (
        set projects=%projects%%delim%%enum_projects%
    )
    goto :next_arg
)

:add_enum_tooling_projects
if /i "%1" equ "--enum-tools" (
    if /i "%projects%" equ "" (
        set projects=%enum_tooling_projects%
    ) else (
        set projects=%projects%%delim%%enum_tooling_projects%
    )
    goto :next_arg
)
if /i "%1" equ "--enumeration-tools" (
    if /i "%projects%" equ "" (
        set projects=%enum_tooling_projects%
    ) else (
        set projects=%projects%%delim%%enum_tooling_projects%
    )
    goto :next_arg
)

:add_bidi_projects
if /i "%1" equ "--bidi" (
    if /i "%projects%" equ "" (
        set projects=%bidi_projects%
    ) else (
        set projects=%projects%%delim%%bidi_projects%
    )
    goto :next_arg
)

if /i "%1" equ "--bidirectional" (
    if /i "%projects%" equ "" (
        set projects=%bidi_projects%
    ) else (
        set projects=%projects%%delim%%bidi_projects%
    )
    goto :next_arg
)

:add_core_projects
if /i "%1" equ "--all-collections-core" (
    if /i "%projects%" equ "" (
        set projects=%collections_core_projects%
    ) else (
        set projects=%projects%%delim%%collections_core_projects%
    )
)

:add_stack_projects
if /i "%1" equ "--stacks" (
    if /i "%projects%" equ "" (
        set projects=%stacks_projects%
    ) else (
        set projects=%projects%%delim%%stacks_projects%
    )
)

:add_queue_projects
if /i "%1" equ "--queues" (
    if /i "%projects%" equ "" (
        set projects=%queues_projects%
    ) else (
        set projects=%projects%%delim%%queues_projects%
    )
)

:add_deque_projects
if /i "%1" equ "--deques" (
    if /i "%projects%" equ "" (
        set projects=%deques_projects%
    ) else (
        set projects=%projects%%delim%%deques_projects%
    )
)

:add_core_projects
if /i "%1" equ "--core" (
    if /i "%projects%" equ "" (
        set projects=%core_projects%
    ) else (
        set projects=%projects%%delim%%core_projects%
    )
)

:add_sets_projects
if /i "%1" equ "--sets" (
    if /i "%projects%" equ "" (
        set projects=%set_projects%
    ) else (
        set projects=%projects%%delim%%set_projects%
    )
)

:add_concurrent_list_projects
if /i "%1" equ "--concur" (
    if /i "%projects%" equ "" (
        set projects=%concurrent_list_projects%
    ) else (
        set projects=%projects%%delim%%concurrent_list_projects%
    )
)
if /i "%1" equ "--concurrent" (
    if /i "%projects%" equ "" (
        set projects=%concurrent_list_projects%
    ) else (
        set projects=%projects%%delim%%concurrent_list_projects%
    )
)
if /i "%1" equ "--concurrent-lists" (
    if /i "%projects%" equ "" (
        set projects=%concurrent_list_projects%
    ) else (
        set projects=%projects%%delim%%concurrent_list_projects%
    )
)

:add_collections_projects
if /i "%1" equ "--collections" (
    if /i "%projects%" equ "" (
        set projects=%collections_projects%
    ) else (
        set projects=%projects%%delim%%collections_projects%
    )
    goto :next_arg
)

:add_variants_projects
if /i "%1" equ "--variant" (
    if /i "%projects%" equ "" (
        set projects=%variants_projects%
    ) else (
        set projects=%projects%%delim%%variants_projects%
    )
    goto :next_arg
)
if /i "%1" equ "--variants" (
    if /i "%projects%" equ "" (
        set projects=%variants_projects%
    ) else (
        set projects=%projects%%delim%%variants_projects%
    )
    goto :next_arg
)

:add_all_projects
if /i "%1" equ "--all" (
    set projects=%all_projects%
    goto :next_arg
)

:add_project
if /i "%1" equ "--project" (
    if /i "%projects%" equ "" (
        set projects=%2
    ) else (
        set projects=%projects%%delim%%2
    )
    shift
    goto :next_arg
)

:next_arg

shift

if /i "%1" equ "" goto :end_args

goto :parse_args

:end_args

:verify_args

:verify_dry
rem Assumes we want a Live (Wet) Run when unspecified.
if /i "%dry%" equ "" set dry=false

:verify_destination
if /i "%destination%" equ "" set destination=local

:verify_config
rem Assumes Release Configuration when not otherwise specified.
if /i "%config%" equ "" set config=Release

:publish_projects

:set_vars

set xcopy_exe=xcopy.exe
set nuget_exe=NuGet.exe

set nupkg_ext=.nupkg
if "%publish_local_drive%" == "" set publish_local_drive=E:
set publish_local_dir=%publish_local_drive%\Dev\NuGet\local\packages

rem Expecting NuGet to be found in the System Path.
set nuget_api_src=https://api.nuget.org/v3/index.json
set nuget_push_verbosity=detailed

set nuget_push_opts=%nuget_push_opts% %nuget_api_key%
set nuget_push_opts=%nuget_push_opts% -Verbosity %nuget_push_verbosity%
set nuget_push_opts=%nuget_push_opts% -NonInteractive
set nuget_push_opts=%nuget_push_opts% -Source %nuget_api_src%

rem Do the main areas here.
pushd ..\..

if not "%projects%" equ "" (
    echo Processing '%config%' configuration for '%projects%' ...
)
:next_project
if not "%projects%" equ "" (
    for /f "tokens=1* delims=%delim%" %%p in ("%projects%") do (
        call :publish_pkg %%p
        set projects=%%q
        goto :next_project
    )
)

popd

goto :end

:publish_pkg
for %%f in ("%1\bin\%config%\%1*%nupkg_ext%") do (

    if /i "%destination%-%dry%" equ "local-true" (
        echo Set to copy "%%f" to "%publish_local_dir%".
    )

    if /i "%destination%-%dry%" equ "local-false" (
        if not exist "%publish_local_dir%" mkdir "%publish_local_dir%"
        echo Copying "%%f" package to local directory "%publish_local_dir%" ...
        %xcopy_exe% /q /y "%%f" "%publish_local_dir%"
    )

    if /i "%destination%-%dry%" equ "nuget-true" (
        echo Dry run: %nuget_exe% push "%%f"%nuget_push_opts%
    )

    if /i "%destination%-%dry%" equ "nuget-false" (
        echo Running: %nuget_exe% push "%%f"%nuget_push_opts%
        %nuget_exe% push "%%f"%nuget_push_opts%
    )
)
exit /b

:end

if /i "%no_pause%" equ "1" (
    goto :fini
)

pause

:fini

endlocal
