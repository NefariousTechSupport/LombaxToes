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
				textures[i].mipmapCount = parent.sh.ReadByte();
				textures[i].width = 1 << parent.sh.ReadByte();
				textures[i].height = 1 << parent.sh.ReadByte();
			}
		}
		//Rewrite to support mipmaps and highmips
		public byte[] RipTexture(ulong tuid, bool ripMipmaps, out TextureFormat format, out int width, out int height, out int mipmapCount)
		{
			int index = Array.FindIndex<Texture>(textures, x => (x.texturesPointer.tuid & 0xFFFFFFFF) == (tuid & 0xFFFFFFFF));	//Usually texture refs only store the last 4 bytes so we only check those last 4 bytes
			if(index < 0)
			{
				format = TextureFormat.DXT1;
				width = 0;
				height = 0;
				mipmapCount = 0;
				return null;
			}

			if(ripMipmaps) throw new NotImplementedException("Mipmaps aren't supported");
			if(highmipsStream == null) throw new NotImplementedException("Highmips must be assigned");

			highmipsStream.Seek(textures[index].highmipsPointer.offset, SeekOrigin.Begin);
			byte[] data = new byte[textures[index].highmipsPointer.length];
			highmipsStream.Read(data, 0x00, (int)textures[index].highmipsPointer.length);

			format = textures[index].format;
			width = textures[index].width;
			height = textures[index].height;
			mipmapCount = textures[index].mipmapCount;

			return data;
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
	}
}