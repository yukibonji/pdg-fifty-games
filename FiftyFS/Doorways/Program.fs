
type cellType = Room | Passageway

type cell = 
    {exits:Set<CardinalDirection.direction>}

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
                |> Map.add (0,0) {exits=Set.empty}
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
                        {exits=Set.empty |> Set.add (choice.Value|> fst |> CardinalDirection.opposite)}
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

let run (gameState:gameState) : unit = 

    let rec runLoop (show:bool) (gameState:gameState) : unit =
        let cell = 
            gameState.cellMap.[gameState.position]
        if show then
            printfn ""
            printfn "You are at (%d,%d)" (gameState.position |> fst) (gameState.position |> snd)
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
            if cell.exits.Contains(CardinalDirection.North) then
                {gameState with position=gameState.position |> CardinalDirection.step 1 CardinalDirection.North} |> runLoop true
            else            
                gameState |> runLoop false

        | System.ConsoleKey.E -> 
            if cell.exits.Contains(CardinalDirection.East) then
                {gameState with position=gameState.position |> CardinalDirection.step 1 CardinalDirection.East} |> runLoop true
            else            
                gameState |> runLoop false

        | System.ConsoleKey.S -> 
            if cell.exits.Contains(CardinalDirection.South) then
                {gameState with position=gameState.position |> CardinalDirection.step 1 CardinalDirection.South} |> runLoop true
            else            
                gameState |> runLoop false

        | System.ConsoleKey.W -> 
            if cell.exits.Contains(CardinalDirection.West) then
                {gameState with position=gameState.position |> CardinalDirection.step 1 CardinalDirection.West} |> runLoop true
            else            
                gameState |> runLoop false

        | _ -> gameState |> runLoop false

    gameState
    |> runLoop true


[<EntryPoint>]
let main argv = 
    let random = new System.Random()
    let cellMap = 
        generateCellMap random 20
    let gameState =
        {cellMap = cellMap;
        position = (pickRandomCellLocation random cellMap)|>Option.get}
    gameState
    |> run
    0
