namespace Sdl

#nowarn "9"

open System.Runtime.InteropServices
open System

module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_ClearError()
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr SDL_GetError()
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
    extern int SDL_SetError(IntPtr fmt)

module Error =

    let set (errorString:string) =
        errorString
        |> Utility.withUtf8String (fun ptr -> Native.SDL_SetError(ptr) |> ignore)

    let get () =
        Native.SDL_GetError()
        |> Utility.intPtrToStringUtf8

    let clear () =
        Native.SDL_ClearError()
