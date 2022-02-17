namespace LibLombaxToes
{
	public class ModelGroup
	{
		Stream mobyStream;
		Stream tieStream;

		IGHWPointer[] mobyPointers;
		IGHWPointer[] tiePointers;

		IrbModel[] mobys;
		IrbModel[] ties;

		public int mobyCount;
		public int tieCount;

		public ModelGroup(Stream mobyStream, Stream tieStream, AssetLookup parent)
		{
			this.mobyStream = mobyStream;
			this.tieStream = tieStream;

			IGHWSectionHeader mobySection = parent.GetSectionHeader(IGHWSectionIdentifier.Mobys);
			IGHWSectionHeader tieSection = parent.GetSectionHeader(IGHWSectionIdentifier.Ties);

			mobyCount = (int)mobySection.itemLength / 0x10;
			tieCount = (int)tieSection.itemLength / 0x10;

			mobys = new IrbModel[mobyCount];
			ties = new IrbModel[tieCount];

			parent.sh.BaseStream.Seek(mobySection.offset, SeekOrigin.Begin);
			mobyPointers = parent.ReadPointerArray(mobyCount);

			parent.sh.BaseStream.Seek(tieSection.offset, SeekOrigin.Begin);
			tiePointers = parent.ReadPointerArray(tieCount);

			for(int i = 0; i < mobyCount; i++)
			{
				mobyStream.Seek(mobyPointers[i].offset, SeekOrigin.Begin);
				byte[] data = new byte[mobyPointers[i].length];
				mobyStream.Read(data);
				mobys[i] = new IrbModel(new MemoryStream(data));
			}
			for(int i = 0; i < tieCount; i++)
			{
				tieStream.Seek(tiePointers[i].offset, SeekOrigin.Begin);
				byte[] data = new byte[tiePointers[i].length];
				tieStream.Read(data);
				ties[i] = new IrbModel(new MemoryStream(data));
			}
		}
		public IrbModel GetModelFromTuid(ulong tuid) => GetModelFromTuid((uint)tuid);
		public IrbModel GetModelFromTuid(uint tuid)
		{
			int index = Array.FindIndex<IGHWPointer>(mobyPointers, x => (x.tuid & 0xFFFFFFFF) == tuid);
			if(index < 0)
			{
				index = Array.FindIndex<IGHWPointer>(tiePointers, x => (x.tuid & 0xFFFFFFFF) == tuid);

				if(index < 0)
				{
					return null;
				}
				else
				{
					return ties[index];
				}
			}
			else
			{
				return mobys[index];
			}
		}
	}
}