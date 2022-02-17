namespace LombaxToes.Editor
{
	public static class EntityManager
	{
		public static List<Entity> entities = new List<Entity>();

		public static GP_Prius prius;
		public static Region region;

		static FileStream priusFS;
		static FileStream regionFS;

		public static void LoadLevel(string folderPath)
		{
			priusFS = new FileStream(folderPath + "/default/gp_prius.dat", FileMode.Open, FileAccess.ReadWrite);
			regionFS = new FileStream(folderPath + "/default/region.dat", FileMode.Open, FileAccess.ReadWrite);

			prius = new GP_Prius(priusFS);
			region = new Region(regionFS);

			Console.WriteLine($"{prius.instanceCount} instances");

			for(int i = 0; i < prius.instanceCount; i++)
			{
				Console.WriteLine($"Instance {i.ToString("X04")} : {prius.instances[i].xpos} {prius.instances[i].ypos} {prius.instances[i].zpos} : {prius.instanceNames[i]}");// with moby {region.mobyTuids[prius.instances[i].mobyIdex].ToString("X016")}");
				EntityManager.entities.Add(new Entity(new Vector3(prius.instances[i].xpos, prius.instances[i].ypos, prius.instances[i].zpos), Vector3.Zero, Vector3.One * 10, AssetManager.LoadIrb(region.mobyTuids[prius.instances[i].mobyIdex])));
			}
			//Camera.transform.position = EntityManager.entities.Last().transform.position;
			Console.WriteLine($"{entities.Count} entities");
			Console.WriteLine($"{Camera.transform.position.X} {Camera.transform.position.Y} {Camera.transform.position.Z}");
		}

		public static void Render()
		{
			for(int i = 0; i < entities.Count; i++)
			{
				entities[i].Render();
			}
		}
	}
}