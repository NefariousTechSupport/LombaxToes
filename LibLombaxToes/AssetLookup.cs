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
	}
}