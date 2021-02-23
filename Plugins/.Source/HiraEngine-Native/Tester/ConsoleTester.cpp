#ifdef _CONSOLE
#include "Debug.h"
#include <iostream>

#define PRINT( x ) std::cout << x << std::endl;

void __stdcall LogToConsole(const ELogType LogType, const char* ToLog)
{
    switch (LogType)
    {
        case ELogType::Error:
            std::cout << ">> [ERROR]     ";
            break;
        case ELogType::Assert:
            std::cout << ">> [ASSERTION] ";
            break;
        case ELogType::Warning:
            std::cout << ">> [WARNING]   ";
            break;
        case ELogType::Log:
            std::cout << ">> [MESSAGE]   ";
            break;
        case ELogType::Exception:
            std::cout << ">> [EXCEPTION] ";
            break;
        default: ;
    }
    PRINT(ToLog)
}

int main(int Argc, char* Argv[])
{
    // attach console output
    Debug::__InternalLogToUnity = LogToConsole;
}

#endif
