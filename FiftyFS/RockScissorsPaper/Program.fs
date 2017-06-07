open System

let goal:int = 3

type GameState = 
    {playerWins:int;
    computerWins:int;
    ties:int}

let determineRoundNumber (gameState:GameState) : int =
    gameState.computerWins + gameState.playerWins + gameState.ties + 1

let createGame() : GameState =
    {playerWins=0;
    computerWins=0;
    ties=0}

let gameOver (gameState:GameState) : bool =
    gameState.playerWins >= goal || gameState.computerWins >=goal

type Choice =
    | Rock
    | Scissors
    | Paper

let random = new Random()
let makeComputerChoice () : Choice =
    match random.Next(3) with
    | 0 -> Rock
    | 1 -> Scissors
    | _ -> Paper

let displayPlayerChoice (choice:Choice) : Choice =
    match choice with
    | Rock -> printfn "You chose Rock."
    | Scissors -> printfn "You chose Scissors."
    | Paper -> printfn "You chose Paper."
    choice

let displayComputerChoice (choice:Choice) : Choice =
    match choice with
    | Rock -> printfn "I chose Rock."
    | Scissors -> printfn "I chose Scissors."
    | Paper -> printfn "I chose Paper."
    choice

let makeChoice (choice:Choice) (gameState:GameState) : GameState = 
    printfn ""
    match (choice |> displayPlayerChoice, makeComputerChoice() |> displayComputerChoice) with
    | (Rock, Rock)     | (Scissors, Scissors) | (Paper, Paper) -> 
        printfn "Tie!"
        {gameState with ties = gameState.ties + 1}

    | (Rock, Scissors) | (Scissors, Paper)    | (Paper, Rock)  -> 
        printfn "One point to you!"
        {gameState with playerWins = gameState.playerWins + 1}
    | _                                                        -> 
        printfn "One point to me!"
        {gameState with computerWins = gameState.computerWins + 1}

let rec play (show:bool) (gameState:GameState) : unit =
    if gameState |> gameOver then
        printfn ""
        match gameState with
        | x when x.computerWins>=goal ->
            printfn "I win!"
        | _ ->
            printfn "You win!"
    else
        if show then
            printfn ""
            gameState
            |> determineRoundNumber
            |> printfn "Round # %i"
            printfn "Score: %i - %i - %i" gameState.playerWins gameState.computerWins gameState.ties
            printfn "What is your choice?"
            printfn "[R]ock"
            printfn "[S]cissors"
            printfn "[P]aper"

        let choice : Choice option = 
            match Console.ReadKey(true).Key with
            | ConsoleKey.R -> Some Rock
            | ConsoleKey.S -> Some Scissors
            | ConsoleKey.P -> Some Paper
            | _ -> None

        match choice with
        | Some x -> 
            gameState
            |> makeChoice x
            |> play true
        | None -> 
            gameState 
            |> play false

let instructions () : unit =
    printfn ""
    printfn "How to Play:"
    printfn "This is a game of Rock, Scissors Paper."
    printfn "It takes place in rounds. During each round, you and I choose one of Rock, Scissors, or Paper."
    printfn "Rock beats Scissors, Scissors beats Paper, and Paper beats Rock. Playing the same thing is a tie."
    printfn "Play continues until one of us has won three rounds."

let rec confirmQuit (show:bool) : bool =
    if show then
        printfn ""
        printfn "Are you sure you want to quit?"
        printfn "[Y]es"
        printfn "[N]o"

    match Console.ReadKey(true).Key with
    | ConsoleKey.Y -> true
    | ConsoleKey.N -> false
    | _ -> confirmQuit false

let rec mainMenu (show:bool) : unit =
    if show then
        printfn ""
        printfn "Rock, Scissors, Paper"
        printfn "[P]lay"
        printfn "[I]nstructions"
        printfn "[Q]uit"

    match Console.ReadKey(true).Key with
    | ConsoleKey.P ->
        createGame()
        |> play true
        mainMenu true

    | ConsoleKey.I ->
        instructions()
        mainMenu true

    | ConsoleKey.Q -> 
        if confirmQuit true then
            ()
        else
            mainMenu true

    | _ -> 
        mainMenu false

[<EntryPoint>]
let main argv = 
    mainMenu true
    0
