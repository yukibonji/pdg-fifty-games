
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

let generateCellMap (random:System.Random) (cellCount:int) : cellMap =
    
    let rec generateCells (random:System.Random) (cellCount:int) (cellMap:cellMap) : cellMap =

        let pickRandomCellLocation (random:System.Random) (cellMap:cellMap) : (int*int) option=
            cellMap
            |> Map.toList
            |> List.map fst
            |> pickRandomlyFromList random

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

    Map.empty
    |> generateCells random cellCount


[<EntryPoint>]
let main argv = 
    let random = new System.Random()
    let cellMap = 
        generateCellMap random 20
    0
