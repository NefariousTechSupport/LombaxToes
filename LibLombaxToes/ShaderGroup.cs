namespace LibLombaxToes
{
	public class ShaderGroup
	{
		Stream shaderStream;

		IGHWPointer[] shaderPointers;

		Shader[] shaders;

		public int shaderCount;

		public ShaderGroup(Stream shaderStream, AssetLookup parent)
		{
			this.shaderStream = shaderStream;

			IGHWSectionHeader shaderSection = parent.GetSectionHeader(IGHWSectionIdentifier.Shaders);

			shaderCount = (int)shaderSection.itemLength / 0x10;

			shaders = new Shader[shaderCount];

			parent.sh.BaseStream.Seek(shaderSection.offset, SeekOrigin.Begin);
			shaderPointers = parent.ReadPointerArray(shaderCount);

			for(int i = 0; i < shaderCount; i++)
			{
				shaderStream.Seek(shaderPointers[i].offset, SeekOrigin.Begin);
				byte[] data = new byte[shaderPointers[i].length];
				shaderStream.Read(data);
				shaders[i] = new Shader(new MemoryStream(data));
			}
		}
		public Shader GetShaderFromTuid(ulong tuid) => GetShaderFromTuid((uint)tuid);
		public Shader GetShaderFromTuid(uint tuid)
		{
			int index = Array.FindIndex<IGHWPointer>(shaderPointers, x => (x.tuid & 0xFFFFFFFF) == tuid);
			if(index < 0)
			{
				return null;
			}
			else
			{
				return shaders[index];
			}
		}
	}
}