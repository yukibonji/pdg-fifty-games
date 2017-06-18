module SDLContext.SDLContext
#nowarn "9"

open System.Runtime.InteropServices
open System

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_Rect = 
    struct
        val mutable x :int
        val mutable y :int
        val mutable w :int
        val mutable h :int
    end


[<Flags>]
type private Init =
    | None           = 0x00000000
    | Timer          = 0x00000001
    | Audio          = 0x00000010
    | Video          = 0x00000020
    | Joystick       = 0x00000200
    | Haptic         = 0x00001000
    | GameController = 0x00002000
    | Events         = 0x00004000
    | Everything     = 0x0000FFFF

[<RequireQualifiedAccess>]
[<Flags>]
type WindowFlags = 
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

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_QuitEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_Keysym = 
    struct
        val Scancode: int32
        val Sym: int32
        val Mod: uint16
        val Unused: uint32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_CommonEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_WindowEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
        val WindowID: uint32
        val Event: uint8     
        val Padding1: uint8
        val Padding2: uint8
        val Padding3: uint8
        val Data1: int32  
        val Data2: int32   
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_KeyboardEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
        val WindowID: uint32
        val State: uint8
        val Repeat: uint8
        val Padding2: uint8
        val Padding3: uint8
        val Keysym: SDL_Keysym
    end

[<StructLayout(LayoutKind.Explicit, Size=52)>]
type internal SDL_TextEditingEvent =
    struct
        [<FieldOffset(0)>]
        val Type: uint32
        [<FieldOffset(4)>]
        val Timestamp: uint32
        [<FieldOffset(8)>]
        val WindowID: uint32                            
        [<FieldOffset(12)>]
        val Text: byte//really a byte[32]
        [<FieldOffset(44)>]
        val Start: int32                               
        [<FieldOffset(48)>]
        val Length: int32                              
    end

[<StructLayout(LayoutKind.Explicit, Size=44)>]
type internal SDL_TextInputEvent =
    struct
        [<FieldOffset(0)>]
        val Type: uint32
        [<FieldOffset(4)>]
        val Timestamp: uint32
        [<FieldOffset(8)>]
        val WindowID: uint32                            
        [<FieldOffset(12)>]
        val Text: byte//really a byte[32]
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_MouseMotionEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
        val WindowID: uint32
        val Which: uint32
        val State: uint32
        val X: int32
        val Y: int32
        val Xrel: int32
        val Yrel: int32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_MouseButtonEvent =
    struct
        val Type: uint32
        val Timestamp: uint32
        val WindowID: uint32
        val Which: uint32
        val Button: uint8
        val State: uint8
        val Clicks: uint8
        val Padding1: uint8
        val X: int32
        val Y: int32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_MouseWheelEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val WindowID:uint32
        val Which:uint32
        val X:int32
        val Y:int32
        val Direction:uint32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_JoyAxisEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Axis:uint8
        val Padding1:uint8
        val Padding2:uint8
        val Padding3:uint8
        val Value:int16
        val Padding4:uint16
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_JoyBallEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Ball:uint8
        val Padding1:uint8
        val Padding2:uint8
        val Padding3:uint8
        val Xrel:int16
        val Yrel:int16
    end 

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_JoyHatEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Hat:uint8
        val Value:uint8
        val Padding1:uint8
        val Padding2:uint8
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_JoyButtonEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Button:uint8
        val State:uint8
        val Padding1:uint8
        val Padding2:uint8
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_JoyDeviceEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_ControllerAxisEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Axis:uint8
        val Padding1:uint8
        val Padding2:uint8
        val Padding3:uint8
        val Value:int16
        val Padding4:uint16
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_ControllerButtonEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
        val Button:uint8
        val State:uint8
        val Padding1:uint8
        val Padding2:uint8
    end 

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_ControllerDeviceEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:int32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_AudioDeviceEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Which:uint32
        val Iscapture:uint8
        val Padding1:uint8
        val Padding2:uint8
        val Padding3:uint8
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_TouchFingerEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val TouchId:int64
        val FingerId:int64
        val X:float
        val Y:float
        val Dx:float
        val Dy:float
        val Pressure:float
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_MultiGestureEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val TouchId:int64
        val DTheta:float
        val DDist:float
        val X:float
        val Y:float
        val NumFingers:uint16
        val Padding:uint16
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_DollarGestureEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val TouchId:int64
        val GestureId:int64
        val NumFingers:uint32
        val Error:float
        val X:float
        val Y:float
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_DropEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val File:IntPtr
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_OSEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_UserEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val WindowID:uint32
        val Code:int32
        val Data1:IntPtr
        val Data2:IntPtr
    end

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_SysWMEvent=
    struct
        val Type:uint32
        val Timestamp:uint32
        val Msg:IntPtr
    end
    
