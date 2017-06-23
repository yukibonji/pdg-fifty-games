namespace Sdl

open System
open System.Runtime.InteropServices

[<RequireQualifiedAccess>]
type CreateWindowFlags = 
    | None               = 0x00000000
    | FullScreen         = 0x00000001
    | OpenGL             = 0x00000002
    | Shown              = 0x00000004
    | Hidden             = 0x00000008
    | Borderless         = 0x00000010
    | Resizable          = 0x00000020
    | Minimized          = 0x00000040
    | Maximized          = 0x00000080
    | InputGrabbed       = 0x00000100
    | InputFocus         = 0x00000200
    | MouseFocus         = 0x00000400
    | FullScreenDesktop  = 0x00001001
    | Foreign            = 0x00000800
    | AllowHighDPI       = 0x00002000
    | MouseCapture       = 0x00004000

[<RequireQualifiedAccess>]
type CreateWindowPosition =
    | Undefined
    | Centered
    | Absolute of int * int

module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr SDL_CreateWindow(IntPtr title, int x, int y, int w, int h, uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyWindow(IntPtr window)
    

module Window =
    
    let create (title:string) (position:CreateWindowPosition) (width:int, height:int) (flags:CreateWindowFlags) (windowId:'TWindow) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        let windowX, windowY = 
            match position with
            | CreateWindowPosition.Undefined -> (0x1FFF0000,0x1FFF0000)
            | CreateWindowPosition.Centered -> (0x2FFF0000,0x2FFF0000)
            | CreateWindowPosition.Absolute (x,y) -> (x, y)
        title
        |> Utility.withUtf8String (fun p -> 
            {context with windows = context.windows |> Map.add windowId (Native.SDL_CreateWindow(p,windowX,windowY,width,height,flags |> uint32))})

    let destroy (windowId:'TWindow) (context:context<'TWindow,'TRenderer>) : context<'TWindow,'TRenderer> =
        if context.windows.ContainsKey windowId then
            Native.SDL_DestroyWindow(context.windows |> Map.find windowId)
            {context with windows = context.windows |> Map.remove windowId}
        else
            context

