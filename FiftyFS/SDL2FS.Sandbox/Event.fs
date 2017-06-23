namespace Sdl

#nowarn "9"

open System.Runtime.InteropServices

[<StructLayout(LayoutKind.Explicit, Size=56)>]
type internal SDL_Event =
    struct
        [<FieldOffset(0)>]
        val Type: uint32
    end