[<StructLayout(LayoutKind.Explicit, Size=56)>]
type internal SDL_Event =
    struct
        [<FieldOffset(0)>]
        val Type: uint32
        [<FieldOffset(0)>]
        val Common:SDL_CommonEvent
        [<FieldOffset(0)>]
        val Window:SDL_WindowEvent
        [<FieldOffset(0)>]
        val Key:SDL_KeyboardEvent
        [<FieldOffset(0)>]
        val Edit:SDL_TextEditingEvent
        [<FieldOffset(0)>]
        val Text:SDL_TextInputEvent
        [<FieldOffset(0)>]
        val Motion:SDL_MouseMotionEvent
        [<FieldOffset(0)>]
        val Button:SDL_MouseButtonEvent
        [<FieldOffset(0)>]
        val Wheel:SDL_MouseWheelEvent
        [<FieldOffset(0)>]
        val Jaxis:SDL_JoyAxisEvent
        [<FieldOffset(0)>]
        val Jball:SDL_JoyBallEvent
        [<FieldOffset(0)>]
        val Jhat:SDL_JoyHatEvent
        [<FieldOffset(0)>]
        val Jbutton:SDL_JoyButtonEvent
        [<FieldOffset(0)>]
        val Jdevice:SDL_JoyDeviceEvent
        [<FieldOffset(0)>]
        val Caxis:SDL_ControllerAxisEvent
        [<FieldOffset(0)>]
        val Cbutton:SDL_ControllerButtonEvent
        [<FieldOffset(0)>]
        val Cdevice:SDL_ControllerDeviceEvent
        [<FieldOffset(0)>]
        val Adevice:SDL_AudioDeviceEvent
        [<FieldOffset(0)>]
        val Quit:SDL_QuitEvent
        [<FieldOffset(0)>]
        val User:SDL_UserEvent
        [<FieldOffset(0)>]
        val Syswm:SDL_SysWMEvent
        [<FieldOffset(0)>]
        val Tfinger:SDL_TouchFingerEvent
        [<FieldOffset(0)>]
        val Mgesture:SDL_MultiGestureEvent
        [<FieldOffset(0)>]
        val Dgesture:SDL_DollarGestureEvent
        [<FieldOffset(0)>]
        val Drop:SDL_DropEvent
    end

type EventType =
    | Quit                     = 0x100
    | AppTerminating           = 0x101
    | AppLowmemory             = 0x102
    | AppWillEnterBackground   = 0x103
    | AppDidEnterBackground    = 0x104
    | AppWillEnterForeground   = 0x105
    | AppDidEnterForeground    = 0x106
    | WindowEvent              = 0x200
    | SysWMEvent               = 0x201
    | KeyDown                  = 0x300
    | KeyUp                    = 0x301
    | TextEditing              = 0x302
    | TextInput                = 0x303
    | KeyMapChanged            = 0x304
    | MouseMotion              = 0x400
    | MouseButtonDown          = 0x401
    | MouseButtonUp            = 0x402
    | MouseWheel               = 0x403
    | JoyAxisMotion            = 0x600
    | JoyBallMotion            = 0x601
    | JoyHatMotion             = 0x602
    | JoyButtonDown            = 0x603
    | JoyButtonUp              = 0x604
    | JoyDeviceAdded           = 0x605
    | JoyDeviceRemoved         = 0x606
    | ControllerAxisMotion     = 0x650 
    | ControllerButtonDown     = 0x651   
    | ControllerButtonUp       = 0x652
    | ControllerDeviceAdded    = 0x653
    | ControllerDeviceRemoved  = 0x654
    | ControllerDeviceRemapped = 0x655
    | FingerDown               = 0x700
    | FingerUp                 = 0x701
    | FingerMotion             = 0x702
    | DollarGesture            = 0x800
    | DollarRecord             = 0x801
    | MultiGesture             = 0x802
    | ClipboardUpdate          = 0x900 
    | DropFile                 = 0x1000
    | AudioDeviceAdded         = 0x1100
    | AudioDeviceRemoved       = 0x1101      
    | RenderTargetsReset       = 0x2000
    | RenderDeviceReset        = 0x2001
    | UserEvent                = 0x8000
    | LastEvent                = 0xFFFF


