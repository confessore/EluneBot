﻿namespace elunebot.statics
{
    static class SendOvers
    {
        public static string[] WardenLoadDetour { get; } =
        {
            "MOV [0xCE8978], EAX",
            "pushfd",
            "pushad",
            "push EAX",
            "call [|addr|]",
            "popad",
            "popfd",
            "jmp 0x006CA233"
        };

        public static string[] WardenMemCpyDetour { get; } =
        {
            "PUSH ESI",
            "PUSH EDI",
            "CLD",
            "MOV EDX, [ESP+20]",
            "MOV ESI, [ESP+16]",
            "MOV EAX, [ESP+12]",
            "MOV ECX, EDX",
            "MOV EDI, EAX",
            "pushfd",
            "pushad",
            "PUSH EDI",
            "PUSH ECX",
            "PUSH ESI",
            "call [|addr|]",
            "popad",
            "popfd",
            "POP EDI",
            "POP ESI",
            "jmp [|addr|]"
        };

        public static string[] WardenPageScanDetour { get; } =
        {
            "mov eax, [ebp+8]", // read base
            "pushfd",
            "pushad",
            "mov ecx, esi",
            "add ecx, edi",
            "add ecx, 0x1C",
            "push ecx",
            "push edi",
            "push eax",
            "call [|addr|]",
            "popad",
            "popfd",
            "inc edi",
            "jmp [|addr|]"
        };

        public static string[] EventSignal0 { get; } =
        {
            "PUSH ESI",
            "CALL 0x007040D0",
            "pushfd",
            "pushad",
            "mov EDI, [EDI]",
            "push EDI",
            "call [|addr|]",
            "popad",
            "popfd",
            "jmp [|addr|]"
        };

        public static string[] EventSignal { get; } =
        {
            "PUSH EBX",
            "PUSH ESI",
            "CALL 0x007040D0",
            "pushfd",
            "pushad",
            "mov EAX, EBP",
            "ADD EAX, 0x10",
            "push eax",
            "mov EAX, [EBP + 0xC]",
            "push EAX",
            "mov EDI, [EDI]",
            "push EDI",
            "call [|addr|]",
            "popad",
            "popfd",
            "jmp [|addr|]"
        };
    }
}
