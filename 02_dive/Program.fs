open System
open System.IO

let parselines (lines: string[]) = 
    let directions = lines |> Array.map(fun state -> state.Split ' ' |> Array.item(0))
    let distances = lines |> Array.map(fun state -> state.Split ' ' |> Array.item(1) |> Int64.Parse) 

    directions, distances

let addMoves (directions:string[], distances: int64[]) = 
    let x = directions |> Array.mapi(fun i item -> if item.Equals("forward") then distances[i] else 0L) |> Array.sum
    let yPos = directions |> Array.mapi(fun i item -> if item.Equals("down") then distances[i] else 0L) |> Array.sum
    let yNeg = directions |> Array.mapi(fun i item -> if item.Equals("up") then -distances[i] else 0L) |> Array.sum

    x, yPos + yNeg

let diveMoves (directions: string[], distances: int64[]) =
    let signed = distances |> Array.mapi(fun i item -> if directions[i].Equals("up") then -item else if directions[i].Equals("forward") then 0L else item)
    let aims = Array.scan (fun ac item -> ac + item) 0L signed
    let x = 
        distances 
        |> Array.mapi (fun i item -> if directions[i].Equals("forward") then item else 0L) 
        |> Array.sum

    let depth = 
        distances
        |> Array.mapi (fun i item -> if directions[i].Equals("forward") then aims[i]*item else 0L)
        |> Array.sum
    
    x, depth

[<EntryPoint>]
let main args = 
    let dir = Path.Combine(Environment.CurrentDirectory, args[0])
    let lines = File.ReadAllLines(dir) 
    
    let x, y = 
        lines
        |> parselines
        |> addMoves

    let x2, y2 =
        lines
        |> parselines
        |> diveMoves

    let mult = x*y
    let mult2 = x2*y2
    printfn "Simple       X: %d, Y: %d => %d" x y mult
    printfn "Corrected    X: %d, Y: %d => %d" x2 y2 mult2

    // exit code
    0