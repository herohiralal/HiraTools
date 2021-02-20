#pragma once

#include "ImporterMacros.h"

enum ELogType
{
    Error,
    Assert,
    Warning,
    Log,
    Exception,
};

class Debug
{
    DECLARE_IMPORTED_LIBRARY_FUNCTION(void, LogToUnity, ELogType, LogType, const char*, ToLog)
};

#define UNITY_LOG(type, msg) if (Debug::LogToUnityIsValid()) Debug::LogToUnity(ELogType::type, msg);

#if _EDITOR
#define UNITY_EDITOR_LOG(type, msg) UNITY_LOG(type, msg)
#else
#define UNITY_EDITOR_LOG(type, msg)
#endif