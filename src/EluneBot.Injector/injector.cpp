#pragma once

#include <windows.h>
#include <iostream>

extern "C" {
	_declspec(dllexport) bool Inject(int pid, char* dll) {
		const char* lpCaption = "EluneBot";
		HANDLE process = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
		if (!process) {
			MessageBox(0, "no such process exists", lpCaption, 0 | MB_OK);
			return false;
		}
		LPVOID addr = (LPVOID)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
		if (!addr) {
			MessageBox(0, "load library was not found", lpCaption, 0 | MB_OK);
			return false;
		}
		LPVOID arg = (LPVOID)VirtualAllocEx(process, NULL, strlen(dll), MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
		if (!arg) {
			MessageBox(0, "could not allocate process memory", lpCaption, 0 | MB_OK);
			return false;
		}
		bool write = WriteProcessMemory(process, arg, dll, strlen(dll), NULL);
		if (!write) {
			MessageBox(0, "nothing was written to process memory", lpCaption, 0 | MB_OK);
			return false;
		}
		HANDLE thread = CreateRemoteThread(process, NULL, 0, (LPTHREAD_START_ROUTINE)addr, arg, NULL, NULL);
		if (!thread) {
			MessageBox(0, "remote thread could not be created", lpCaption, 0 | MB_OK);
			return false;
		}
		CloseHandle(process);
		return true;
	}
}