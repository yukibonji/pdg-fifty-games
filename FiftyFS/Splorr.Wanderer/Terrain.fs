namespace Splorr.Wanderer

open Pdg.Common

type terrainType =
    | Grass
    | Tree
    | Rock
    | FlintRock
    | BerryBush

type treeDescriptor =
    {wood:int}

type rockDescriptor =
    {stone:int}

type flintRockDescriptor =
    {flint:int}

type berryType =
    | Crimson
    | Copper
    | Gold
    | Jade
    | Turqoise
    | Ruby
    | Amethyst
    | Silver

type berryBushDescriptor =
    {berryType:berryType;
    berries:int}

type terrain =
    | Grass
    | Tree of treeDescriptor
    | Rock of rockDescriptor
    | FlintRock of flintRockDescriptor
    | BerryBush of berryBushDescriptor

module Terrain =
    let toTerrainType =
        function
        | terrain.Grass       -> terrainType.Grass
        | terrain.Tree      _ -> terrainType.Tree
        | terrain.Rock      _ -> terrainType.Rock
        | terrain.FlintRock _ -> terrainType.FlintRock
        | terrain.BerryBush _ -> terrainType.BerryBush

    let create (berryTypeGenerator:generator<berryType>) (randomizer: int -> int) =
        function
        | terrainType.Grass     -> terrain.Grass
        | terrainType.Tree      -> terrain.Tree      {wood    = 10}
        | terrainType.Rock      -> terrain.Rock      {stone   = 10}
        | terrainType.FlintRock -> terrain.FlintRock {flint   = 10}
        | terrainType.BerryBush -> terrain.BerryBush {berries = 10; berryType=berryTypeGenerator |> Generator.generate randomizer |> Option.get}
        