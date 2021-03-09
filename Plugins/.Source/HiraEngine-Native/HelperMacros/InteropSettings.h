#pragma once

#if !defined(CALLING_CONVENTION)
#   define CALLING_CONVENTION __stdcall
#else
#   error Calling convention macro has been defined elsewhere.
#endif

#if !defined(DLLEXPORT)

#   if _WIN32
#       define DLLEXPORT(type) extern "C" __declspec(dllexport) type CALLING_CONVENTION
#   else
#       define DLLEXPORT(type) extern "C" type CALLING_CONVENTION
#   endif

#else

#   error DLLExport macro has been defined elsewhere.

#endif