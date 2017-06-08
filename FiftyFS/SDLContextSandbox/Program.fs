open SDLContext

let rec private sdlRunnerLoop (ctx:SDLContext.context) : unit = 
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.render (0,0) (Some Colors.red,None) '@'
    |> RomFont.renderString (0,8) (Some Colors.yellow,None) "This is a test."
    |> SDLContext.present
    |> ignore
    match SDLContext.pollForKeyPress() with
    | (false,None) | (true,Some 27) -> ()
    | _ -> ctx |> sdlRunnerLoop

let private sdlRunner (ctx:SDLContext.context) : unit =
    ctx
    |> SDLContext.setLogicalSize (320,240)
    |> sdlRunnerLoop

[<EntryPoint>]
let main argv =
    sdlRunner
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None
    0
