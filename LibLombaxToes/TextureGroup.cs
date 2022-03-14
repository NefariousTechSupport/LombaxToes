namespace LibLombaxToes
{
	public class TextureGroup
	{
		Stream texturesStream;
		Stream highmipsStream;

		Texture[] textures;

		public TextureGroup(Stream texStream, Stream? hmStream, AssetLookup parent)
		{
			texturesStream = texStream;
			highmipsStream = hmStream;

			IGHWSectionHeader texturesSection = parent.GetSectionHeader(IGHWSectionIdentifier.Textures);
			IGHWSectionHeader highmipsSection = parent.GetSectionHeader(IGHWSectionIdentifier.Highmips);
			IGHWSectionHeader metadataSection = parent.GetSectionHeader(IGHWSectionIdentifier.HighmipMetadata);

			textures = new Texture[texturesSection.itemLength / 0x10];

			for(int i = 0; i < textures.Length; i++)
			{
				parent.sh.BaseStream.Seek(texturesSection.offset + i*0x10, SeekOrigin.Begin);
				textures[i].texturesPointer = parent.sh.ReadStruct<IGHWPointer>();
				if(highmipsStream != null)
				{
					parent.sh.BaseStream.Seek(highmipsSection.offset + i*0x10, SeekOrigin.Begin);
					textures[i].highmipsPointer = parent.sh.ReadStruct<IGHWPointer>();
				}
				parent.sh.BaseStream.Seek(metadataSection.offset + i*0x04, SeekOrigin.Begin);
				byte format = parent.sh.ReadByte();
				if(format == 0x06)      textures[i].format = TextureFormat.DXT1;
				else if(format == 0x08) textures[i].format = TextureFormat.DXT5;
				else textures[i].format = TextureFormat.Unknown;
				textures[i].mipmapCount = parent.sh.ReadByte();
				textures[i].width = 1 << parent.sh.ReadByte();
				textures[i].height = 1 << parent.sh.ReadByte();
			}
		}
		public int FindTexture(ulong tuid)
		{
			return Array.FindIndex<Texture>(textures, x => (x.texturesPointer.tuid & 0xFFFFFFFF) == (tuid & 0xFFFFFFFF));	//Usually texture refs only store the last 4 bytes so we only check those last 4 bytes
		}
		//Rewrite to support mipmaps and highmips
		public byte[] RipTexture(ulong tuid, bool ripMipmaps, out TextureFormat format, out int width, out int height, out int mipmapCount)
		{
			int index = FindTexture(tuid);
			if(index < 0)
			{
				format = TextureFormat.Unknown;
				width = 0;
				height = 0;
				mipmapCount = 0;
				return null;
			}

			//if(ripMipmaps) throw new NotImplementedException("Mipmaps aren't supported");
			if(highmipsStream == null) throw new NotImplementedException("Highmips must be assigned");

			uint rippedBytes = 0;
			uint finalSize = CalculateTextureSize(tuid, ripMipmaps ? -1 : 0);

			if(finalSize == 0)
			{
				format = TextureFormat.Unknown;
				width = 0;
				height = 0;
				mipmapCount = 0;
				return null;
			}

			highmipsStream.Seek(textures[index].highmipsPointer.offset, SeekOrigin.Begin);
			byte[] data = new byte[finalSize];
			highmipsStream.Read(data, 0x00, (int)textures[index].highmipsPointer.length);

			rippedBytes += textures[index].highmipsPointer.length;

			format = textures[index].format;
			width = textures[index].width;
			height = textures[index].height;
			mipmapCount = 1;

			if(ripMipmaps)
			{
				mipmapCount = textures[index].mipmapCount;

				texturesStream.Seek(textures[index].texturesPointer.offset, SeekOrigin.Begin);
				texturesStream.Read(data, (int)rippedBytes, (int)textures[index].texturesPointer.length);
			}


			if(format != TextureFormat.DXT1 && format != TextureFormat.DXT5)
			{
				Console.WriteLine($"{tuid.ToString("X016")} has an unsupported texture format");
			}

			return data;
		}
		public uint CalculateTextureSize(ulong tuid, int mipmap)
		{
			int index = FindTexture(tuid);
			if(index < 0) return 0;
			

			switch(textures[index].format)
			{
				case TextureFormat.DXT1:
				case TextureFormat.DXT5:
					uint size = 0;
					for(int i = (mipmap < 0 ? 0 : mipmap); i < (mipmap < 0 ? textures[index].mipmapCount : mipmap + 1); i++)
					{
						size += (uint)(
							Math.Max(1, ((textures[index].width  / (1 << i)) + 3) / 4) * 
							Math.Max(1, ((textures[index].height / (1 << i)) + 3) / 4)
							);
					}
					size *= textures[index].format == TextureFormat.DXT1 ? 8u : 16u;
					return size;
				default:
					return 0;
			}
		}
	}

	public struct Texture
	{
		public IGHWPointer texturesPointer;
		public IGHWPointer highmipsPointer;
		public TextureFormat format;
		public int mipmapCount;
		public int width;
		public int height;
	}
	public enum TextureFormat
	{
		DXT1,
		DXT5,
		Unknown,
	}
}