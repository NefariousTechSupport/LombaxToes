# Shaders

They're quite simple currently, expect this to get worse overtime

* Texture tuids are stored in 0x00005A00
* Shader strings are stored in 0x00011300
* Shader strings toc is stored in 0x00005D00

## Textures

The tuids are stored in the texture tuids section, the tuid is only of the last 4 bytes though. The tuids are followed by the raw size of the texture and its mipmaps as a uint32, probably to help with memory allocation.

## Strings

* The string toc starts with the tuid of this shader, followed by the offset to the string in the file, there is then 4 bytes of padding to make sure it has alignment of 0x10.
* This is followed by the texture tuids, same order, still the last 4 bytes, no size this time, then padding for alignment.
* Next we have a tuid i think, not sure of what, followed by offsets to the texture names.