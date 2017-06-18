open SDLContext

let boardColumns = 32
let boardRows = 32
let cellWidth = 7
let cellHeight = 7
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

let internal boardPositions = [for column in 0..(boardColumns-1) do for row in 0..(boardRows-1) do yield (column,row)] |> Set.ofList

let internal random = 
    new System.Random()

let makeCells (cell:BoardCell) (count:int) : BoardCell list =
    [for index in 1..count do yield cell]

let fireForLevel (level:int) : int =
    System.Math.Min(64 + 64 * level, 512)

let blocksForLevel (level:int) : int =
    System.Math.Min(32 + 8 * level, 128)
    
let goldForLevel (level:int) : int =
    64

let waterForLevel (level:int) : int =
    8

let internal generateLevel (game:GameState) : GameState =
    let cells = 
        []
        |> List.append (makeCells Fire (game.level |> fireForLevel))
        |> List.append (makeCells Block (game.level |> blocksForLevel))
        |> List.append (makeCells Gold (game.level |> goldForLevel))
        |> List.append (makeCells Water (game.level |> waterForLevel))
    let available = 
        boardPositions
        |> Set.remove game.player
        |> Set.toList
        |> List.sortBy (fun x-> random.Next())
        |> List.take (cells |> List.length)

    let board = 
        cells
        |> List.zip available
        |> Map.ofList

    {game with board = board}

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

    ctx
    |> SDLContext.setDrawColor (0uy,128uy,128uy,255uy)
    |> SDLContext.fillRect (0,0,boardWidth,boardHeight)
    |> ignore

    game.board
    |> Map.iter renderCell

    ctx
    |> SDLContext.setDrawColor SDLContext.Colors.green
    |> SDLContext.fillRect (game.player |> plotBoardCell)
    |> RomFont.renderString (boardWidth,0) (Some Colors.white, None) (sprintf "Level: %d" game.level)
    |> RomFont.renderString (boardWidth,8) (Some Colors.yellow, None) (sprintf "Score: %d" game.score)
    |> RomFont.renderString (boardWidth,16) (Some Colors.blue, None) (sprintf "Water: %d" game.protection)
    |> RomFont.renderString (boardWidth,24) (Some Colors.green, None) (sprintf "Moves: %d" game.moves)


type Direction =
    | North
    | East
    | South
    | West

let makeStep (direction:Direction) (column:int,row:int) : Position =
    match direction with
    | North -> (column,row-1)
    | East -> (column+1,row)
    | South -> (column,row+1)
    | West -> (column-1,row)

let internal checkForNextLevel (game:GameState) : GameState =
    if game.board |> Map.exists (fun _ v -> v = Gold) then
        game
    else
        {game with level = game.level + 1}
        |> generateLevel

let internal finalizeMove (game:GameState) : GameState =
    match game.board |> Map.tryFind game.player with
    | Some Gold -> 
        {game with board = game.board.Remove game.player; score = game.score + 1}
        |> checkForNextLevel
    | Some Water ->
        {game with board = game.board.Remove game.player; protection = game.protection + 1}
    | Some Fire ->
        if game.protection>0 then
            {game with board = game.board.Remove game.player; protection = game.protection - 1}
        else
            game
    | _ -> game

let internal move (direction:Direction) (game:GameState) : GameState =
    let (nextColumn, nextRow) = game.player |> makeStep direction
    if nextColumn < 0 || nextColumn >= boardColumns || nextRow<0 || nextRow>=boardRows || (Map.tryFind (nextColumn,nextRow) game.board) = Some Block then
        game
    else
        {game with player = (nextColumn,nextRow);board =game.board |> Map.add game.player Fire; moves = game.moves + 1}
        |> finalizeMove

let RightArrow = 79 + (1<<<30)
let LeftArrow = 80 + (1<<<30)
let DownArrow = 81 + (1<<<30)
let UpArrow = 82 + (1<<<30)

let rec internal gameOver (game:GameState) (ctx:SDLContext.context) : SDLContext.context option =
    ctx
    |> SDLContext.setDrawColor Colors.black
    |> SDLContext.clear
    |> RomFont.renderString (0,0) (Some Colors.white, None) "Game Over!"
    |> RomFont.renderString (0,8) (Some Colors.white, None) (sprintf "Final Score: %d" game.score)
    |> RomFont.renderString (0,16) (Some Colors.white, None) (sprintf "Total Moves: %d" game.moves)
    |> RomFont.renderString (0,24) (Some Colors.white, None) (sprintf "Score per 100 moves: %f" ((game.score |> float) * 100.0 / (game.moves |> float)))
    |> RomFont.renderString (0,32) (Some Colors.white, None) "[Esc] Back to Menu"
    |> SDLContext.present
    |> ignore
    match SDLContext.waitForKeyPress() with
    | None -> None
    | Some 27 -> ctx |> Some
    | _ -> ctx |> gameOver game

let rec internal play (game:GameState) (ctx:SDLContext.context) : SDLContext.context option =
    if (game.board |> Map.tryFind game.player) = Some Fire then
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
        | Some x when x = RightArrow -> ctx |> play (game |> move East)
        | Some x when x = LeftArrow -> ctx |> play (game |> move West)
        | Some x when x = DownArrow -> ctx |> play (game |> move South)
        | Some x when x = UpArrow -> ctx |> play (game |> move North)
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
    |> RomFont.renderString (0,8 ) (Some Colors.white, None) "You are a green square. You must collect"
    |> RomFont.renderString (0,16) (Some Colors.white, None) "all of the yellow squares to advance to"
    |> RomFont.renderString (0,24) (Some Colors.white, None) "the next level. Avoid the red squares,"
    |> RomFont.renderString (0,32) (Some Colors.white, None) "as they will kill you. Collect blue"
    |> RomFont.renderString (0,40) (Some Colors.white, None) "squares to allow one move onto a red"
    |> RomFont.renderString (0,48) (Some Colors.white, None) "square."
    |> RomFont.renderString (0,56) (Some Colors.white, None) "Good luck."
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
