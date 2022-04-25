// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "nativehost.h"
#include <cstdio>


bool injected = false;
HMODULE handle;

extern "C" __declspec(dllexport) void launch(int* param) {
    auto val = *param;
    if (injected) return;
    wchar_t path[MAX_PATH];
    if (GetModuleFileNameW(handle, path, MAX_PATH) == 0)
    {
        int ret = GetLastError();
        fprintf(stderr, "GetModuleFileName failed, error = %d\n", ret);
    }
    nativehost host;
    host.main(path);
    injected = true;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        handle = hModule;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}