﻿open SDLContext

let private sdlRunner (ctx:SDLContext.context) : unit =
    let rec sdlRunnerLoop (ctx:SDLContext.context) : unit = 
        ctx
        |> SDLContext.setDrawColor StandardColors.black
        |> SDLContext.clear
        |> RomFont.render (0,0) (Some StandardColors.red,None) '@'
        |> RomFont.renderString (0,8) (Some StandardColors.yellow,None) "This is a test."
        |> SDLContext.present
        |> ignore
        match SDLContext.pollForKeyPress() with
        | (false,None) | (true,Some 27) -> ()
        | _ -> ctx |> sdlRunnerLoop

    ctx
    |> SDLContext.setLogicalSize (320,240)
    |> sdlRunnerLoop

[<EntryPoint>]
let main argv =
    sdlRunner
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None
    0
