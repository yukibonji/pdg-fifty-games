open SDLContext

type GameState =
    {year:int;
    population:int;
    starved:int;
    immigrants:int;
    bushels:int;
    bushelsLost:int;
    acres:int;
    harvestPerAcre:int;
    acrePrice:int}

let rec internal gameOver (game:GameState) (ctx:SDLContext.context) : SDLContext.context option =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.renderString (0,0) (Some Colors.white, None) "Game Over!"
    |> RomFont.renderString (0,32) (Some Colors.white, None) "[Esc] Back to Menu"
    |> SDLContext.present
    |> ignore
    match SDLContext.waitForKeyPress() with
    | None -> None
    | Some 27 -> ctx |> Some
    | _ -> ctx |> gameOver game

let internal renderGame (game:GameState) (ctx:SDLContext.context) : SDLContext.context =
    ctx
    |> RomFont.renderString (0,0) (Some Colors.white, None) (sprintf "Year: %d" game.year)
    |> RomFont.renderString (0,8) (Some Colors.white, None) (sprintf "Starved People: %d" game.starved)
    |> RomFont.renderString (0,16) (Some Colors.white, None) (sprintf "New People: %d" game.immigrants)
    |> RomFont.renderString (0,24) (Some Colors.white, None) (sprintf "Total People: %d" (game.immigrants + game.population))
    |> RomFont.renderString (0,32) (Some Colors.white, None) (sprintf "Acres: %d" game.acres)
    |> RomFont.renderString (0,40) (Some Colors.white, None) (sprintf "Harvest (per acre): %d" game.harvestPerAcre)
    |> RomFont.renderString (0,48) (Some Colors.white, None) (sprintf "Bushels lost (to rats): %d" game.bushelsLost)
    |> RomFont.renderString (0,56) (Some Colors.white, None) (sprintf "Bushels stored: %d" game.bushels)
    |> RomFont.renderString (0,64) (Some Colors.white, None) (sprintf "Acre price (in bushels): %d" game.acrePrice)
    |> RomFont.renderString (0,72) (Some Colors.white, None) "[B]uy Acres"
    |> RomFont.renderString (0,80) (Some Colors.white, None) "[S]ell Acres"
    |> RomFont.renderString (0,88) (Some Colors.white, None) "[E]nd Turn"

let rec internal play (game:GameState) (ctx:SDLContext.context) : SDLContext.context option =
    if game.year>10 then
        ctx
        |> gameOver game
    else
        ctx
        |> SDLContext.setDrawColor Colors.black
        |> SDLContext.clear
        |> renderGame game
        |> SDLContext.present
        |> ignore
        match SDLContext.waitForKeyPress() with
        | None -> None
        | Some 27 -> ctx |> Some
        | _ -> ctx |> play game

let random: System.Random = new System.Random()

let internal createGame () : GameState =
    {year=1;population=95;starved=0;immigrants=5;bushels=2800;bushelsLost=200;acres=1000;harvestPerAcre=3;acrePrice=random.Next(10)+17}

let rec internal instructions (ctx:SDLContext.context) : SDLContext.context =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.renderString (0,0) (Some Colors.white, None) "Instructions"
    |> RomFont.renderString (0,232) (Some Colors.white, None) "[Esc] Back to Menu"
    |> SDLContext.present
    |> ignore
    match SDLContext.waitForKeyPress() with
    | None | Some 27 -> ctx
    | _ -> ctx |> instructions

let rec internal confirmQuit (ctx:SDLContext.context) : bool =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.renderString (0,0) (Some Colors.white, None) "Are you sure you want to quit?"
    |> RomFont.renderString (0,8) (Some Colors.white, None) "[Y]es"
    |> RomFont.renderString (0,16) (Some Colors.white, None) "[N]o"
    |> SDLContext.present
    |> ignore
    match SDLContext.waitForKeyPress() with
    | None -> true
    | Some 121 -> true
    | Some 110 -> false
    | _ -> ctx |> confirmQuit

let internal mainMenu (ctx:SDLContext.context) : unit =
    let rec mainMenuLoop (ctx:SDLContext.context) : unit = 
        ctx
        |> SDLContext.setDrawColor Colors.black
        |> SDLContext.clear
        |> RomFont.renderString (0,0) (Some Colors.white, None) "Hamurabi"
        |> RomFont.renderString (0,8) (Some Colors.white, None) "[P]lay"
        |> RomFont.renderString (0,16) (Some Colors.white, None) "[I]nstructions"
        |> RomFont.renderString (0,24) (Some Colors.white, None) "[Q]uit"
        |> SDLContext.present
        |> ignore
        match SDLContext.waitForKeyPress() with
        | None -> ()
        | Some 105 ->
            ctx 
            |> instructions
            |> mainMenuLoop
        | Some 112 ->
            (createGame(), ctx)
            ||> play
            |> Option.iter mainMenuLoop
        | Some 113 -> 
            if ctx |> confirmQuit then
                ()
            else
                ctx |> mainMenuLoop
        | _ -> ctx |> mainMenuLoop

    ctx
    |> SDLContext.setLogicalSize (320,240)
    |> mainMenuLoop

[<EntryPoint>]
let main argv =
    mainMenu
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None
    0