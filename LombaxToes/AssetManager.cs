namespace LombaxToes.Editor
{
	public static class AssetManager
	{
		public static AssetLookup assetlookup;
		public static TextureGroup textureGroup;
		public static ModelGroup modelGroup;
		public static ShaderGroup shaderGroup;

		public static Dictionary<uint, Texture> textures = new Dictionary<uint, Texture>();
		public static Dictionary<uint, Material> materials = new Dictionary<uint, Material>();
		public static Dictionary<uint, Model> models = new Dictionary<uint, Model>();

		static FileStream assetlookupFS;
		static FileStream texturesFS;
		static FileStream highmipsFS;
		static FileStream mobysFS;
		static FileStream tiesFS;
		static FileStream shadersFS;

		public static void LoadAssets(string folderPath)
		{
			assetlookupFS = new FileStream(folderPath + "/assetlookup.dat", FileMode.Open, FileAccess.ReadWrite);
			texturesFS = new FileStream(folderPath + "/textures.dat", FileMode.Open, FileAccess.ReadWrite);
			highmipsFS = new FileStream(folderPath + "/highmips.dat", FileMode.Open, FileAccess.ReadWrite);
			mobysFS = new FileStream(folderPath + "/mobys.dat", FileMode.Open, FileAccess.ReadWrite);
			tiesFS = new FileStream(folderPath + "/ties.dat", FileMode.Open, FileAccess.ReadWrite);
			shadersFS = new FileStream(folderPath + "/shaders.dat", FileMode.Open, FileAccess.ReadWrite);

			assetlookup = new AssetLookup(assetlookupFS);
			textureGroup = assetlookup.ReadTextures(texturesFS, highmipsFS);
			modelGroup = assetlookup.ReadModels(mobysFS, tiesFS);
			shaderGroup = assetlookup.ReadShaders(shadersFS);

			EntityManager.LoadLevel(folderPath);
		}
		public static Texture LoadTexture(ulong tuid) => LoadTexture((uint)tuid);
		public static Texture LoadTexture(uint tuid)
		{
			if(textures.Any(x => x.Key == tuid)) return textures.First(x => x.Key == tuid).Value;

			//Console.WriteLine(tuid.ToString("X08"));	
		
			byte[] data = textureGroup.RipTexture(tuid, false, out TextureFormat format, out int width, out int height, out int mipmapCount);

			if(data != null || width != 0)
			{
				Texture tex = new Texture(data, format, width, height, mipmapCount);
				textures.Add(tuid, tex);
				return tex;
			}
			else
			{
				Console.WriteLine($"Texture {tuid.ToString("X08")} not found in assetlookup.dat");
				textures.Add(tuid, null);
				return null;
			}
		}

		public static Model LoadIrb(ulong tuid) => LoadIrb((uint)tuid);
		public static Model LoadIrb(uint tuid)
		{
			if(models.Any(x => x.Key == tuid)) return models.First(x => x.Key == tuid).Value;

			IrbModel irb = modelGroup.GetModelFromTuid(tuid);

			if(irb == null)
			{
				models.Add(tuid, null);
				return null;
			}

			Model mod = new Model(irb);

			models.Add(tuid, mod);

			return mod;
		}
	}
}