namespace Pdg.Common

type location = int * int

module Location =
    
    let add (first:location) (second:location) : location =
        ((first |> fst) + (second |> fst), (first |> snd) + (second |> snd))

type board<'TItem> = Map<location,'TItem>


    

