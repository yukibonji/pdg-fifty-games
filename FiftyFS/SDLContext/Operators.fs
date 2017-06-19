module SDLContext.Operators

let (=>^) (first:'TFirst,second:'TSecond) (f:'TSecond->'TSecond) : 'TFirst * 'TSecond =
    (first, second |> f)

let (=^>) (first:'TFirst,second:'TSecond)  (f:'TFirst->'TFirst): 'TFirst * 'TSecond =
    (first |> f, second)


