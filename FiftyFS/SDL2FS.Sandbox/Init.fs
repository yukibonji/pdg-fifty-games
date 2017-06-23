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
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_InitSubSystem")>]
    extern int SdlInitSubSystem(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_QuitSubSystem")>]
    extern void SdlQuitSubSystem(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_WasInit")>]
    extern uint32 SdlWasInit(uint32 flags)


module Init =

    let private destroyWindows (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        (context,context.windows)
        ||> Map.fold (fun ctx id _ -> Window.destroy id ctx)

    let private destroyRenderers (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        (context,context.renderers)
        ||> Map.fold (fun ctx id _ -> Renderer.destroy id ctx)

    let run (initFlags: InitFlags) (runFunc: context<'TWindow,'TRenderer>->context<'TWindow,'TRenderer>) : string option =
        match Native.SdlInit (initFlags |> uint32) with
        | 0 ->
            let context = 
                Context.create ()
                |> runFunc
                |> destroyRenderers
                |> destroyWindows

            Native.SdlQuit()

            context.lastError
        | _ ->
            Error.get () |> Some

    let add (initFlags:InitFlags) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        match Native.SdlInitSubSystem(initFlags |> uint32) with
        | 0 -> context
        | _ -> {context with lastError = Error.get () |> Some}

    let remove (initFlags:InitFlags) (context:context<'TWindow,'TRenderer>) :  context<'TWindow,'TRenderer> =
        Native.SdlQuitSubSystem(initFlags |> uint32)
        context

    let contains (initFlags:InitFlags) : InitFlags = 
        Native.SdlWasInit(initFlags |> uint32) |> int32 |> enum<InitFlags>
