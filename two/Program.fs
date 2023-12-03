#if INTERACTIVE
#load "../utils/strutil.fs"
#endif

open System
open System.IO
open System.Text.RegularExpressions
open Utils

type Score = (int * int * int)

let lines = File.ReadAllLines "input.txt"
let (RED, GREEN, BLUE) = (12, 13, 14)

let createGame (acc: Score) (curr: Match) =
    let value = Int32.Parse(curr.Groups.[1].Value)
    let (red, green, blue) = acc
    match curr.Groups[2].Value with
    | "red" -> (red + value, green, blue)
    | "green" -> (red, green + value, blue)
    | "blue" -> (red, green, blue + value)
    | _ -> (red, green, blue)

let extractNumbers (input: string): Score =
    let regex = Regex("(\\d+)\\s*(red|green|blue)")
    let matches = regex.Matches(input)
    matches |> Seq.fold createGame (0, 0, 0)

let splitIntoGames (line: string) =
    let preample = line.IndexOf(":") + 2
    let game = line[preample..]
    game.Split("; ") |> Seq.map extractNumbers

let isPossible games =
    games
    |> Seq.tryFind ((fun (r, g, b) -> r > RED || g > GREEN || b > BLUE))
    |> Option.isNone

let sumIndices bools =
    let mutable sum = 0
    for i, b in Seq.indexed bools do
        if (b) then
            sum <- sum + i + 1
    sum

let one =
    lines
    |> Seq.map splitIntoGames
    |> Seq.map isPossible
    |> sumIndices

printfn "Part A: %A" one

let findSmallestTuple acc curr : Score =
    let (aR, aG, aB) = acc
    let (bR, bG, bB) = curr
    (max aR bR, max aG bG, max aB bB)

let calculateMinimum games =
    let initialState: Score = (0, 0, 0)
    Seq.fold findSmallestTuple initialState games

let calculateScore (game: Score) =
    let (red, green, blue) = game
    normalise red * normalise green * normalise blue

let two =
    lines
    |> Seq.map splitIntoGames
    |> Seq.map calculateMinimum
    |> Seq.map calculateScore
    |> Seq.sum

printfn "Part B: %A" two
