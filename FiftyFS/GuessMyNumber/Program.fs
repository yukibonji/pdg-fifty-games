//Guess My Number
//A simple console game in F#
//The purpose of this project is to demonstrate a simple game-like application that is well understood.
//My F# style varies considerably from the accepted norms, and if you are looking to my code for the "right way", you are looking in the wrong place
//This application is a good simple use of 'rec' and 'match'.
//You won't find many 'let' bindings inside of a function body. They just weren't necessary.
//You may notice that I rarely use type inference. I'm spent countless hours staring at my entire codebase underlined in red to make much use of it.

//For the pedantic or otherwise zealous:
//1. Random is random enough for this purpose
//2. My use of 'printfn' is only to escape the notice of the idiom police. I'd just a soon use Console.WriteLine.
//3. My answer to any question that starts with "why did/didn't you...?" is an menacing glare.

open System

let minimumValue:int = 1
let maximumValue:int = 100

type GameState=
    {guessCount:int;
    actual:int}

let createGame () : GameState=
    {guessCount=0;
    actual=(new System.Random()).Next(minimumValue,maximumValue + 1)}

let addGuess (gameState:GameState) : GameState =
    {gameState with guessCount=gameState.guessCount+1}

let rec play (gameState:GameState) : unit =
    printfn ""
    printfn "Gruess my number [%i-%i]!" minimumValue maximumValue
    match Console.ReadLine() |> Int32.TryParse with
    | (true,n) ->
        if n<minimumValue || n>maximumValue then
            printfn ""
            printfn "That number is out of range!"
            gameState
            |> play
        else
            match n with
            | x when x = gameState.actual ->
                printfn ""
                printfn "Correct!"
                printfn "It took you %i guesses." gameState.guessCount

            | x when x < gameState.actual ->
                printfn ""
                printfn "Too low!"
                gameState
                |> addGuess
                |> play
                
            | _ -> //it must, ergo, be too high!
                printfn ""
                printfn "Too high!"
                gameState
                |> addGuess
                |> play

    | _ ->
        printfn ""
        printfn "That is not a number!"
        gameState
        |> play


let instructions () : unit =
    printfn ""
    printfn "How to Play:"
    printfn "I will pick a number between %i and %i." minimumValue maximumValue
    printfn "You will then make guesses, and I will tell you if you are correct, too high, or too low."
    printfn "Once you have guessed correctly, I will tell you how many guesses it took you."
    

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
        printfn "Guess My Number [%i-%i]" minimumValue maximumValue
        printfn "[P]lay"
        printfn "[I]nstructions"
        printfn "[Q]uit"

    match Console.ReadKey(true).Key with
    | ConsoleKey.P -> 
        createGame()
        |> play
        mainMenu true

    | ConsoleKey.I ->
        instructions()
        mainMenu true

    | ConsoleKey.Q -> 
        if confirmQuit true then 
            () 
        else 
            mainMenu true

    | _ -> mainMenu false

[<EntryPoint>]
let main argv = 
    mainMenu true
    0
