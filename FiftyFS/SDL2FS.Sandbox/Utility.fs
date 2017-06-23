namespace Sdl

open System.Text
open System.Runtime.InteropServices
open System
open Microsoft.FSharp.NativeInterop


module internal Utility =
    
    let private allocString (encoder:string->byte[]) (text:string) =
        let bytes = encoder(text)
        let pinnedArray = GCHandle.Alloc(bytes, GCHandleType.Pinned)
        pinnedArray

    let private withString (encoder:string->byte[]) (func:IntPtr->'T) (text:string) =
        let pinnedArray =
            (encoder,text)
            ||> allocString
        let result = pinnedArray.AddrOfPinnedObject() |> func
        pinnedArray.Free()
        result

    let internal allocUtf8String (text:string) =
        allocString Encoding.UTF8.GetBytes text

    let internal withUtf8String (func:IntPtr->'T) (text:string) =
        withString Encoding.UTF8.GetBytes func text

    let internal withAsciiString (func: IntPtr->'T) (text:string) =
        withString Encoding.ASCII.GetBytes func text


    let private intPtrToString (encoder:byte[]->string) (ptr:IntPtr):string = 
        if ptr = IntPtr.Zero then
            null
        else
            let byteEmitter (bytePtr:nativeptr<byte>) =
                match bytePtr |> NativePtr.read with
                | 0uy ->  None
                | nextByte -> Some (nextByte, 1 |> NativePtr.add bytePtr)

            ptr 
            |> NativePtr.ofNativeInt<byte>
            |> Seq.unfold byteEmitter
            |> Seq.toArray
            |> encoder


    let internal intPtrToStringUtf8 =
        intPtrToString Encoding.UTF8.GetString


    let internal intPtrToStringAscii=
        intPtrToString Encoding.ASCII.GetString

