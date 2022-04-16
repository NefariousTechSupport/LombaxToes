# AssetLookup

The assetlookup is essentially a dictionary for all other asset files (mobys, textures, shaders, etc). It consists almost entirely of [IGHW pointers](./IGHW.md#ighw-pointers)

## Sections

| Section Identifier | Description
|--------------------|-------------------------------------------
|      0001D100      | Shaders
|      0001D101      | [Shader Ones](#why-the-ones)
|      0001D140      | [Highmip Metadata](./textures.md)
|      0001D180      | [Textures](./textures.md)
|      0001D181      | [Texture Ones](#why-the-ones)
|      0001D1C0      | [Highmips](./textures.md)
|      0001D200      | Cubemaps
|      0001D201      | [Cubemap Ones](#why-the-ones)
|      0001D300      | Ties
|      0001D301      | [Tie Ones](#why-the-ones)
|      0001D600      | Mobys
|      0001D601      | [Moby Ones](#why-the-ones)
|      0001D700      | Animsets
|      0001D701      | [Animset Ones](#why-the-ones)
|      0001DA00      | Zones
|      0001DA01      | [Zone Ones](#why-the-ones)
|      0001DB00      | Lighting
|      0001DB01      | [Lighting Ones](#why-the-ones)


## Why The Ones?

I don't know