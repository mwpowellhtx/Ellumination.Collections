@echo off

setlocal

echo Cleaning is now a normal part of the build process.
goto :end

:setconfig
set config=%1
if ("%config%" === "") (
    set config=Release
)

:: Do the main areas here.
pushd ..\..

call :cleanpkgs Ellumination.Collections.ImmutableBitArray
call :cleanpkgs Ellumination.Collections.Enumerations
call :cleanpkgs Ellumination.Collections.Enumerations.Tests
call :cleanpkgs Ellumination.Collections.Stacks
call :cleanpkgs Ellumination.Collections.Queues
call :cleanpkgs Ellumination.Collections.Deques

popd

goto :end

:cleanpkgs
set f=%1
for %%f in ("%f%\bin\%config%\%f%.*.nupkg") do (
    del /q "%%f"
    echo "%%f" cleaned...
)
exit /b

:end

endlocal

pause