module private Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Init")>]
    extern int SdlInit(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Quit")>]
    extern void SdlQuit()
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_CreateWindowAndRenderer(int width, int height, uint32 window_flags, IntPtr* window, IntPtr* renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyWindow(IntPtr window)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyRenderer(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_WaitEvent(SDL_Event* event);
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_PollEvent(SDL_Event* event);
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderClear(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_RenderPresent(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_SetRenderDrawColor(IntPtr renderer, uint8 r, uint8 g, uint8 b, uint8 a)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderFillRect(IntPtr renderer, SDL_Rect* rect)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderSetLogicalSize(IntPtr renderer, int w, int h)

type context =
    {window:IntPtr;
    renderer:IntPtr}

let run (width:int,height:int) (flags:WindowFlags) (runFunc:context->unit) : unit =
    match Native.SdlInit(Init.Everything |> uint32) with
    | 0 -> 
        let mutable window:IntPtr = IntPtr.Zero
        let mutable renderer:IntPtr = IntPtr.Zero
        if Native.SDL_CreateWindowAndRenderer(width, height, flags |> uint32, &&window, &&renderer) = 0 then
            {window=window;
            renderer=renderer}
            |> runFunc
        if window <> IntPtr.Zero then
            Native.SDL_DestroyWindow(window)
        if renderer <> IntPtr.Zero then
            Native.SDL_DestroyRenderer(renderer)
        Native.SdlQuit()
    | _ -> ()//error

let rec waitForKeyPress () : int32 option =
    let mutable event = new SDL_Event()
    match Native.SDL_WaitEvent(&&event) with
    | 1 -> 
        match event.Type |> int |> enum<EventType> with
        | EventType.Quit -> None
        | EventType.KeyDown -> event.Key.Keysym.Sym |> Some
        | _ -> waitForKeyPress()
    | _ -> None

let rec pollForKeyPress () : bool * (int32 option) =
    let mutable event = new SDL_Event()
    match Native.SDL_PollEvent(&&event) with
    | 1 -> 
        match event.Type |> int |> enum<EventType> with
        | EventType.Quit -> (false,None)
        | EventType.KeyDown -> (true, event.Key.Keysym.Sym |> Some)
        | _ -> (true,None)
    | _ -> (true,None)

let clear (context:context) : context = 
    Native.SDL_RenderClear(context.renderer)
    |> ignore
    context

let present (context:context) : context =
    Native.SDL_RenderPresent(context.renderer)
    |> ignore
    context

let setDrawColor (r:byte,g:byte,b:byte,a:byte) (context:context) :context = 
    Native.SDL_SetRenderDrawColor(context.renderer,r,g,b,a)
    |> ignore
    context

let setLogicalSize (w:int,h:int) (context:context) : context =
    Native.SDL_RenderSetLogicalSize(context.renderer, w,h)
    |> ignore
    context

let fillRect (x:int,y:int,w:int,h:int) (context:context) : context =
    let mutable rect = new SDL_Rect()
    rect.x <- x
    rect.y <- y
    rect.w <- w
    rect.h <- h
    Native.SDL_RenderFillRect(context.renderer, &&rect)
    |> ignore
    context
