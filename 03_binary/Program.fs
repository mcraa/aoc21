open System
open System.IO

let nthPlaceCommonBit (n: int, lines: string[]) = 
    let sum = 
        lines 
        |> Array.fold (fun acc item -> acc + Int32.Parse( item.Substring(n,1) ) ) 0

    if (sum*2) > lines.Length then
        1L
    else 
        0L 

let getCommonBits (length: int, lines: string[]) =
    Array.zeroCreate length
    |> Array.mapi (fun i item -> nthPlaceCommonBit(i, lines))    

let invertBinaryArray (bits: int64[]) = 
    bits
    |> Array.map (fun item -> (item * -1L) + 1L)

let getLength (lines: string[]) =
    lines[0].Length, lines

let bitArrayToInt (arr: int64[]) =
    arr
    |> Array.map (fun item -> Convert.ToString(item) )
    |> Array.map (fun item -> item.Trim())
    |> String.concat ""
    |> fun s -> Convert.ToInt64(s, 2)

[<EntryPoint>]
let main args = 
    let dir = Path.Combine(Environment.CurrentDirectory, args[0])
    let lines = File.ReadAllLines(dir) 

    let gammaBits = 
        lines
        |> getLength
        |> getCommonBits

    let epsilonBits = invertBinaryArray gammaBits

    let gamma = bitArrayToInt gammaBits
    let epsilon = bitArrayToInt epsilonBits
    
    
    printfn "G: %d, E: %d" gamma epsilon 
    printfn "Power: %d" (gamma*epsilon) 
    
    // exit code
    0