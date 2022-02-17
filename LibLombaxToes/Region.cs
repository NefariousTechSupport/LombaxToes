namespace LibLombaxToes
{
	public class Region : IGHW
	{
		public ulong[] mobyTuids;

		public int mobyTuidCount;

		public Region(Stream input) : base(input)
		{
			IGHWSectionHeader mobyTuidsSection = GetSectionHeader(IGHWSectionIdentifier.RegionMobys);

			mobyTuidCount = (int)mobyTuidsSection.itemCount & ~0x10000000;

			mobyTuids = new ulong[mobyTuidCount];

			sh.BaseStream.Seek(mobyTuidsSection.offset, SeekOrigin.Begin);

			for(int i = 0; i < mobyTuidCount; i++)
			{
				mobyTuids[i] = sh.ReadUInt64();
			}
		}
	}
}