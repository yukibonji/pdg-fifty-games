namespace Pdg.Common

type generator<'TItem> when 'TItem : comparison = Map<'TItem,int>

module Generator=

    type randomizer = int -> int

    let generate (randomizer:randomizer) (generator:generator<'TItem>) : 'TItem option=
        (((0,generator) ||> Map.fold (fun acc _ v -> acc + v) |> randomizer, None), generator)
        ||> Map.fold (fun acc k v -> 
            match acc with
            | (_,Some x) -> acc
            | (n, None) -> 
                if n <= v then
                    (0, Some k)
                else
                    (n - v, None))
        |> snd

    type aggregator<'TItem> = 'TItem -> 'TItem -> 'TItem

    let aggregate (aggregator:aggregator<'TItem>) (firstGenerator:generator<'TItem>) (secondGenerator:generator<'TItem>) : generator<'TItem> =
        (Map.empty, firstGenerator)
        ||> Map.fold (fun acc1 k1 v1 -> 
            (acc1, secondGenerator)
            ||> Map.fold (fun acc2 k2 v2->
                let k = aggregator k1 k2
                let v = v1 * v2
                if acc2.ContainsKey k then
                    acc2
                    |> Map.add k (v+acc2.[k])
                else
                    acc2
                    |> Map.add k v))

    let ofList<'TItem when 'TItem:comparison> (items:'TItem list) : generator<'TItem> =
        items
        |> List.map (fun x -> (x,1))
        |> Map.ofList