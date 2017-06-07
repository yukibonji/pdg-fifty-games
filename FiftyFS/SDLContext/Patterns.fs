module SDLContext.Patterns

type pattern = 
    byte list

let private getStatus (x:int, y:int) (pat:pattern) : bool =
    if pat.Length <= y then 
        false
    else
        pat
        |> List.skip y
        |> List.head
        |> (&&&) ((1 <<< x) |> byte)
        |> (<>) 0uy


let internal makePattern (data: string list) : pattern =
    let makeLine (line:string) : byte =
        line.ToCharArray()
        |> Array.fold(
            fun (acc,flag) c -> ((if c='X' then acc + flag else acc), flag <<< 1)) (0uy,1uy)
        |> fst

    data
    |> List.map makeLine

type color = byte * byte * byte * byte

let render (x:int,y:int) (foreground: color option, background: color option) (pattern:pattern) (context:SDLContext.context) : unit =
    [0..((pattern |> List.length) - 1)]
    |> List.iter (fun cellY -> 
        [0..7]
        |>List.iter (fun cellX -> 
            if pattern |> getStatus(cellX,cellY) then
                foreground
                |> Option.iter(fun c -> 
                    SDLContext.setDrawColor c context
                    SDLContext.fillRect (x+cellX,y+cellY,1,1) context)
            else    
                background
                |> Option.iter(fun c -> 
                    SDLContext.setDrawColor c context
                    SDLContext.fillRect (x+cellX,y+cellY,1,1) context)))
    ()