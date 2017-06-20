namespace Pdg.Common

type location = int * int

module Location =
    
    let add (first:location) (second:location) : location =
        ((first |> fst) + (second |> fst), (first |> snd) + (second |> snd))

type boardCell<'TTerrain,'TItem,'TCreature,'TEffect when 'TEffect:comparison> =
    {terrain:'TTerrain;
    items:'TItem list;
    creature: 'TCreature option;
    effects:Set<'TEffect>}

module BoardCell =
    
    let setTerrain (terrain) (boardCell) =
        {boardCell with terrain = terrain}

    let addEffect (effect) (boardCell) =
        {boardCell with effects = boardCell.effects |> Set.add effect}

    let removeEffect (effect) (boardCell) =
        {boardCell with effects = boardCell.effects |> Set.remove effect}

    let removeEffects (boardCell) =
        {boardCell with effects = Set.empty}

    
type board<'TTerrain,'TItem,'TCreature,'TEffect when 'TEffect:comparison> = 
    Map<location,boardCell<'TTerrain,'TItem,'TCreature,'TEffect>>

module Board =
    
    let setTerrain (location:location) (terrain:'TTerrain) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : board<'TTerrain,'TItem,'TCreature,'TEffect> =
        board
        |> Map.add location 
            (match board.TryFind location with
            | Some cell -> (cell |> BoardCell.setTerrain terrain)
            | None -> {terrain=terrain;items=[];creature=None;effects=Set.empty})

    let getTerrain (location:location) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : 'TTerrain option =
        match board |> Map.tryFind location with
        | None -> None
        | Some t -> t.terrain |> Some

    let addEffect (effect:'TEffect) (location:location) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : bool * board<'TTerrain,'TItem,'TCreature,'TEffect> =
        match board.TryFind location with
        | None -> (false, board)
        | Some cell -> (true, board |> Map.add location (cell |> BoardCell.addEffect effect))

    let removeEffect (effect:'TEffect) (location:location) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : bool * board<'TTerrain,'TItem,'TCreature,'TEffect> =
        match board.TryFind location with
        | None -> (false, board)
        | Some cell -> (true, board |> Map.add location (cell |> BoardCell.removeEffect effect))

    let removeEffects (location:location) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : bool * board<'TTerrain,'TItem,'TCreature,'TEffect> =
        match board.TryFind location with
        | None -> (false, board)
        | Some cell -> (true, board |> Map.add location (cell |> BoardCell.removeEffects))

    let hasEffect (effect:'TEffect) (location:location) (board:board<'TTerrain,'TItem,'TCreature,'TEffect>) : bool =
        match board.TryFind location with
        | None -> false
        | Some cell -> cell.effects.Contains effect
        
