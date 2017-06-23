namespace Sdl

open System.Runtime.InteropServices
open System

[<Flags>]
type CreateRendererFlags = 
    | None          = 0x00000000
    | Software      = 0x00000001
    | Accelerated   = 0x00000002
    | PresentVSync  = 0x00000004
    | TargetTexture = 0x00000008


module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyRenderer(IntPtr renderer)

    extern int SDL_RenderClear(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_RenderPresent(IntPtr renderer)

    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2)

    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_SetRenderDrawColor(IntPtr renderer, uint8 r, uint8 g, uint8 b, uint8 a)

module Renderer =

    let create (driverIndex:int option) (flags:CreateRendererFlags) (windowId:'TWindow) (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        let windowPtr = context.windows.[windowId] //TODO: window not found?
        let rendererPtr = Native.SDL_CreateRenderer(windowPtr, (if driverIndex.IsNone then -1 else driverIndex.Value), flags |> uint32)//TODO: returns null?
        {context with renderers = context.renderers |> Map.add rendererId rendererPtr}//TODO: what if renderer at rendererId already exists?

    let destroy (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        if context.renderers.ContainsKey rendererId then
            Native.SDL_DestroyWindow(context.renderers |> Map.find rendererId)
            {context with renderers = context.renderers |> Map.remove rendererId}
        else
            context

    let clear (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        match Native.SDL_RenderClear(context.renderers.[rendererId]) with
        | _ -> context

    let present (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        Native.SDL_RenderPresent(context.renderers.[rendererId])
        context

    let setDrawColor (color:color) (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        match Native.SDL_SetRenderDrawColor(context.renderers.[rendererId],color |> Color.getRed,color |> Color.getGreen,color |> Color.getBlue,color |> Color.getAlpha) with
        | _ -> context

    let drawLine (first:int * int) (last: int * int) (rendererId:'TRenderer) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        match Native.SDL_RenderDrawLine(context.renderers.[rendererId], first |> fst, first |> snd, last |> fst, last |> snd) with
        | _ -> context