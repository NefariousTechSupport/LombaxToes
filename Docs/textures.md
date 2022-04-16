# Textures

First off we should clear up what highmips are and why they're important. Highmips are higher resolutions versions of the textures that the game uses when you're up close, look up mipmapping if you're confused

# Old Engine

ha i wish

# New Engine

in the highmips metadata section, the metadata for each texture is 4 bytes long. byte 0 is the format, see below:

| Format | Description
|--------|-------------------------------------
|  0005  | a8b8g8r8 but swizzled, we don't know how to unswizzle
|  0006  | DXT1
|  0008  | DXT5
|  000B  | Maybe r5g6b5 but byte swapped + swizzled, we don't know how to unswizzle