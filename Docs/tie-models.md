# Tie Models

THIS DOCUMENTATION IS LARGELY BASED OFF OF THE FINDINGS OF Gh0stblade

* The metadata section has id 0x00003300
* The indices section has id 0x00003200
* The shaders section has id 0x00005600
* The vertices section has id 0x00003000
* The name section has id 0x00003410
* The misc section has id 0x00003400

## Metadata

Mobys consists of an array of metadata, 0x40, which contains most of the info needed to read them. Any unaccounted for bytes serve an unknown purpose.

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  u32 | index of the starting index. start of the index buffer = indexSectionStart + this * 2 
|  0004  |  u32 | offset of vertices in the vertex section. start of the vertex buffer = vertexSectionStart + this 
|  0008  |  u16 | index of shader tuid for this mesh in the #shaders section
|  000A  |  u16 | vertex count
|  000C  | byte | bone map index count
|  000D  | byte | specifies whether or not this mesh has bones
|  000E  | byte | bone map index
|  0012  |  u16 | index count
|  0020  |  u32 | bone map offset

## Vertex Block Format

all meshes in a tie have vertex blocks of length, and hence stride, of 0x14. Note that there's stuff missing

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  s16 | x position of vertex
|  0002  |  s16 | y position of vertex
|  0004  |  s16 | z position of vertex
|  0008  |  f16 | u uv
|  000A  |  f16 | v uv

## Shaders

The shaders section of a tie consists of an array of tuids

## Tie Mesh Name

The name of a tie is stored in the tie path section

## Mesh Scaling

When reading vertices it's advasable you divide them by 0x7FFF to basically normalise it then multiply it by the mesh scale, stored 0x70 bytes into the misc section, as 3 floats, one for each axis: x, y, and z.
