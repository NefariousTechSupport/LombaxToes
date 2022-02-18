namespace LibLombaxToes
{
	//Get your head out of the gutters

	public class Zone : IGHW
	{
		public TieInstance[] instances;
		public string[] instanceNames;
		public IGHWPointer[] instanceNamePointers;

		public int instanceCount;

		public ulong[] tieTuids;

		public Zone(Stream input, ModelGroup ties) : base(input)
		{
			ReadTieTuids();
			ReadInstances(ties);
		}
		public void ReadTieTuids()
		{
			IGHWSectionHeader tieTuidsSection = GetSectionHeader(IGHWSectionIdentifier.ZoneTieTuids);
			sh.BaseStream.Seek(tieTuidsSection.offset, SeekOrigin.Begin);
			tieTuids = new ulong[(int)tieTuidsSection.itemCount & ~0x10000000];
			for(int i = 0; i < tieTuids.Length; i++)
			{
				tieTuids[i] = sh.ReadUInt64();
			}
		}
		public void ReadInstances(ModelGroup ties)
		{
			IGHWSectionHeader instanceNamesSection = GetSectionHeader(IGHWSectionIdentifier.ZoneTieNames);
			IGHWSectionHeader instancesSections = GetSectionHeader(IGHWSectionIdentifier.ZoneTieInstances);

			instanceCount = (int)instancesSections.itemCount & ~0x10000000;

			instanceNames = new string[instanceCount];
			instances = new TieInstance[instanceCount];

			sh.BaseStream.Seek(instanceNamesSection.offset, SeekOrigin.Begin);
			instanceNamePointers = ReadPointerArray(instanceCount);				//No ordinary pointer array
			for(int i = 0; i < instanceCount; i++)
			{
				instanceNames[i] = sh.ReadStringFromOffset(instanceNamePointers[i].offset);
				sh.BaseStream.Seek(instancesSections.offset + i * 0x80, SeekOrigin.Begin);
				instances[i] = sh.ReadStruct<TieInstance>();

				Console.WriteLine($"\"{instanceNames[i]}\" should be \"{ties.GetModelFromTuid(tieTuids[instances[i].tieIndex]).path}\"");
			}
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x80)]
	public struct TieInstance
	{
		/*[FieldOffset(0x40)] public float xpos;
		[FieldOffset(0x44)] public float ypos;
		[FieldOffset(0x48)] public float zpos;*/
		[FieldOffset(0x00)] public float float01;
		[FieldOffset(0x04)] public float float02;
		[FieldOffset(0x08)] public float float03;
		[FieldOffset(0x0C)] public float float04;
		[FieldOffset(0x10)] public float float05;
		[FieldOffset(0x14)] public float float06;
		[FieldOffset(0x18)] public float float07;
		[FieldOffset(0x1C)] public float float08;
		[FieldOffset(0x20)] public float float09;
		[FieldOffset(0x24)] public float float10;
		[FieldOffset(0x28)] public float float11;
		[FieldOffset(0x2C)] public float float12;
		[FieldOffset(0x30)] public float float13;
		[FieldOffset(0x34)] public float float14;
		[FieldOffset(0x38)] public float float15;
		[FieldOffset(0x3C)] public float float16;
		[FieldOffset(0x50)] public uint tieIndex;
	}
}