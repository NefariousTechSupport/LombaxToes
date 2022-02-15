namespace LibLombaxToes
{
	public class IGHW
	{
		public StreamHelper sh;

		public IGHWHeader header;
		public IGHWSectionHeader[] sections;

		public IGHW(Stream input)
		{
			sh = new StreamHelper(input, StreamHelper.Endianness.Big);

			header = sh.ReadStruct<IGHWHeader>();

			if(header.magic != 0x49474857)
			{
				throw new InvalidDataException("This is not an IGHW file");
			}

			sections = new IGHWSectionHeader[header.sectionCount];

			for(int i = 0; i < header.sectionCount; i++)
			{
				sections[i] = sh.ReadStruct<IGHWSectionHeader>();
			}
		}

		public IGHWPointer[] ReadPointerArray(int count)
		{
			IGHWPointer[] pointers = new IGHWPointer[count];
			for(int i = 0; i < count; i++)
			{
				pointers[i] = sh.ReadStruct<IGHWPointer>();
			}
			return pointers;
		}

		public IGHWSectionHeader GetSectionHeader(IGHWSectionIdentifier id)
		{
			return sections.First(x => x.identifier == id);
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x20)]
	public struct IGHWHeader
	{
		[FieldOffset(0x00)] public uint magic;
		[FieldOffset(0x04)] public ushort majorVersion;
		[FieldOffset(0x06)] public ushort minorVersion;
		[FieldOffset(0x08)] public uint sectionCount;
		[FieldOffset(0x0C)] public uint headerLength;
		[FieldOffset(0x10)] public uint fileLength;
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x10)]
	public struct IGHWPointer
	{
		[FieldOffset(0x00)] public ulong tuid;
		[FieldOffset(0x08)] public uint offset;
		[FieldOffset(0x0C)] public uint length;
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x10)]
	public struct IGHWSectionHeader
	{
		[FieldOffset(0x00), MarshalAs(UnmanagedType.U4)] public IGHWSectionIdentifier identifier;
		[FieldOffset(0x04)] public uint offset;
		[FieldOffset(0x08)] public uint itemCount;
		[FieldOffset(0x0C)] public uint itemLength;
	}
	public enum IGHWSectionIdentifier : uint
	{
		// = 0x0001D000
		Shaders				 = 0x0001D100,
		ShaderOnes			 = 0x0001D101,
		HighmipMetadata		 = 0x0001D140,
		Textures			 = 0x0001D180,
		TextureOnes			 = 0x0001D181,
		Highmips			 = 0x0001D1C0,
		Cubemaps			 = 0x0001D200,
		CubemapOnes			 = 0x0001D201,
		Ties				 = 0x0001D300,
		TieOnes				 = 0x0001D301,
		Mobys				 = 0x0001D600,
		MobyOnes			 = 0x0001D601,
		Animsets			 = 0x0001D700,
		AnimsetOnes			 = 0x0001D701,
		Zones				 = 0x0001DA00,
		ZoneOnes			 = 0x0001DA01,
		Lighting			 = 0x0001DB00,
		LightingOnes		 = 0x0001DB01,
	}
}