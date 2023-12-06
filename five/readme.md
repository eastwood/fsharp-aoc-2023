seeds: 79 14 55 13

seed-to-soil map (82):
50 98 2
52 50 48

soil-to-fertilizer map (84)
0 15 37
37 52 2
39 0 15

fertilizer-to-water map (84)
49 53 8
0 11 42 
42 0 7
57 7 4

water-to-light map (84):
88 18 7
18 25 70 

light-to-temperature map (77):
45 77 23
81 45 19
68 64 13

temperature-to-humidity map (45):
0 69 1
1 0 69

humidity-to-location map: (46)
60 56 37
56 93 4

Location = 46

# The numbres against the map are the numbers working backwards, one solution is to iterate over the range of possible
# locations and calculate it back above

## Another solution is to transform entire ranges down the maps - haven't figured out this solution yet

Start:
(79,14), (55-13)

Seed->Soil Step:
----
a) 50 98 2
b) 52 50 48

(79,14) fits within b), new range is (81, 14)
(55,13) fits entirely within b), so new range is (57, 13)

=> Ranges are now (81,14), (57,13)

Soil->Fertizer Step:
----
a) 0 15 37
b) 37 52 2
c) 39 0 15

=> Both do not fit at all
=> Ranges are now (81,14), (57,13)

Fertilizer->Water Step:
----
a) 49 53 8 [53-60] <- [57-70] = [57-60] [61-70]
b) 0 11 42 
c) 42 0 7
d) 57 7 4

=> (81,14) => (81,14)
=> (57,13) => (53,4) (61,9)

water-to-light map:
----
a) 88 18 7
b) 18 25 70 // [25-94]

=> (53,4) => (47,4)
=> (61,9) => (54,9)
=> (81,14) => (74, 14)

light-to-temperature map:
----
a) 45 77 23 // [77-99]
b) 81 45 19
c) 68 64 13 // [64-77]

=> (47,4) => (83,4)
=> (54,9) => (90,9)
=> (74,14) | [74-88] => (74,3) (77,11) => (78,3)(45,11)

temperature-to-humidity map:
----
a) 0 69 1
b) 1 0 69

=> 

humidity-to-location map:
----
a) 60 56 37
b) 56 93 4

