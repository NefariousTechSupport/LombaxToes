namespace LibLombaxToes
{
	public class AssetLookup : IGHW
	{
		public AssetLookup(Stream input) : base(input){}

		public TextureGroup ReadTextures(Stream texStream, Stream? hmStream)
		{
			return new TextureGroup(texStream, hmStream, this);
		}
	}
}