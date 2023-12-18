set SOLUTION_NAME="AirFlow"
set APPLICATION_PLUGINS_PATH="%programdata%\Autodesk\ApplicationPlugins"
set BUILD_PLUGIN_PATH="build\%SOLUTION_NAME%.bundle\Contents\Program"
set STARTUP_PROJECT_NAME="BIMLogiq.%SOLUTION_NAME%.Startup"
set PACKAGE="%STARTUP_PROJECT_NAME%\Manifest\PackageContents.xml"
set MANIFEST="%STARTUP_PROJECT_NAME%\Manifest\%SOLUTION_NAME%.addin"

rd /s /q "build"
md %BUILD_PLUGIN_PATH%
md %RESOURCES%

xcopy /y /r %PACKAGE% "build\%SOLUTION_NAME%.bundle"
xcopy /y /r %MANIFEST% "build\%SOLUTION_NAME%.bundle\Contents"

xcopy /y /r /c "BIMLogiq.%SOLUTION_NAME%.Startup\bin\X64\Debug\net48\*.dll" %BUILD_PLUGIN_PATH%

xcopy /y /r /s /h /c "build" %APPLICATION_PLUGINS_PATH%