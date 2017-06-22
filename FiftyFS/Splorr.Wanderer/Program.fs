open Pdg.Common
open Splorr.Wanderer
open SDLContext

let terrainTypeGenerator =
    [(terrainType.Grass,8000);
    (terrainType.Tree,2000);
    (terrainType.Rock,500);
    (terrainType.FlintRock,50)]
    |> Map.ofList

let terrainGenerators =
    [(terrainType.Grass,fun (randomizer:Generator.randomizer) -> terrain.Grass);
    (terrainType.Tree,fun (randomizer:Generator.randomizer) -> terrain.Tree {wood=10});
    (terrainType.Rock,fun (randomizer:Generator.randomizer) -> terrain.Rock {stone=10});
    (terrainType.FlintRock,fun (randomizer:Generator.randomizer) -> terrain.FlintRock {flint=10})]
    |> Map.ofList

let boardColumns = 32
let boardRows = 16
let cellWidth = 32
let cellHeight = 32

type direction =
    | North
    | East
    | South
    | West

type Avatar =
    {position:location;
    facing:direction}

type GameState =
    {board:board<terrain,unit,creature,unit>;
    avatar:Avatar}

let terrainTypeImages =
    [(terrainType.Grass, ((Colors.darkjade,Colors.onyx),'.'));
    (terrainType.Tree, ((Colors.darkjade,Colors.onyx),'Y'));
    (terrainType.Rock, ((Colors.darkjade,Colors.onyx),'*'));
    (terrainType.FlintRock, ((Colors.darkjade,Colors.onyx),'*'))]
    |> Map.ofList

let creatureTypeImages = 
    [(creatureType.Tagon, ((Colors.medium,Colors.transparent),'@'))]
    |> Map.ofList

let createGame (randomizer:Generator.randomizer) : GameState =
    let board = 
        (Map.empty, [for column in [0..(boardColumns-1)] do
                        for row in [0..(boardRows-1)] do
                            yield (column,row)])
        ||> List.fold (fun (board:board<terrain,unit,creature,unit>) location -> 
            let terrainType = 
                terrainTypeGenerator
                |> Generator.generate randomizer

            let terrain =
                (terrainGenerators
                |> Map.find terrainType.Value)(randomizer)

            board
            |> Board.setTerrain location terrain)

    {board=board;avatar={position=(0,0);facing=[North;East;South;West] |> Generator.ofList |> Generator.generate randomizer |> Option.get}}

let private renderGame (game:GameState) (ctx:SDLContext.context) : SDLContext.context =
    game.board
    |> Map.iter (fun (x,y) cell -> 
        ctx
        |> RomFont.
        ())
    ctx

let rec private playGame (randomizer:Generator.randomizer) (game:GameState) (ctx:SDLContext.context) : SDLContext.context option =
    let ctx' =
        ctx
        |> SDLContext.setDrawColor Colors.onyx.Value
        |> SDLContext.clear
        |> renderGame game
        |> SDLContext.present
    match SDLContext.pollForKeyPress() with
    | (false,_) -> None
    | (true,Some 27) -> ctx' |> Some
    | (true,_) -> 
        ctx' |> playGame randomizer game
    
let private newGame (ctx:SDLContext.context) : SDLContext.context option=
    let random = new System.Random()
    let randomizer (max:int) : int = random.Next(max)

    let game =
        randomizer
        |> createGame

    (game,ctx)
    ||> playGame randomizer

let private sdlRunner (ctx:SDLContext.context) : unit =
    let rec sdlRunnerLoop (ctx:SDLContext.context) : unit = 
        ctx
        |> SDLContext.setDrawColor Colors.onyx.Value
        |> SDLContext.clear
        |> RomFont.renderString (0,0) (Colors.silver,Colors.transparent) "Wanderer of Splorr!!"
        |> RomFont.renderString (0,16) (Colors.silver,Colors.transparent) "[N]ew Game"
        |> SDLContext.present
        |> ignore
        match SDLContext.pollForKeyPress() with
        | (false,_) | (true,Some 27) -> ()
        | (true,None) -> 
            ctx |> sdlRunnerLoop
        | (true,Some 110) -> 
            ctx |> newGame |> Option.iter sdlRunnerLoop
        | (true,c) -> 
            ctx |> sdlRunnerLoop

    ctx
    |> SDLContext.setLogicalSize (320,240)
    |> sdlRunnerLoop

[<EntryPoint>]
let main argv = 
    sdlRunner
    |>SDLContext.run (640,480) SDLContext.WindowFlags.None

    0
