# gp_prius

It has a hybrid drivetrain, combining an internal combustion engine with an electric motor. Initially offered as a four-door sedan, it has been produced only as a five-door liftback since 2003.

* The instance names are at section id 0x0002504C
* The instance are at section id 0x00025048
* The volume names are at section id 0x00025060
* The volumes are at section id 0x0002505C

## Instances

The instances are 0x50 bytes long and consist of:

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  u16 | moby tuid index in region.dat
|  0002  |  u16 | moby group, aka the first parameter in moby_handle
|  0014  |  f32 | x position
|  0018  |  f32 | y position
|  001C  |  f32 | z position
|  0020  |  f32 | x rotation as ZYX euler
|  0024  |  f32 | y rotation as ZYX euler
|  0028  |  f32 | z rotation as ZYX euler
|  002C  |  f32 | scale

The instance names are stored in a structure similar to the IGHW pointers, with the main difference being that the length field is the group

## Volumes

The volumes are 0x40 bytes long and consist of:

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  | mat4 | that's it lol

Volumes are cubes, the mat4 is its transformation

The volume names are stored in a structure similar to the IGHW pointers, with the main difference being that the length field is something else lol