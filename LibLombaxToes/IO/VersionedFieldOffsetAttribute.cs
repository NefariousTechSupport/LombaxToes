namespace LibLombaxToes.IO
{
	[System.AttributeUsage(System.AttributeTargets.Struct)]
	public class VersionedFieldOffsetAttribute : Attribute
	{
		public Dictionary<Games, uint> offsets;

		public VersionedFieldOffsetAttribute(params (Games, uint)[] offsetInfo)
		{
			offsets = offsetInfo.ToDictionary(item => item.Item1, item => item.Item2);
		}
	}
}