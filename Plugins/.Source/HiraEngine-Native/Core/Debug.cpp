#include "Debug.h"

#define IMPLEMENT_LOGGER_OPERATOR(type, typeName) \
    void (CALLING_CONVENTION*GLog##typeName)(type) = nullptr; \
    DLLEXPORT(void) Init##typeName##Logger(void (CALLING_CONVENTION*InDelegate)(type)) \
    { \
        GLog##typeName = InDelegate; \
    } \
    Logger& operator<<(Logger& OutLogger, type Other) \
    { \
        GLog##typeName(Other); \
        return OutLogger; \
    }

// start log

void (CALLING_CONVENTION*GLogStart)(ELogType) = nullptr;

DLLEXPORT(void) InitLoggerLogStart(void (CALLING_CONVENTION*InDelegate)(ELogType))
{
    GLogStart = InDelegate;
}

Logger::Logger(const ELogType LogType)
{
    GLogStart(LogType);
}

// left shift operators

FOR_EACH_2_ARGUMENTS(IMPLEMENT_LOGGER_OPERATOR, const char*, String, const int32, Integer, const float, Float)

// end log

void (CALLING_CONVENTION*GLogEnd)() = nullptr;

DLLEXPORT(void) InitLoggerLogEnd(void (CALLING_CONVENTION*InDelegate)())
{
    GLogEnd = InDelegate;
}

void operator<<(Logger& OutLogger, const Logger* OtherLogger)
{
    GLogEnd();
}

const char* GetLogPrefix(const ELogType LogType)
{
    switch (LogType)
    {
        case ELogType::Error:
            return ">> [ERROR]     ";
        case ELogType::Assert:
            return ">> [ASSERTION] ";
        case ELogType::Warning:
            return ">> [WARNING]   ";
        case ELogType::Log:
            return ">> [MESSAGE]   ";
        case ELogType::Exception:
            return ">> [EXCEPTION] ";
        default:
            return ">> [UNKNOWN]   ";
    }
}

#undef IMPLEMENT_LOGGER_OPERATOR