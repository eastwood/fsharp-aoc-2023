module Utils

let split (separators: string) (x: string) = x.Split(separators)

let normalise x = if x = 0 then 1 else x
