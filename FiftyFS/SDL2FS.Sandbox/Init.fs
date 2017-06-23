namespace Sdl

open System
open System.Runtime.InteropServices

[<Flags>]
type InitFlags =
    | None           = 0x00000000
    | Timer          = 0x00000001
    | Audio          = 0x00000010
    | Video          = 0x00000020
    | Joystick       = 0x00000200
    | Haptic         = 0x00001000
    | GameController = 0x00002000
    | Events         = 0x00004000
    | Everything     = 0x0000FFFF


module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Init")>]
    extern int SdlInit(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Quit")>]
    extern void SdlQuit()
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyRenderer(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_InitSubSystem")>]
    extern int SdlInitSubSystem(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_QuitSubSystem")>]
    extern void SdlQuitSubSystem(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_WasInit")>]
    extern uint32 SdlWasInit(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr SDL_GetError()


module Init =

    let private destroyMainRenderer (context:context) : context =
        if context.mainRenderer <> IntPtr.Zero then
            Native.SDL_DestroyRenderer(context.mainRenderer)
            {context with mainRenderer = IntPtr.Zero}
        else
            context

    let run (initFlags: InitFlags) (runFunc: context->context) : string option =
        match Native.SdlInit (initFlags |> uint32) with
        | 0 ->
            Context.create ()
            |> runFunc
            |> destroyMainRenderer
            |> Window.destroyMainWindow
            |> ignore

            Native.SdlQuit()

            None
        | _ ->
            Native.SDL_GetError()
            |> Marshal.PtrToStringAnsi
            |> Some

    let add (initFlags:InitFlags) : string option =
        match Native.SdlInitSubSystem(initFlags |> uint32) with
        | 0 -> None
        | _ -> Native.SDL_GetError() |> Marshal.PtrToStringAnsi |> Some

    let remove (initFlags:InitFlags) : unit =
        Native.SdlQuitSubSystem(initFlags |> uint32)

    let contains (initFlags:InitFlags) : InitFlags = 
        Native.SdlWasInit(initFlags |> uint32) |> int32 |> enum<InitFlags>
