# Zone

This contains the locations of all the ties.

* The tie tuids are at section id 0x00007200
* The tie instances are at section id 0x00007240
* The tie instance names are at section id 0x000072C0

## Tie Instances

The tie instances are 0x80 bytes long and consist of:

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  | mat4 | transformation
|  0040  |  f32 | bounding sphere x position
|  0044  |  f32 | bounding sphere y position
|  0048  |  f32 | bounding sphere z position
|  004C  |  f32 | bounding sphere radius
|  0050  |  f32 | index of the tie in the tie tuids section

The tie instance names are stored in a structure similar to the IGHW pointers, with the main difference being that the length field is something else lol