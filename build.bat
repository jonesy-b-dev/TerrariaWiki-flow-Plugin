@echo off

taskkill /im Flow.Launcher.exe

set "sourceFolder=D:\Code Projects\C#\TerrariaWiki-flow-Plugin\TerrariaWiki\Flow.Launcher.Plugin.TerrariaWiki\bin\Debug"
set "destinationFolder=C:\Users\jonas\AppData\Local\FlowLauncher\app-1.16.2\UserData\Plugins\TerrariaWiki"

echo Copying files from %sourceFolder% to %destinationFolder%...

xcopy /E /I /Y "%sourceFolder%" "%destinationFolder%"

echo Copy completed.

echo Starting Flow Launcher...
start "" "C:\Users\jonas\AppData\Local\FlowLauncher\app-1.16.2\Flow.Launcher.exe"

pause
