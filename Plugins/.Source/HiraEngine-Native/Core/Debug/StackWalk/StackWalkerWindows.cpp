#include "StackWalkerWindows.h"
#include "Debug/Debug.h"
#include "StringHandling/NativeStringBuilder.h"

#if UNITY_EDITOR_WIN || (UNITY_STANDALONE_WIN && DEVELOPMENT_BUILD)

#pragma comment(lib, "DbgHelp.lib")
#include <Windows.h>
#include <DbgHelp.h>

#define ERROR_CODE SNativeString::FromInteger(GetLastError()) << TEXT(". StackWalker might not work.")

static void* GCurrentProcess = nullptr;
static bool8 GSymInitializeSuccessful = false;
static DWORD64 GModuleBase = 0;

bool8 GetFilePath(wchar* Buffer, const uint32 BufferSize);
bool8 GetFileSize(const wchar* File, uint32& Size);

void SStackWalkerWindows::Initialize()
{
    if (AttemptInit())
    {
        Instance = new SStackWalkerWindows();
    }
    else
    {
        Super::Initialize();
    }
}

bool8 SStackWalkerWindows::AttemptInit()
{
    wchar FilePath[MAX_PATH];
    uint32 FileSize;
    if (!GetFilePath(FilePath, MAX_PATH)) return false;
    if (!GetFileSize(FilePath, FileSize)) return false;

    GCurrentProcess = GetCurrentProcess();
    GSymInitializeSuccessful = SymInitializeW(GCurrentProcess, nullptr, true);

    if (!GSymInitializeSuccessful)
    {
        UNITY_LOG(Warning, TEXT("SymInitialize failed with error code: ") << ERROR_CODE)
        return false;
    }

    GModuleBase = SymLoadModuleExW(
        GCurrentProcess,
        nullptr,
        FilePath,
        nullptr,
        0x10000000,
        FileSize,
        nullptr,
        0);

    if (!GModuleBase)
    {
        UNITY_LOG(Warning, TEXT("SymLoadModule failed with error code: ") << ERROR_CODE)
        return false;
    }

    return true;
}

void SStackWalkerWindows::ShutdownImplementation()
{
    if (GModuleBase)
    {
#if _WIN64
        if(!SymUnloadModule64(GCurrentProcess, GModuleBase))
#else
        if (!SymUnloadModule(GCurrentProcess, GModuleBase))
#endif
        {
            UNITY_LOG(Warning, TEXT("SymUnloadModule failed with error code: ") << ERROR_CODE)
        }
    }

    if (GSymInitializeSuccessful)
    {
        if (!SymCleanup(GCurrentProcess))
        {
            UNITY_LOG(Warning, TEXT("SymCleanup failed with error code: ") << ERROR_CODE)
        }
    }
}

void GetSymbolData(SYMBOL_INFO_PACKAGEW& Sip, IMAGEHLP_LINEW64& LineInfo, const uintptr Address)
{
    DWORD64 Displacement = 0;
    if (!SymFromAddrW(GCurrentProcess, Address, &Displacement, &Sip.si))
    {
        SNativeString::StringCopyUnsafe(Sip.si.Name, TEXT("__UNRECOGNIZED_FUNCTION__"));
    }

    DWORD LineDisplacement = 0;
    if (!SymGetLineFromAddrW64(GCurrentProcess, Address, &LineDisplacement, &LineInfo))
    {
        LineInfo.LineNumber = -1;
        SNativeString::StringCopyUnsafe(LineInfo.FileName, TEXT("__UNRECOGNIZED_FILE__"));
    }
}

void SStackWalkerWindows::AppendCallStackImplementation(NativeStringBuilder& OutBuilder, const uint8 FramesToSkip)
{
    void* StackTraceBuffer[63];
    const int32 Count =
        CaptureStackBackTrace(FramesToSkip + 1, 63 - (FramesToSkip + 1), StackTraceBuffer, nullptr);

    SYMBOL_INFO_PACKAGEW Sip;
    Sip.si.SizeOfStruct = sizeof(SYMBOL_INFOW);
    Sip.si.MaxNameLen = sizeof(Sip.name);

    IMAGEHLP_LINEW64 LineInfo;
    LineInfo.SizeOfStruct = sizeof(IMAGEHLP_LINEW64);

    for (int32 It = 0; It < Count; ++It)
    {
        GetSymbolData(Sip, LineInfo, reinterpret_cast<uintptr>(StackTraceBuffer[It]));
        OutBuilder <<
            TEXT("\nFrom ") <<
            Sip.si.Name <<
            TEXT("() (at ") <<
            LineInfo.FileName <<
            TEXT(": ") <<
            static_cast<uint16>(LineInfo.LineNumber) <<
            TEXT(")");
    }
}

bool8 GetFilePath(wchar* Buffer, const uint32 BufferSize)
{
    HMODULE Module;

    if (!GetModuleHandleExW(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
                            reinterpret_cast<wchar*>(&GetFilePath), &Module))
    {
        UNITY_LOG(Warning, TEXT("GetModuleHandle failed with error code: ") << ERROR_CODE)
        return false;
    }

    if (GetModuleFileNameW(Module, Buffer, BufferSize) == 0)
    {
        UNITY_LOG(Warning, TEXT("GetModuleFileName failed with error code: ") << ERROR_CODE)
        return false;
    }

    return true;
}

bool8 GetFileSize(const wchar* File, uint32& Size)
{
    if (File == nullptr) return false;

    const HANDLE FileHandle = CreateFileW(
        File,
        GENERIC_READ,
        FILE_SHARE_READ,
        nullptr,
        OPEN_EXISTING,
        0,
        nullptr);

    if (File == INVALID_HANDLE_VALUE)
    {
        UNITY_LOG(Warning, TEXT("CreateFile failed with error code: ") << ERROR_CODE)
        return false;
    }

    Size = GetFileSize(FileHandle, nullptr);
    if (Size == INVALID_FILE_SIZE)
    {
        UNITY_LOG(Warning, TEXT("GetFileSize failed with error code: ") << ERROR_CODE)
    }

    if (!CloseHandle(FileHandle))
    {
        UNITY_LOG(Warning, TEXT("CloseHandle failed."))
    }

    return Size != INVALID_FILE_SIZE;
}

#endif
