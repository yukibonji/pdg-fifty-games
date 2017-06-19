
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
                let cell = 
                    pickRandomCellLocation random cellMap
                let neighborCells = 
                    CardinalDirection.directions
                    |> List.map (fun d -> CardinalDirection.step 1 d cell.Value)
                    |> List.filter (fun p -> Map.containsKey p cellMap |> not)
                cellMap

    Map.empty
    |> generateCells random cellCount


[<EntryPoint>]
let main argv = 
    let random = new System.Random()
    let cellMap = 
        generateCellMap random 500
    0
