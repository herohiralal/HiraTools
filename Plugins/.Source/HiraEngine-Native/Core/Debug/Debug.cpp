#include "Debug.h"
#include "Interop/ManagedAPIMacros.h"
#include "StringHandling/NativeString.h"

IMPORT_FUNCTION_WITH_SIGNATURE(void, LogInternal, ELogType, LogType, const wchar*, ToLog)

void Debug::Log(const ELogType Type, const SNativeString& ToLog)
{
    LogInternal(Type, ToLog.GetRaw());
}

#if _CONSOLE

const wchar* GetLogPrefix(const ELogType LogType)
{
    switch (LogType)
    {
        case ELogType::Error:
            return TEXT(">> [ERROR]     ");
        case ELogType::Assert:
            return TEXT(">> [ASSERTION] ");
        case ELogType::Warning:
            return TEXT(">> [WARNING]   ");
        case ELogType::Log:
            return TEXT(">> [MESSAGE]   ");
        case ELogType::Exception:
            return TEXT(">> [EXCEPTION] ");
        default:
            return TEXT(">> [UNKNOWN]   ");
    }
}

#endif
