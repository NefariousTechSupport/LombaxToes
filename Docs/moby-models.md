# Moby Models

THIS DOCUMENTATION IS LARGELY BASED OFF OF THE FINDINGS OF Gh0stblade

* The metadata section has id 0x0000DD00
* The indices section has id 0x0000E100
* The shaders section has id 0x00005600
* The vertices section has id 0x0000E200
* The name section has id 0x0000D200
* The misc section has id 0x0000D100

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

if the mesh in question lacks bones, the length of this vertex block and hence the vertex stride is 0x14. Note that there's stuff missing.

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  s16 | x position of vertex
|  0002  |  s16 | y position of vertex
|  0004  |  s16 | z position of vertex
|  0008  |  f16 | u uv
|  000A  |  f16 | v uv

if the mesh in question has bones, the length of this vertex block and hence the vertex stride is 0x1C. Note that there's stuff missing.

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  s16 | x position of vertex
|  0002  |  s16 | y position of vertex
|  0004  |  s16 | z position of vertex
|  0008  | byte | bone index
|  000C  |  f16 | bone weight
|  001C  |  f16 | u uv
|  001E  |  f16 | v uv

## Wait How Are Bones Actually Stored?
I don't know lol, check Gh0stBlade's noesis plugin

## Shaders

The shaders section of a moby consists of an array of tuids

## Moby Mesh Name

The name of a moby is stored in the moby path section

## Moby Bounding Sphere

The bounding sphere is stored as 4 floats: x, y, z, and radius at the start of the misc section

## Mesh Scaling

When reading vertices it's advasable you divide them by 0x7FFF to basically normalise it then multiply it by the mesh scale, stored 0x70 bytes into the misc section as a single float for all axis
