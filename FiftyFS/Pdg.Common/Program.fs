
type TerrainType =
    | Grass
    | Tree
    | Stone
    | Flint
    | BerryBush

type TreeDescriptor =
    {wood:int}

type StoneDescriptor =
    {stone:int}

type FlintDescriptor =
    {flint:int}

type BerryBushType =
    | Crimson
    | Copper
    | Gold
    | Jade
    | Turqoise
    | Ruby
    | Amethyst
    | Silver

type BerryBushDescriptor =
    {berryBushType:BerryBushType;
    berries:int}

type Terrain =
    | Grass
    | Tree of TreeDescriptor
    | Stone of StoneDescriptor
    | Flint of FlintDescriptor
    | BerryBush of BerryBushDescriptor

let terrainTypeGenerator =
    [(TerrainType.Grass,100);
    (TerrainType.Tree,25);
    (TerrainType.Stone,10);
    (TerrainType.Flint,5)]
    |> Map.ofList

let terrainGenerators =
    [(TerrainType.Grass,fun (random:System.Random) -> Terrain.Grass);
    (TerrainType.Tree,fun (random:System.Random) -> Terrain.Tree {wood=10});
    (TerrainType.Stone,fun (random:System.Random) -> Terrain.Stone {stone=10});
    (TerrainType.Flint,fun (random:System.Random) -> Terrain.Flint {flint=10})]
    |> Map.ofList


[<EntryPoint>]
let main argv = 
    
    0
