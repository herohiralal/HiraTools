#pragma once

#include "HelperMacros.h"
#include "Platform/Platform.h"
#include "StringHandling/NativeStringBuilder.h"

struct SNativeString;

enum class ELogType : int32 { FOR_EACH(WRITE_ENUM_NAME, Error, Assert, Warning, Log, Exception) LogTypeMax };

class Debug
{
public:
    static void Log(ELogType Type, const SNativeString& ToLog);
};

#if !_CONSOLE   // print to unity logger

#define UNITY_LOG(type, msg) \
    Debug::Log(ELogType::type, NativeStringBuilder::Create(1000) << msg << \
    TEXT("\nFrom") << WFUNC << TEXT("() (at ") << WFILE << TEXT(": ") << __LINE__ << TEXT(")") << \
    NativeStringBuilder::Disposer());

#include "StackWalk/StackWalker.h"
#define UNITY_LOG_WITH_CALL_STACK(type, msg) \
    { \
        NativeStringBuilder* __LogBuilder = &(NativeStringBuilder::Create(1000) << msg); \
        SStackWalker::AppendCallStack(*__LogBuilder, 0); \
        Debug::Log(ELogType::type, *__LogBuilder << NativeStringBuilder::Disposer()); \
    }

#else   // print log to console 

#include<iostream>

const wchar* GetLogPrefix(ELogType LogType);

#define UNITY_LOG(type, msg) std::wcout << GetLogPrefix(ELogType::type) << msg << std::endl;

#define UNITY_LOG_WITH_CALL_STACK(type, msg) \
    { \
        NativeStringBuilder* __LogBuilder = &(NativeStringBuilder::Create(1000) << msg); \
        SStackWalker::AppendCallStack(*__LogBuilder, 0); \
        UNITY_LOG(type, (*__LogBuilder << NativeStringBuilder::Disposer()).GetRaw()) \
    }

#endif

#if UNITY_EDITOR || _CONSOLE   // get all log for console
#define UNITY_EDITOR_LOG(type, msg) UNITY_LOG(type, msg)
#define UNITY_EDITOR_LOG_WITH_CALL_STACK(msg) UNITY_LOG_WITH_CALL_STACK(msg)
#else
#define UNITY_EDITOR_LOG(type, msg)
#define UNITY_EDITOR_LOG_WITH_CALL_STACK(msg)
#endif