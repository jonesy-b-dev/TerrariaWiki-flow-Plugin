@echo off

taskkill /im Flow.Launcher.exe

set "sourceFolder=D:\Programming\TerrariaWiki-flow-Plugin\TerrariaWiki\Flow.Launcher.Plugin.TerrariaWiki\bin\Debug"
set "destinationFolder=C:\Users\xvanhunen\AppData\Roaming\FlowLauncher\Plugins\TerrariaWiki"

echo Removing plugin folder...

rd /s /q "%destinationFolder%"

echo Copying files from %sourceFolder% to %destinationFolder%...

xcopy /E /I /Y "%sourceFolder%" "%destinationFolder%"

echo Copy completed.

echo Starting Flow Launcher...
start "" "C:\Users\xvanhunen\AppData\Local\FlowLauncher\app-1.16.2\Flow.Launcher.exe"

pause
