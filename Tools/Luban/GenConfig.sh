#!/bin/bash

WORKSPACE=../..

LUBAN_DLL=$WORKSPACE/Tools/Luban/LubanRelease/Luban.dll
CONF_ROOT=$WORKSPACE/Unity/Assets/Config/Excel

# Client
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t client \
    -c cs-bin \
    -d bin \
    -d json \
    --conf $CONF_ROOT/__luban__.conf \
    -x outputCodeDir=$WORKSPACE/Unity/Assets/Scripts/Model/Generate/Client/Config \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/c \
    -x json.outputDataDir=$WORKSPACE/Config/Json/c \
    -x lineEnding=CRLF 

echo ==================== FuncConfig : GenClientFinish ====================

if [ $? -ne 0 ]; then
    echo "An error occurred, press any key to exit."
    read -n 1 -s
    exit 1
fi

# Server
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -c cs-bin \
    -d bin \
    -d json \
    --conf $CONF_ROOT/__luban__.conf \
    -x outputCodeDir=$WORKSPACE/Unity/Assets/Scripts/Model/Generate/Server/Config \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s \
    -x lineEnding=CRLF 

echo ==================== FuncConfig : GenServerFinish ====================

if [ $? -ne 0 ]; then
    echo "An error occurred, press any key to exit."
    read -n 1 -s
    exit 1
fi

# StartConfig Release
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -c cs-bin \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Release/__luban__.conf \
    -x outputCodeDir=$WORKSPACE/Unity/Assets/Scripts/Model/Generate/Server/Config/StartConfig \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Release \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Release \
    -x lineEnding=CRLF 

echo ==================== StartConfig : GenReleaseFinish ====================

if [ $? -ne 0 ]; then
    echo "An error occurred, press any key to exit."
    read -n 1 -s
    exit 1
fi

# StartConfig Benchmark
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Benchmark/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Benchmark \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Benchmark 

echo ==================== StartConfig : GenBenchmarkFinish ====================

if [ $? -ne 0 ]; then
    echo "An error occurred, press any key to exit."
    read -n 1 -s
    exit 1
fi

# StartConfig Localhost
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Localhost/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Localhost \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Localhost 

echo ==================== StartConfig : GenLocalhostFinish ====================

if [ $? -ne 0 ]; then
    echo "An error occurred, press any key to exit."
    read -n 1 -s
    exit 1
fi

# StartConfig RouterTest
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/RouterTest/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/RouterTest \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/RouterTest 

echo ==================== StartConfig : GenRouterTestFinish ====================

read -n 1 -p "Press any key to continue..."