namespace LibLombaxToes
{
	//It has a hybrid drivetrain, combining an internal combustion engine with an electric motor. Initially offered as a four-door sedan, it has been produced only as a five-door liftback since 2003.

	public class GP_Prius : IGHW
	{
		public Instance[] instances;
		public string[] instanceNames;
		public IGHWPointer[] instanceNamePointers;

		public int instanceCount;

		public GP_Prius(Stream input) : base(input)
		{
			IGHWSectionHeader instanceNamesSection = GetSectionHeader(IGHWSectionIdentifier.PriusInstanceNames);
			IGHWSectionHeader instancesSections = GetSectionHeader(IGHWSectionIdentifier.PriusInstances);

			instanceCount = (int)instancesSections.itemCount & ~0x10000000;

			instanceNames = new string[instanceCount];
			instances = new Instance[instanceCount];

			sh.BaseStream.Seek(instanceNamesSection.offset, SeekOrigin.Begin);
			instanceNamePointers = ReadPointerArray(instanceCount);				//No ordinary pointer array
			for(int i = 0; i < instanceCount; i++)
			{
				instanceNames[i] = sh.ReadStringFromOffset(instanceNamePointers[i].offset);
				sh.BaseStream.Seek(instancesSections.offset + i * 0x50, SeekOrigin.Begin);
				instances[i] = sh.ReadStruct<Instance>();
			}
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x50)]
	public struct Instance
	{
		[FieldOffset(0x00)] public ushort mobyIdex;
		[FieldOffset(0x02)] public ushort mobyGroup;	//Needs to be renamed
		[FieldOffset(0x14)] public float xpos;
		[FieldOffset(0x18)] public float ypos;
		[FieldOffset(0x1C)] public float zpos;
	}
}