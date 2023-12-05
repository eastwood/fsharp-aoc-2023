open System.IO
open System.Text.RegularExpressions
open System

let lines = File.ReadAllLines "input.txt"

let split (separators: string) (x: string) = x.Split(separators)

let stripPrefix (line: string) =
    let regex = Regex("Card .*: (.*)")
    let matches = regex.Matches(line)

    matches
    |> Seq.map (fun m -> m.Groups[1].Value.Trim())
    |> String.concat ""

type Card = {
    Theirs: Set<int>
    Mine: Set<int>
}

let convertToGame (card: string) =
    let cards = card.Split("|")
    let l = cards[0].Trim()
    let r = cards[1].Trim()

    let regex = Regex("(\d{1,2})+")
    let theirs = regex.Matches(l) |> Seq.map(fun m -> Int32.Parse(m.Value)) |> Set.ofSeq
    let ours = regex.Matches(r) |> Seq.map(fun m -> Int32.Parse(m.Value)) |> Set.ofSeq

    let out = {
        Theirs = theirs;
        Mine = ours
    }
    out

let calculateScore (card: Card) =
    let winners = (Set.intersect card.Mine card.Theirs |> Seq.length) - 1
    if winners >= 0 then Math.Pow(2, winners) else 0

let numbers =
    lines
    |> Seq.map stripPrefix
    |> Seq.map convertToGame
    |> Seq.map calculateScore
    |> Seq.sum

printfn "%A" numbers
