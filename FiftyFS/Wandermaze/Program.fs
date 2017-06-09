open SDLContext

let boardColumns = 32
let boardRows = 32
let cellWidth = 15
let cellHeight = 15
let boardWidth = boardColumns * cellWidth
let boardHeight = boardRows * cellHeight

type Position = int * int

type BoardCell =
    | Block
    | Water
    | Fire
    | Gold

type GameState = 
    {board:Map<Position,BoardCell>;
    player:Position;
    level:int;
    score:int;
    moves:int;
    protection:int}

let internal random = 
    new System.Random()

let internal generateLevel (game:GameState) : GameState =
    game

let boardCellColor = function
    | Block -> SDLContext.Colors.black
    | Water -> SDLContext.Colors.blue
    | Fire -> SDLContext.Colors.red
    | Gold -> SDLContext.Colors.yellow

let plotBoardCell = function
    | (x,y) -> (x*cellWidth,y*cellHeight,cellWidth,cellHeight)

let internal renderGame (game:GameState) (ctx:SDLContext.context) : SDLContext.context =
    let renderCell (position:Position) (boardCell:BoardCell) : unit =
        ctx
        |> SDLContext.setDrawColor (boardCell |> boardCellColor)
        |> SDLContext.fillRect (position |> plotBoardCell)
        |> ignore

    game.board
    |> Map.iter renderCell

    ctx
    |> SDLContext.setDrawColor SDLContext.Colors.green
    |> SDLContext.fillRect (game.player |> plotBoardCell)

let rec internal play (game:GameState) (ctx:SDLContext.context) : SDLContext.context =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> renderGame game
    |> SDLContext.present
    |> ignore
    match SDLContext.waitForKeyPress() with
    | None | Some 27 -> ctx
    | _ -> ctx |> play game

let internal createGame() : GameState =
    {board=Map.empty;
    player=(random.Next(boardColumns),random.Next(boardRows));
    level=1;
    score=0;
    moves=0;
    protection=0}
    |> generateLevel

let rec internal instructions (ctx:SDLContext.context) : SDLContext.context =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.renderString (0,0) (Some Colors.white, None) "Instructions"
    |> RomFont.renderString (0,8) (Some Colors.white, None) "[Esc] Back to Menu"
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
        |> RomFont.renderString (0,0) (Some Colors.white, None) "Wandermaze"
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
            |> mainMenuLoop
        | Some 113 -> 
            if ctx |> confirmQuit then
                ()
            else
                ctx |> mainMenuLoop
        | _ -> ctx |> mainMenuLoop

    ctx
    |> SDLContext.setLogicalSize (640,480)
    |> mainMenuLoop

[<EntryPoint>]
let main argv =
    mainMenu
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None
    0
