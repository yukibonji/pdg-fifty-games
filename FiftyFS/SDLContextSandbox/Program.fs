open SDLContext

let rec private sdlRunnerLoop (ctx:SDLContext.context) : unit = 
    SDLContext.setDrawColor (0uy,0uy,0uy,255uy) ctx
    SDLContext.clear ctx
    RomFont.render (0,0) (Some (255uy,0uy,0uy,255uy),None) '@' ctx
    RomFont.renderString (0,8) (Some (255uy,0uy,0uy,255uy),None) "This is a test." ctx
    SDLContext.present ctx
    match SDLContext.waitForKeyPress() with
    | None | Some 27 -> ()
    | Some _ -> ctx |> sdlRunnerLoop

let sdlRunner (ctx:SDLContext.context) : unit =
    ctx
    |> SDLContext.setLogicalSize (320,240)

    ctx 
    |> sdlRunnerLoop

[<EntryPoint>]
let main argv =
    sdlRunner
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None
    0
