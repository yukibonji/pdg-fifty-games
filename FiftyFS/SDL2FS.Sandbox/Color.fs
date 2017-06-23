namespace Sdl

#nowarn "9"

open System.Runtime.InteropServices

[<StructLayout(LayoutKind.Sequential)>]
type internal SDL_Color =
    struct
        val mutable r: uint8
        val mutable g: uint8
        val mutable b: uint8
        val mutable a: uint8
    end

type color = byte * byte * byte * byte


module Color = 

    let internal ofSDL_Color (color:SDL_Color) : color =
        (color.r, color.g, color.b, color.a)
        
    let internal toSDL_Color (color:color) : SDL_Color =
        let r,g,b,a = color
        let mutable result = new SDL_Color()
        result.r <- r
        result.g <- g
        result.b <- b
        result.a <- a
        result

    let getRed  = 
        function
        | (x,_,_,_) -> x

    let getGreen  = 
        function
        | (_,x,_,_) -> x

    let getBlue  = 
        function
        | (_,_,x,_) -> x

    let getAlpha  = 
        function
        | (_,_,_,x) -> x

    let setRed (red:byte) (color:color) : color =
        (red, color |> getGreen, color |> getBlue, color |> getAlpha)

    let setGreen (green:byte) (color:color) : color =
        (color |> getRed, green, color |> getBlue, color |> getAlpha)

    let setBlue (blue:byte) (color:color) : color =
        (color |> getRed, color |> getGreen, blue, color |> getAlpha)

    let setAlpha (alpha:byte) (color:color) : color =
        (color |> getRed, color |> getGreen, color |> getBlue, alpha)

