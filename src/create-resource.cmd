@ECHO OFF
rm -R build
mkdir build\Server
mkdir build\Client



copy /B "Server\bin\Release\netstandard2.0\*.dll" "build\Server\"
copy /B "Client\bin\Release\net452\*.dll" "build\Client\"


copy /B "Lua\*.lua" "build\"
copy /B "Lua\Server\*.lua" "build\Server\"
copy /B "Lua\Client\*.lua" "build\Client\"