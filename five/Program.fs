open System.IO
open System

let split (separators: string) (x: string) = x.Split(separators)

let lines = File.ReadAllLines "input.txt"

let sections =
    lines
    |> String.concat("|")
    |> split "||"

let seeds = sections[0][7..] |> split " " |> Seq.map(fun f -> Int64.Parse(f))

type Mapping = (int64 * int64 * int64)
  
let createMap (line: string): Mapping =
    let numbers = line.Split(" ")
    (Int64.Parse(numbers[1]), Int64.Parse(numbers[0]), Int64.Parse(numbers[2]))

let createMappings (line: string) =
    line.Split("|")[1..]
    |> Seq.map createMap

let maps = sections[1..] |> Seq.map createMappings |> Seq.toArray

let seedToSoil = maps[0]
let soilToFertilizer = maps[1]
let fertilizerToWater = maps[2]
let waterToLight = maps[3]
let lightToTemperature = maps[4]
let temperatureToHumidity = maps[5]
let humidityToLocation = maps[6]

// Given a number and a map, look up the seed inside of the map to figure out
// the destination
let findInMap (number: int64) (mappings: seq<Mapping>) =
    let findFunc map =
        let (src, _, range) = map
        number >= src && number < src + range
        
    let found =
        mappings
        |> Seq.tryFind findFunc

    if Option.isSome found then
        let (src, dst, _) = found.Value
        dst - src + number
    else
        number

let convertSeedToLocation (seed: int64) =
    let soil = findInMap seed seedToSoil
    let fert = findInMap soil soilToFertilizer
    let water = findInMap fert fertilizerToWater
    let light = findInMap water waterToLight
    let temp = findInMap light lightToTemperature
    let hum = findInMap temp temperatureToHumidity
    let loc = findInMap hum humidityToLocation
    loc
    
let one = seeds |> Seq.map convertSeedToLocation |> Seq.min
printfn "Part A: %A" one

let ranges = seeds |> Seq.chunkBySize 2 |> Seq.map(fun arr -> (arr[0], arr[1]))

type Range = (int64 * int64)
let calculateRange (range: Range): seq<int64> =
    let (start, range) = range
    let mutable locations = Seq.empty
    for i in 0L..range do
        let seed = start + i
        let location = convertSeedToLocation seed
        locations <- Seq.append locations [location]

    locations
        
let two = ranges |> Seq.map calculateRange |> Seq.concat |> Seq.min
printfn "Part B: %A" two
