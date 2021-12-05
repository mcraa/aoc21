open System
open System.IO

let nthPlaceCommonBit (n: int, lines: string[]) = 
    let sum = 
        lines 
        |> Array.fold (fun acc item -> acc + Int32.Parse( item.Substring(n,1) ) ) 0

    if (sum*2) >= lines.Length then
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

let ToCharAndTrim (bit: int64) =
    bit
    |> Convert.ToString
    |> fun s -> s.Trim()
    |> Convert.ToChar

let reduceLinesByNthCommonBit (commonBitPlace: int, lines: string[]) =
    let commonBit = 
        nthPlaceCommonBit(commonBitPlace, lines)
        |> ToCharAndTrim

    lines 
    |> Array.filter(fun item -> commonBit.Equals(item[commonBitPlace]) )
    
let reduceLinesByNthUncommonBit (commonBitPlace: int, lines: string[]) =
    let commonBit = 
        nthPlaceCommonBit(commonBitPlace, lines)
        |> (fun item -> (item * -1L) + 1L) // invert
        |> ToCharAndTrim

    lines 
    |> Array.filter(fun item -> commonBit.Equals(item[commonBitPlace]) )

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

    //-------------------------------
    
    let mutable oxylines = lines
    let mutable index = 0
    while oxylines.Length > 1 do 
        oxylines <- reduceLinesByNthCommonBit(index, oxylines)
        index <- index+1

    let mutable co2lines = lines
    index <- 0
    while co2lines.Length > 1 do
        co2lines <- reduceLinesByNthUncommonBit(index, co2lines)
        index <- index+1

    let oxygen = Convert.ToInt64(oxylines[0], 2)
    let co2 = Convert.ToInt64(co2lines[0], 2)
    printfn "Oxygen: %d, Co2: %d" oxygen co2
    printfn "Power: %d" (oxygen*co2)

    // exit code
    0