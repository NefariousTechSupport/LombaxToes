namespace LibLombaxToes
{
	public class Shader : IGHW
	{
		public uint[] textureTuids;
		public int textureTuidCount;
		public string name;

		public Shader(Stream input) : base(input)
		{
			IGHWSectionHeader textureSection = GetSectionHeader(IGHWSectionIdentifier.ShaderTextures);
			IGHWSectionHeader stringSection = GetSectionHeader(IGHWSectionIdentifier.ShaderStrings);

			textureTuidCount = (int)textureSection.itemCount & ~0x10000000;

			name = sh.ReadStringFromOffset(stringSection.offset);

			textureTuids = new uint[textureTuidCount];

			for(int i = 0; i < textureTuidCount; i++)
			{
				sh.BaseStream.Seek(textureSection.offset + textureSection.itemLength * i, SeekOrigin.Begin);
				textureTuids[i] = sh.ReadUInt32();
			}
		}

		public uint GetAlbedoTextureTuid()
		{
			if(textureTuids.Length == 0)
			{
				Console.WriteLine($"No textures in shader {name}");
				return 0;
			}
			return textureTuids[0];
		}
	}
}