#pragma once

#include "SyntacticMacros.h"
#include "Platform/Platform.h"

DECLARE_ENUM(int32, LogType, Error, Assert, Warning, Log, Exception)

class Debug
{
public:
    struct Logger
    {
        // start logging
        explicit Logger(ELogType LogType = ELogType::Log);
    };
};

// main operators

#define DECLARE_LOGGER_OPERATOR(x) Debug::Logger operator<<(Debug::Logger OutLogger, x Other);

FOR_EACH(DECLARE_LOGGER_OPERATOR,
         const char*,
         bool8,
         int8,
         uint8,
         int16,
         uint16,
         int32,
         uint32,
         int64,
         uint64,
         float,
         double)

#undef DECLARE_LOGGER_OPERATOR

// end logging

void operator<<(const Debug::Logger OutLogger, const Debug::Logger* OtherLogger);

#if !_CONSOLE   // print to unity logger

#define UNITY_LOG(type, msg) \
    Debug::Logger(ELogType::type) << \
    msg << "\nFrom " << __FUNCTION__ << "() (at " << __FILE__ << ": " << __LINE__ << ")" \
    << static_cast<const Debug::Logger*>(nullptr);

#else   // print log to console 

#include<iostream>
const char* GetLogPrefix(ELogType LogType);
#define UNITY_LOG(type, msg) std::cout << GetLogPrefix(ELogType::type) << msg << std::endl;

#endif

#if UNITY_EDITOR || _CONSOLE   // get all log for console
#define UNITY_EDITOR_LOG(type, msg) UNITY_LOG(type, msg)
#else
#define UNITY_EDITOR_LOG(type, msg)
#endif