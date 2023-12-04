open System.IO
open System.Text.RegularExpressions
open System

let lines = File.ReadAllLines "input.txt"

type PartNumber = (string * int * int * int)
type Symbol = (string * int * int)

let locatePartNumbers (linum: int) (line: string): seq<PartNumber> =
    let regex = Regex("\d+")
    let matches = regex.Matches(line)
    matches |> Seq.map(fun m -> (m.Value, m.Value.Length, m.Index, linum))

let locateSymbols (linum: int) (line: string): seq<Symbol> =
    let regex = Regex("[^\.\d]")
    let matches = regex.Matches(line)
    matches |> Seq.map(fun m -> (m.Value, m.Index, linum))

let partNumbers = lines |> Seq.mapi locatePartNumbers |> Seq.concat
let symbols = lines |> Seq.mapi locateSymbols |> Seq.concat

// A much faster solution would be to use 2D array and use the log10 of the array length
// to do this check properly, accounting for overflows egtc
let isSymbolAdjacent (part: PartNumber) (symbol: Symbol): bool =
    let (_, _, pX, pY) = part
    let (_, sX, sY) = symbol

    let adjacentX = (sX = pX || sX = pX - 1 || sX = pX + 1)
    adjacentX && (sY = pY - 1 || sY = pY || sY = pY + 1)


let hasAdjacentSymbol (part: PartNumber) =
    let (value, len, x, y) = part

    let matchingFunc newX =
        let newPart = (value, len, newX, y)
        symbols |> Seq.tryFind (isSymbolAdjacent newPart) |> Option.isSome

    [x..x+len - 1] |> Seq.tryFind matchingFunc |> Option.isSome

let matchingParts =
    partNumbers
    |> Seq.filter hasAdjacentSymbol

let sum =
    matchingParts
    |> Seq.map(fun p ->
       let (value, _, _, _) = p
       Int32.Parse(value))
    |> Seq.sum

printfn "Part A: %A" sum

let locateGears (linum: int) (line: string): seq<Symbol> =
    let regex = Regex("\*")
    let matches = regex.Matches(line)
    matches |> Seq.map(fun m -> (m.Value, m.Index, linum))

let gears = lines |> Seq.mapi locateGears |> Seq.concat

let hasAdjacentNumber (gear: Symbol) (part: PartNumber) =
    let (value, len, x, y) = part
    [x..x+len-1] |> Seq.tryFind(fun newX ->
        let newPart = (value, len, newX, y)
        isSymbolAdjacent newPart gear)
    |> Option.isSome

let getTwoAdjacentNumbers (gear: Symbol) =
    let number = partNumbers |> Seq.filter (hasAdjacentNumber gear)
    let matches = Seq.length number
    if matches = 2 then
        number
        |> Seq.fold (fun acc curr ->
            let (value, _, _, _) = curr
            acc * Int32.Parse(value)) 1
    else
      0

let gearSum =
    gears
    |> Seq.map getTwoAdjacentNumbers
    |> Seq.sum

printfn "Part B: %A" gearSum