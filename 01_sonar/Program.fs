module sonar

open System
open System.IO

let parselines lines = lines |> Array.map(fun state -> Int64.Parse(state))

let findIncrements (intLines: Int64[]) =
    // convert line to true if greater than prev
    let inc = intLines |> Array.mapi(fun i state -> if i < 1 then false else state > intLines[i-1])
    
    // count greater lines by filtering them
    let counts = inc |> Array.filter(fun state -> state)

    // implicit return
    counts.Length 

let groupLines intLines = 
    intLines |> Array.mapi(fun i item -> if i+2 >= intLines.Length then 0L else (intLines[i] + intLines[i+1] + intLines[i+2]))
    

let simpleIncrease lines = 
    lines
    |> parselines
    |> findIncrements

let groupedIncrease (lines: string[])  = 
    lines
    |> parselines
    |> groupLines
    |> findIncrements

[<EntryPoint>]
let main args = 
    let dir = Path.Combine(Environment.CurrentDirectory, args[0])
    let lines = File.ReadAllLines(dir)

    simpleIncrease  lines |> printfn "Simple  : %d"
    groupedIncrease lines |> printfn "Grouped : %d"

    // exit code
    0