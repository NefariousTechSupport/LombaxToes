namespace LibLombaxToes
{
	public class AssetLookup : IGHW
	{
		public AssetLookup(Stream input) : base(input){}

		public TextureGroup ReadTextures(Stream texStream, Stream? hmStream)
		{
			return new TextureGroup(texStream, hmStream, this);
		}

		public ModelGroup ReadModels(Stream mobyStream, Stream tieStream)
		{
			return new ModelGroup(mobyStream, tieStream, this);
		}
		public ShaderGroup ReadShaders(Stream shaderStream)
		{
			return new ShaderGroup(shaderStream, this);
		}
		public Zone[] ReadZones(Stream zoneStream)
		{
			IGHWSectionHeader zoneSection = GetSectionHeader(IGHWSectionIdentifier.Zones);
			sh.BaseStream.Seek(zoneSection.offset, SeekOrigin.Begin);
			Zone[] zones = new Zone[zoneSection.itemLength / 0x10];
			IGHWPointer[] zonePointers = ReadPointerArray(zones.Length);
			Console.WriteLine($"{zones.Length} zones");
			for(int i = 0; i < zones.Length; i++)
			{
				Console.WriteLine($"{zonePointers[i].offset.ToString("X08")} zone {i} offset");
				Console.WriteLine($"{zonePointers[i].length.ToString("X08")} zone {i} length");
				zoneStream.Seek(zonePointers[i].offset, SeekOrigin.Begin);
				byte[] zoneData = new byte[zonePointers[i].length];
				zoneStream.Read(zoneData, 0x00, (int)zonePointers[i].length);
				MemoryStream mZoneStream = new MemoryStream(zoneData);
				mZoneStream.Seek(0x00, SeekOrigin.Begin);
				zones[i] = new Zone(mZoneStream);
			}
			return zones;
		}
	}
}