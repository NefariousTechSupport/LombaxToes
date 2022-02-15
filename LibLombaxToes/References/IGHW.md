This is the main file format used by the games.
This file consists of several sections that contain different bits of data

## IGHW Header

addr | size | desc
-----|------|-------------------------------------
0000 | 0004 | Magic number (0x49474857 or "IGHW")
0004 | 0002 | Major version ??
0006 | 0002 | Minor version ??
0008 | 0004 | Number of [sections](#ighw-section-header)
000C | 0004 | Length of header
0010 | 0004 | Length of file
0014 | 0004 | Padding (zeros)
0018 | 0008 | Padding (0xDEAD repeated)

## IGHW Section Header

addr | size | desc
-----|------|-------------------------
0000 | 0004 | [Section identifier](#ighw-section-identifiers)
0004 | 0004 | Offset in current file
0008 | 0004 | number of elements if or'd by 0x10000000
000C | 0004 | length of single element if ^that^ is or'd 0x10000000, else length in current file

## IGHW Section Identifiers

 Identifier | Description
------------|-------------------------------------------------------
  0001D000  | ?? (Is normally the number 6)
  0001D100  | Shader [Pointer](#ighw-pointer) Array\**
  0001D101  | Shader Ones\*
  0001D140  | [Highmip metadata](#ighw-texture-metadata) array
  0001D180  | Texture [Pointer](#ighw-pointer) Array\**
  0001D181  | Texture Ones\*
  0001D1C0  | Highmip [Pointer](#ighw-pointer) Array\**
  0001D200  | Cubemap [Pointer](#ighw-pointer) Array\**
  0001D201  | Cubemap Ones\*
  0001D300  | Tie [Pointer](#ighw-pointer) Array\**
  0001D301  | Tie Ones\*
  0001D600  | Moby [Pointer](#ighw-pointer) Array\**
  0001D601  | Moby Ones\*
  0001D700  | Animset [Pointer](#ighw-pointer) Array\**
  0001D701  | Animset Ones\*
  0001DA00  | Zone [Pointer](#ighw-pointer) Array\**
  0001DA01  | Zone Ones\*
  0001DB00  | Lighting [Pointer](#ighw-pointer) Array\**
  0001DB01  | Lighting Ones\*
  0001CF00  | Some names for textures I think
  000081C0  | points to an ighw file when in regions.dat
  000081E0  | unknown, mostly -1 repeated a lot, sometimes 0
  00008B00  | unknown, floats
  00008120  | zeroes
  00008100  | unknown, more floats
  00008150  | unknown, floats
  00008190  | unknown, shorts

\* If the identifier ends in 0x1 then it's a list of 1s, the number of items in this list is the same as the number of items in the related section <br>
\** Only applies when in assetlookup.dat, otherwise it's a list of references

## IGHW Pointer

addr | size | desc
-----|------|---------------------------
0000 | 0008 | [Section identifier](#ighw-section-identifiers)
0008 | 0004 | Offset in respective file
000C | 0004 | Length in respective file

## IGHW Texture Metadata

addr | size | desc
-----|------|-------------------
0000 | 0001 | Format ID
0001 | 0001 | Number of Mipmaps
0002 | 0001 | Power of Width
0003 | 0001 | Power of Height

you calculate width and height as so:

```cs
width = Math.Pow(2, powWidth);
height = Math.Pow(2, powHeight);
```
or
```cs
width = 1 << powWidth;
height = 1 << powHeight;
```

Format IDs are:

 ID | Description
----|---------------------------------------
 03 | ????
 04 | ????
 05 | Seems like it's R8G8B8A8 but not sure
 06 | DXT1
 08 | DXT5
 0B | ???? (16 bits per pixel)

 Also fun fact, the highest res versions if the textures are stored in highmips.dat, the mipmaps for when further away are stored in textures.dat