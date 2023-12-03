module One
open System
open System.IO

let lines = File.ReadAllLines "input.txt"

let calculate (line: string) =
  let digits = line |> Seq.filter Char.IsDigit |> Seq.toArray
  sprintf "%c%c" digits[0] digits[digits.Length - 1] |> int32

let one =
  lines |> Seq.map calculate |> Seq.sum

printfn "Part 1: %A" one

let wordToNumberMap = dict [ "one", "1"; "two", "2"; "three", "3"; "four", "4"; "five", "5"; "six", "6"; "seven", "7"; "eight", "8"; "nine", "9"; ]

let rec convertWordsToNumbers (input: string) =
  let possible_match = wordToNumberMap |> Seq.tryFind((fun (word) -> input.StartsWith(word.Key)))
  match possible_match with
  | Some (pair) -> pair.Value + convertWordsToNumbers(" " + input.Substring(pair.Key.Length - 1))
  | None ->
      if String.IsNullOrEmpty(input) then ""
      else input[0].ToString() + convertWordsToNumbers (input.Substring(1))

let two =
  lines
    |> Seq.map convertWordsToNumbers
    |> Seq.map calculate
    |> Seq.sum

printfn "Part 2: %A" two
