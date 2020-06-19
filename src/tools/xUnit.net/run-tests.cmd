@echo off

setlocal

rem TODO: TBD: the R# xUnit testing saga is as of this moment still unresolved
rem TODO: TBD: specifically concerning treatment of user furnished xUnit settings
rem TODO: TBD: working to move past this using Visual Studio Test Explorer, console runner, etc
rem TODO: TBD: although it would be very good to have that capability in sound working order from the jetbrains folks sooner than later

rem Default project delimiter is the Comma (,).
rem Additionally, Delim may be anything, but not a Dot (.).
:redefine_delim
if not defined delim (
    set delim=,
)
rem Do this just in the event we eventually decide to accept command line args.
if "%delim%" == "." (
    set delim=
    goto :redefine_delim
)

set projects=
set framework=
set config_suffix=
set xunit_opts=
set xunit_bin_ext=
set xunit_bin=
set dotnet_exec_prefix=
set deps_json=.deps.json
set runtimeconfig_json=.runtimeconfig.json

rem We cannot really run all unit tests all at once. We must separate our
rem CLR concerns a little bit and divide the work accordingly. Run some
rem net452 tests, then run some netcoreapp2.0 tests, for instance.
:parse_args

rem rem For debugging...
rem echo %%1 = "%1"

if "%1" == "" (
    goto :end_args
)

if "%1" == "--config" (
    rem echo config %2
    set config=%2
    shift
    goto :next_arg
)

if "%1" == "--project" (
    rem echo project %2
    if not defined projects (
        set projects=%2
    ) else (
        set projects=%projects%%delim%%2
    )
    shift
    goto :next_arg
)

rem Now handle some xUnit.net specific options.
if "%1" == "--verbose" (
    rem echo verbose
    if not defined xunit_opts (
        set xunit_opts=-verbose
    ) else (
        set xunit_opts=%xunit_opts% -verbose
    )
    goto :next_arg
)

rem echo parallel %2
if "%1" == "--parallel" (
    call :set_parallel %2
    shift
    goto :next_arg
)

if "%1" == "--xml" (
    rem echo xml %2
    if not defined xunit_opts (
        set xunit_opts=-xml "%2.xml"
    ) else (
        set xunit_opts=%xunit_opts% -xml "%2.xml"
    )
    shift
    goto :next_arg
)

if "%1" == "--framework" (
    call :set_framework %2
    shift
    goto :next_arg
)

if "%1" == "--stop-on-failure" (
    rem echo stop-on-failure
    if not defined xunit_opts (
        set xunit_opts=-stoponfail
    ) else (
        set xunit_opts=%xunit_opts% -stoponfail
    )
    goto :next_arg
)

if "%1" == "--fail-skips" (
    rem echo fail-skips
    if not defined xunit_opts (
        set xunit_opts=-failskips
    ) else (
        set xunit_opts=%xunit_opts% -failskips
    )
    goto :next_arg
)

:set_parallel
set parallel_opts=none%delim%collections%delim%assemblies%delim%all
:next_parallel_opt
if defined parallel_opts (
    for /f "tokens=1* delims=%delim%" %%p in ("%parallel_opts%") do (
        if "%1" == "%%p" (
            if not defined xunit_opts (
                set xunit_opts=-parallel %1
            ) else (
                set xunit_opts=%xunit_opts% -parallel %1
            )
            goto :found_parallel
        )
        set parallel_opts=%%q
        goto :next_parallel_opt
    )
)
:found_parallel
exit /b

:set_framework
rem Do not let the names fool you, for the traditional .NET Framework 4.x paths, use the 'net452' CLR.
set frameworks=net452%delim%netcoreapp1.0%delim%netcoreapp2.0
:next_framework
if defined frameworks (
    for /f "tokens=1* delims=%delim%" %%f in ("%frameworks%") do (
        if "%1" == "%%f" (
            set framework=%1
            goto :found_framework
        )
        set frameworks=%%g
        goto :next_framework
    )
)
:found_framework
exit /b

