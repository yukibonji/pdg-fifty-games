namespace Sdl

open System
open System.Runtime.InteropServices

type event =
    | Other of uint32

type context<'TWindow,'TRenderer when 'TRenderer:comparison and 'TWindow:comparison> =
    {windows:Map<'TWindow,IntPtr>;
    renderers:Map<'TRenderer,IntPtr>;
    lastError:string option;
    lastEvent:event option}

module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_WaitEvent(SDL_Event* event)

module Event = 

    let internal ofSDL_Event (other:SDL_Event) : event =
        match other.Type with
        | x -> x |> Other

    let wait (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        let mutable event = Sdl.SDL_Event()
        match Native.SDL_WaitEvent(&&event) with
        | _ -> context

module Context =
    
    let create () : context<'TWindow,'TRenderer> = 
        {windows = Map.empty;
        renderers = Map.empty;
        lastError = None;
        lastEvent = None}



