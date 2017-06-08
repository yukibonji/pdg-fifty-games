open System

type PromptResponse<'TResult> =
    | Return of 'TResult
    | ConditionalReturn of (unit->bool) * bool * 'TResult
    | Continue of (unit->unit)
    | ContinueRefresh of (unit->unit)


let prompt (onShow:unit->unit) (input:unit->'TInput) (inputHandlers:Map<'TInput, PromptResponse<'TResult>>) : 'TResult =
    let rec promptLoop (onShow:unit->unit) (input:unit->'TInput) (inputHandlers:Map<'TInput, PromptResponse<'TResult>>) (show:bool) : 'TResult =
        if show then 
            onShow()

        match (input(), inputHandlers) ||> Map.tryFind with
        | Some (Return result) -> 
            result

        | Some (ConditionalReturn (test,refresh,result)) ->
            if test() then
                result
            else
               promptLoop onShow input inputHandlers refresh 

        | Some (Continue action) -> 
            action()
            promptLoop onShow input inputHandlers false

        | Some (ContinueRefresh action) ->
            action()
            promptLoop onShow input inputHandlers true

        | None -> 
            promptLoop onShow input inputHandlers false

    promptLoop onShow input inputHandlers true

let chambers : int = 6

type GameState =
    {rounds:int;
    alive:bool}

let createGame () : GameState =
    {rounds=0;
    alive=true}

let random = new Random()
let checkChamber (chamber:int) (gameState:GameState) : GameState =
    if random.Next(1,chambers+1) = chamber then
        printfn ""
        printfn "*BANG*"
        {gameState with alive=false}
    else
        printfn ""
        printfn "*CLICK*"
        {gameState with rounds = gameState.rounds + 1}

let rec chooseChamber (show:bool) (gameState:GameState) : GameState =
    if show then
        printfn ""
        printfn "Choose a chamber [1-%i]" chambers

    match Int32.TryParse(Console.ReadLine()) with
    | (true, x) when x<1 || x>chambers ->
        printfn ""
        printfn "That number is out of range!"
        gameState
        |> chooseChamber true

    | (true, x) when x>=1 || x<=chambers ->
        gameState
        |> checkChamber x

    | _ ->
        printfn ""
        printfn "That is not a number!"
        gameState
        |> chooseChamber true

let play (gameState:GameState) : unit =
    let rec playLoop (show:bool) (gameState:GameState) : unit =
        if gameState.alive then
            if show then
                printfn ""
                printfn "You survived %i rounds." gameState.rounds
                printfn "Would you like to..."
                printfn "[W]alk away"
                printfn "[C]hoose a chamber"

            match Console.ReadKey(true).Key with
            | ConsoleKey.C ->
                gameState
                |> chooseChamber true
                |> playLoop true

            | ConsoleKey.W ->
                printfn ""
                printfn "You walk away."
                printfn "You survived %i rounds." gameState.rounds
                printfn "More importantly, you are alive."

            | _ -> 
                gameState 
                |> playLoop false
        else
            printfn ""
            printfn "You are dead."
            printfn "You survived %i rounds." gameState.rounds

    playLoop true gameState

let instructions () : unit =
    printfn ""
    printfn "How to Play:"
    printfn "Game takes place in rounds."
    printfn "There are %i chambers. One of them has a bullet randomly placed within it each round." chambers
    printfn "Each round, you choose whether or not to choose a chamber or walk away."
    printfn "If you walk away, your final score is the number of rounds you have survived."
    printfn "If you choose the chamber with the bullet, you die, and your score is meaningless."
    printfn "If you choose a chamber without the bullet, you get one point and may play another round."

let confirmQuit () =
    let onShow () : unit =
        printfn ""
        printfn "Are you sure you want to quit?"
        printfn "[Y]es"
        printfn "[N]o"

    let input() : ConsoleKey = 
        Console.ReadKey(true).Key

    let inputHandlers =
        Map.empty
        |> Map.add ConsoleKey.Y (Return true)
        |> Map.add ConsoleKey.N (Return false)

    prompt onShow input inputHandlers
        

let mainMenu () : unit =
    let onShow () : unit =
        printfn ""
        printfn "Russian Roulette"
        printfn "[P]lay"
        printfn "[I]nstructions"
        printfn "[Q]uit"

    let input() : ConsoleKey =
        Console.ReadKey(true).Key

    let inputHandlers =
        Map.empty
        |> Map.add ConsoleKey.P (ContinueRefresh (createGame >> play))
        |> Map.add ConsoleKey.I (ContinueRefresh instructions)
        |> Map.add ConsoleKey.Q (ConditionalReturn (confirmQuit ,true, ()))

    prompt onShow input inputHandlers

[<EntryPoint>]
let main argv = 
    mainMenu ()
    0
