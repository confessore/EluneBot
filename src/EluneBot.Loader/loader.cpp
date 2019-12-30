#pragma once

#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include <corerror.h>
#include <metahost.h>
#include <process.h>
#include <string>

#pragma comment(lib, "mscoree.lib")

#define LOAD_DLL_FILE_NAME L"EluneBot.exe"
#define NAMESPACE_AND_CLASS L"EluneBot.Program"
#define MAIN_METHOD L"Entry"
#define MAIN_METHOD_ARGS L"NONE"

HMODULE g_myDllModule = NULL;
HANDLE g_hThread = NULL;
wchar_t* dllLocation = NULL;
ICLRMetaHost* metaHost = NULL;
ICLRRuntimeInfo* runtimeInfo = NULL;
ICLRRuntimeHost* runtimeHost = NULL;

unsigned _stdcall ThreadMain(void* pParam) {
	if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)& metaHost) == S_OK) {
		if (metaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&runtimeInfo) == S_OK) {
			if (runtimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)& runtimeHost) == S_OK) {
				if (runtimeHost->Start() == S_OK) {
					DWORD pReturnValue;
					runtimeHost->ExecuteInDefaultAppDomain(dllLocation, NAMESPACE_AND_CLASS, MAIN_METHOD, MAIN_METHOD_ARGS, &pReturnValue);
					metaHost->Release();
					runtimeInfo->Release();
					runtimeHost->Release();
				}
			}
		}
	}
	return 0;
}

void LoadCLR()
{
	wchar_t buffer[255];
	if (!GetModuleFileNameW(g_myDllModule, buffer, 255)) return;
	std::wstring modulePath(buffer);
	modulePath = modulePath.substr(0, modulePath.find_last_of('\\') + 1);
	modulePath = modulePath.append(LOAD_DLL_FILE_NAME);
	dllLocation = new wchar_t[modulePath.length() + 1];
	wcscpy(dllLocation, modulePath.c_str());
	dllLocation[modulePath.length()] = '\0';
	g_hThread = (HANDLE)_beginthreadex(NULL, 0, ThreadMain, NULL, 0, NULL);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved) {
	g_myDllModule = hModule;
	switch (dwReason) {
	case DLL_PROCESS_ATTACH:
		LoadCLR();
		break;

	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
