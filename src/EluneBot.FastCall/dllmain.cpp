// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

extern "C" {
	void __declspec(dllexport) __stdcall EnumerateVisibleObjects(unsigned int callback, int filter, unsigned int ptr)
	{
		typedef void __fastcall func(unsigned int callback, int filter);
		func* f = (func*)ptr;
		f(callback, filter);
	}
}