:next_arg

shift

goto :parse_args

:end_args

:verify_args

:verify_framework
rem Sorts out not only the Target Framework, as pertains to the versions of xUnit.net
rem to invoke, but the scripting bits that support running the unit tests.
set xunit_console_version=2.3.1

set up_dir_prefix=..\..\..\
if not defined framework (
    set framework=net452
    set xunit_bin_ext=.exe
) else (
    rem The up-dir path is one deeper for netcoreapp project outputs.
    if "%framework%" == "netcoreapp1.0" (
        set up_dir_prefix=..\%up_dir_prefix%
        set config_suffix=\%framework%
    ) else if "%framework%" == "netcoreapp2.0" (
        set up_dir_prefix=..\%up_dir_prefix%
        set config_suffix=\%framework%
    ) else (
        rem Let's commit to the xUnit.net file extension at this time.
        set xunit_bin_ext=.exe
    )
)

rem We know this much of the xUnit.net path up front.
set xunit_bin_dir=packages\xunit.runner.console.%xunit_console_version%\tools\%framework%

 rem But we still need to sort out which Extension to use.
if not defined xunit_bin_ext (
    set xunit_bin_ext=.dll
    rem Mind the trailing spaces, which  is  quite  intentional.
    set dotnet_exec_prefix=dotnet exec 
    rem                               ^^ (here in comments for emphasis)
)

set xunit_bin=%xunit_bin_dir%\xunit.console%xunit_bin_ext%

rem Do the main areas here.
:test_projects
pushd ..\..

rem Projects will have already been set, appended, etc.
:next_project
if defined projects (
    for /f "tokens=1* delims=%delim% " %%p in ("%projects%") do (
        rem Processing now as a function of input arguments.
        call :test_one_project %%p
        set projects=%%q
    )
    goto :next_project
)

popd

goto :end

:test_one_project
echo Pushing directory "%1\bin\%config%%config_suffix%" ...
pushd %1\bin\%config%%config_suffix%
if "%framework%" == "net452" (
    rem Undefine the Options in this case.
    set dotnet_exec_opts=
) else (
    rem We can know the DotNet Exec options ahead of time which is convenient for us.
    set dotnet_exec_opts=--depsfile "%1%deps_json%" --runtimeconfig "%1%runtimeconfig_json%" 
)
rem Re-set this one every time.
set extensions=.exe%delim%.dll
:next_ext
rem Iterate the possible Extensions here.
if defined extensions (
    for /f "tokens=1* delims=%delim%" %%e in ("%extensions%") do (
        set extensions=%%f
        rem Then test for existence.
        if exist "%1%%e" (
            echo Invoking console tests for '%1%%e' targeting framework '%framework%' ...
            rem We can leverage the same DotNet Exec prefix bits here whether or not they are defined.
            echo %dotnet_exec_prefix%%dotnet_exec_opts%"%up_dir_prefix%%xunit_bin%" "%1%%e" %xunit_opts%
            %dotnet_exec_prefix%%dotnet_exec_opts%"%up_dir_prefix%%xunit_bin%" "%1%%e" %xunit_opts%
			rem Indeed yes the same ErrorLevel may be leveraged here regardless of the Runtime path.
            if errorlevel 0 (
                echo All tests passed.
            ) else if errorlevel 1 (
                echo One or more tests failed.
            ) else if errorlevel 3 (
                echo Error with one or more xUnit arguments: "%1%%e" %xunit_opts%
            ) else if errorlevel 4 (
                echo Problem loading one or more assemblies during test: "%1\bin\%config%%config_suffix%\%1%%e".
            ) else if errorlevel -1073741510 (
                echo User canceled tests.
            )
        ) else (
            echo Test artifact %1\bin\%config%%config_suffix%\%1%%e not found or has not yet been built.
        )
    )
    goto :next_ext
)
popd
exit /b

goto :end

:end

endlocal
