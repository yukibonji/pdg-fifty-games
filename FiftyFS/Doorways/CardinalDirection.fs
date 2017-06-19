module CardinalDirection

type direction =
    | North
    | East
    | South
    | West

let next = function
    | North -> East
    | East -> South
    | South -> West
    | West -> North
    
let previous = function
    | North -> West
    | East -> North
    | South -> East
    | West -> South

let opposite = function
    | North -> South
    | East -> West
    | South -> North
    | West -> East

let step (steps:int) (direction:direction) (x:int,y:int) : int * int =
    match direction with
    | North -> (x,y-steps) 
    | East ->  (x+steps,y) 
    | South ->  (x,y+steps) 
    | West -> (x-steps,y) 
        
let directions = [North;East;South;West]

