#pragma once

#include "HelperMacros.h"
#include "Platform/Platform.h"

struct SNativeString;

enum class ELogType : int32 { FOR_EACH(WRITE_ENUM_NAME, Error, Assert, Warning, Log, Exception) LogTypeMax };

class Debug
{
public:
    struct Logger
    {
        // start logging
        explicit Logger(ELogType LogType = ELogType::Log);
    };
};

Debug::Logger operator<<(Debug::Logger OutLogger, const SNativeString& Other);

// main operators

#define DECLARE_LOGGER_OPERATOR(x) Debug::Logger operator<<(Debug::Logger OutLogger, x Other);

FOR_EACH(DECLARE_LOGGER_OPERATOR,
         const wchar*,
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

void operator<<(Debug::Logger OutLogger, const Debug::Logger* OtherLogger);

#if !_CONSOLE   // print to unity logger

#define UNITY_LOG(type, msg) \
    Debug::Logger(ELogType::type) << \
    msg << L"\nFrom " << WFUNC << L"() (at " << WFILE << L": " << __LINE__ << L")" \
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
