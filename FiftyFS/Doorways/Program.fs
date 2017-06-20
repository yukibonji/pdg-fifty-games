
type cellType = Room | Passageway

type cell = 
    {exits:Set<CardinalDirection.direction>;priorVisits:int}

type cellMap = Map<int*int,cell>

let pickRandomlyFromList (random:System.Random) (l:'TItem list) : 'TItem option =
    match l with
    | [] -> None
    | x ->
        x
        |> List.sortBy (fun _ -> random.Next())
        |> List.head
        |> Some

let pickRandomCellLocation (random:System.Random) (cellMap:cellMap) : (int*int) option=
    cellMap
    |> Map.toList
    |> List.map fst
    |> pickRandomlyFromList random

let generateCellMap (random:System.Random) (cellCount:int) : cellMap =
    
    let rec generateCells (random:System.Random) (cellCount:int) (cellMap:cellMap) : cellMap =

        if cellCount<=0 then
            cellMap        
        else
            if cellMap |> Map.isEmpty then
                cellMap
                |> Map.add (0,0) {exits=Set.empty; priorVisits=0}
                |> generateCells random (cellCount-1) 
            else
                let cellLocation = 
                    pickRandomCellLocation random cellMap
                let neighborCells = 
                    CardinalDirection.directions
                    |> List.map (fun d -> (d,(CardinalDirection.step 1 d cellLocation.Value)))
                    |> List.filter (fun (d,p) -> (cellMap |> Map.containsKey p |> not))
                match neighborCells with
                | [] -> 
                    cellMap
                    |> generateCells random cellCount
                | _ -> 
                    let choice = 
                        pickRandomlyFromList random neighborCells
                    let startCell =
                        {cellMap.[cellLocation.Value] with exits = cellMap.[cellLocation.Value].exits |> Set.add (choice.Value|> fst)}
                    let endCell = 
                        {exits=Set.empty |> Set.add (choice.Value|> fst |> CardinalDirection.opposite); priorVisits = 0}
                    cellMap
                    |> Map.add (cellLocation.Value) (startCell)
                    |> Map.add (choice.Value |> snd) (endCell)
                    |> generateCells random (cellCount-1)

    let cellMap = 
        Map.empty
        |> generateCells random cellCount

    let offsetX, offsetY =
        ((0,0),cellMap)
        ||> Map.fold (fun (x,y) (kx,ky) _ -> 
            ((if x < kx then x else kx),(if y < ky then y else ky)))

    (Map.empty,cellMap)
    ||> Map.fold (fun acc (x,y) v -> 
        acc
        |> Map.add (x-offsetX,y-offsetY) v)

type gameState = 
    {cellMap:cellMap;
    position:int*int}

let attemptMove (direction:CardinalDirection.direction) (gameState:gameState) : bool * gameState =
    let cell = 
        gameState.cellMap.[gameState.position]
    if cell.exits.Contains(direction) then
        (true,
            {gameState with 
                position=gameState.position |> CardinalDirection.step 1 direction;  
                cellMap = gameState.cellMap |> Map.add gameState.position {cell with priorVisits = cell.priorVisits+1}})
    else            
        (false,gameState)
    

let run (gameState:gameState) : unit = 

    let rec runLoop (show:bool) (gameState:gameState) : unit =
        if show then
            let cell = 
                gameState.cellMap.[gameState.position]
            printfn ""
            printfn "You are at (%d,%d)." (gameState.position |> fst) (gameState.position |> snd)
            printfn "You have been here %d times previously." (cell.priorVisits)
            if cell.exits.Contains(CardinalDirection.North) then
                printfn "[N]orth"
            if cell.exits.Contains(CardinalDirection.East) then
                printfn "[E]ast"
            if cell.exits.Contains(CardinalDirection.South) then
                printfn "[S]outh"
            if cell.exits.Contains(CardinalDirection.West) then
                printfn "[W]est"
        match System.Console.ReadKey(true).Key with
        | System.ConsoleKey.N -> 
            (CardinalDirection.North, gameState)
            ||> attemptMove
            ||> runLoop

        | System.ConsoleKey.E -> 
            (CardinalDirection.East, gameState)
            ||> attemptMove
            ||> runLoop

        | System.ConsoleKey.S -> 
            (CardinalDirection.South, gameState)
            ||> attemptMove
            ||> runLoop

        | System.ConsoleKey.W -> 
            (CardinalDirection.West, gameState)
            ||> attemptMove
            ||> runLoop

        | _ -> gameState |> runLoop false

    gameState
    |> runLoop true


[<EntryPoint>]
let main argv = 
    let random = new System.Random()
    let cellMap = 
        generateCellMap random 200
    let gameState =
        {cellMap = cellMap;
        position = (pickRandomCellLocation random cellMap)|>Option.get}
    gameState
    |> run
    0
