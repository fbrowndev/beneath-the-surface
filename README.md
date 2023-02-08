# beneath-the-surface

## Chunk Loading Language

The files chunks are stored in are just .txt files; however, the are written in a human readible language.
The language has comments signified by two octothorps `#`. Tiles are defined with `defineTile` and mobs are defined by `defineMob`.

### defining tiles

Under the tile category is three ways to define a tile or group of tiles.

#### `tile`

One way is to define a single tile using `tile` as the next word in the definition. this is followed by the tile name in quotations.
Next the ID of the sprite and rotation of the sprite seperated by a colon `:`, the rotation is measured in quarter rotations.
Then finally the position of the tile on the grid.

This snippit of code makes a tile named "example_tile" with the tile id of 1 with no rotation on the tile grid at (0,0).
```
defineTile tile "example_tile" 0:0 (0,0)
```
#### `tileArray`

another way to define tiles is to define an are of tiles by using `tileArray` as the next word.
It has much simlar arguments as the single tile except it has two postion arguments.
The first one is for the top left corner of the area, the other is for the bottom right.

This snippt makes an area of tiles from (0,0) to (10,10) with the sprite of id 0 and no rotation named "example_array".
```
defineTile tileArray "example_array" 0:0 (0,0) (10,10)
```

#### `tileLine`

the final way to make tiles is in a line with `tileLine`, do note `tileLine` does take a rotational argument in the same way the sprite id does.
The arguments are like the `tileArray` except the last argument is a number for the length in tiles of the line.

This snippit make a line at a 45 degree angle named "ramp" with a length of 10 being centered on (28,14).
```
defineTile tileLine:.5 "ramp" 1:0 (28,14) 10
```

### defining mobs

there is currently only one way to define a mob.

#### `mob`

`mob` spawns a single mob or scripted object. the 3rd word is the name of the mob, not to be confused with id, the name can be whatever you want.
The next argument is the mob id. And the final argument is the initial position. 

This snippit defines a mob with an id of 0 named "example_mob" at (30,0).
```
defineMob mob "example_mob" 0 (30,0)
```
