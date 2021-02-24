#pragma once

#include "SyntacticMacros.h"
#include "Platform/Platform.h"

DECLARE_ENUM(int32, LogType, Error, Assert, Warning, Log, Exception)

struct Logger
{
    // start logging
    explicit Logger(ELogType LogType);
};

// main operators

#define DECLARE_LOGGER_OPERATOR(x) Logger& operator<<(Logger& OutLogger, x Other);

FOR_EACH(DECLARE_LOGGER_OPERATOR, const char*, int32, float)

#undef DECLARE_LOGGER_OPERATOR

// end logging

void operator<<(Logger& OutLogger, const Logger* OtherLogger);

#if !_CONSOLE   // print to unity logger

#define UNITY_LOG(type, msg) \
    { \
        Logger NewLogger = Logger(ELogType::type); \
        NewLogger << msg << static_cast<const Logger*>(nullptr); \
    }

#else   // print log to console 

#include<iostream>
const char* GetLogPrefix(ELogType LogType);
#define UNITY_LOG(type, msg) std::cout << GetLogPrefix(ELogType::type) << msg << std::endl;

#endif

#if _EDITOR || _CONSOLE   // get all log for console
#define UNITY_EDITOR_LOG(type, msg) UNITY_LOG(type, msg)
#else
#define UNITY_EDITOR_LOG(type, msg)
#endif