// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
using namespace std;

#include <stdio.h>

extern "C" {
	void __declspec(dllexport) __stdcall EnumerateVisibleObjects(unsigned int callback, int filter, unsigned int ptr)
	{
		typedef void __fastcall func(unsigned int callback, int filter);
		func* f = (func*)ptr;
		f(callback, filter);
	}

	void __declspec(dllexport) __stdcall DoString(char* code, unsigned int parPtr)
	{
		typedef void __fastcall func(char* code, const char* zero);
		func* f = (func*)parPtr;
		f(code, "unused");
		return;
	}
}

