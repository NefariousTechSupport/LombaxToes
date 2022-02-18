namespace LibLombaxToes
{
	//It has a hybrid drivetrain, combining an internal combustion engine with an electric motor. Initially offered as a four-door sedan, it has been produced only as a five-door liftback since 2003.

	public class GP_Prius : IGHW
	{
		public Instance[] instances;
		public string[] instanceNames;
		public IGHWPointer[] instanceNamePointers;

		public Volume[] volumes;
		public string[] volumeNames;
		public IGHWPointer[] volumeNamePointers;


		public int instanceCount;
		public int volumeCount;

		public GP_Prius(Stream input) : base(input)
		{
			ReadInstances();
			ReadVolumes();
		}
		public void ReadInstances()
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
		public void ReadVolumes()
		{
			IGHWSectionHeader volumeNamesSection = GetSectionHeader(IGHWSectionIdentifier.PriusVolumeNames);
			IGHWSectionHeader volumesSections = GetSectionHeader(IGHWSectionIdentifier.PriusVolumes);

			volumeCount = (int)volumesSections.itemCount & ~0x10000000;

			volumeNames = new string[volumeCount];
			volumes = new Volume[volumeCount];

			sh.BaseStream.Seek(volumeNamesSection.offset, SeekOrigin.Begin);
			volumeNamePointers = ReadPointerArray(volumeCount);				//No ordinary pointer array
			for(int i = 0; i < volumeCount; i++)
			{
				volumeNames[i] = sh.ReadStringFromOffset(volumeNamePointers[i].offset);
				sh.BaseStream.Seek(volumesSections.offset + i * 0x40, SeekOrigin.Begin);
				volumes[i] = sh.ReadStruct<Volume>();
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
		[FieldOffset(0x20)] public float xrot;
		[FieldOffset(0x24)] public float yrot;
		[FieldOffset(0x28)] public float zrot;
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x40)]
	public struct Volume
	{
		[FieldOffset(0x00)] public float item01;
		[FieldOffset(0x04)] public float item02;
		[FieldOffset(0x08)] public float item03;
		[FieldOffset(0x0C)] public float item04;
		[FieldOffset(0x10)] public float item05;
		[FieldOffset(0x14)] public float item06;
		[FieldOffset(0x18)] public float item07;
		[FieldOffset(0x1C)] public float item08;
		[FieldOffset(0x20)] public float item09;
		[FieldOffset(0x24)] public float item10;
		[FieldOffset(0x28)] public float item11;
		[FieldOffset(0x2C)] public float item12;
		[FieldOffset(0x30)] public float item13;
		[FieldOffset(0x34)] public float item14;
		[FieldOffset(0x38)] public float item15;
		[FieldOffset(0x3C)] public float item16;
	}
}