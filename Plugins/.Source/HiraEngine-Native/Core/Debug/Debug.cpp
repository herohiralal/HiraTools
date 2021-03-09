#include "Debug.h"

// start log

void (CALLING_CONVENTION*GLogStart)(ELogType) = nullptr;

DLLEXPORT(void) InitLoggerLogStart(void (CALLING_CONVENTION*InDelegate)(ELogType))
{
    GLogStart = InDelegate;
}

Debug::Logger::Logger(const ELogType LogType)
{
    GLogStart(LogType);
}

// left shift operators

#define IMPLEMENT_LOGGER_OPERATOR(type, typeName) \
    void (CALLING_CONVENTION*GLog##typeName)(type) = nullptr; \
    DLLEXPORT(void) Init##typeName##Logger(void (CALLING_CONVENTION*InDelegate)(type)) \
    { \
        GLog##typeName = InDelegate; \
    } \
    Debug::Logger operator<<(const Debug::Logger OutLogger, type Other) \
    { \
        GLog##typeName(Other); \
        return OutLogger; \
    }

FOR_EACH_2_ARGUMENTS(IMPLEMENT_LOGGER_OPERATOR,
                     const wchar*, WideString,
                     const bool8, Boolean,
                     const int8, SignedByte,
                     const uint8, Byte,
                     const int16, Short,
                     const uint16, UnsignedShort,
                     const int32, Integer,
                     const uint32, UnsignedInteger,
                     const int64, Long,
                     const uint64, UnsignedLong,
                     const float, Float,
                     const double, Double)

#undef IMPLEMENT_LOGGER_OPERATOR

// end log

void (CALLING_CONVENTION*GLogEnd)() = nullptr;

DLLEXPORT(void) InitLoggerLogEnd(void (CALLING_CONVENTION*InDelegate)())
{
    GLogEnd = InDelegate;
}

void operator<<(const Debug::Logger OutLogger, const Debug::Logger* OtherLogger)
{
    GLogEnd();
}

#if _CONSOLE

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

#endif