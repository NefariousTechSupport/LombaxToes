namespace LombaxToes.Editor
{
	public static class EntityManager
	{
		public static List<Entity> entities = new List<Entity>();

		public static GP_Prius prius;
		public static Region region;
		public static Zone zone;

		static FileStream priusFS;
		static FileStream regionFS;
		static FileStream zoneFS;

		public static void LoadLevel(string folderPath)
		{
			priusFS = new FileStream(folderPath + "/default/gp_prius.dat", FileMode.Open, FileAccess.ReadWrite);
			regionFS = new FileStream(folderPath + "/default/region.dat", FileMode.Open, FileAccess.ReadWrite);
			zoneFS = new FileStream(folderPath + "/zones.dat", FileMode.Open, FileAccess.ReadWrite);

			prius = new GP_Prius(priusFS);
			region = new Region(regionFS);
			zone = new Zone(zoneFS, AssetManager.modelGroup);

			//Console.WriteLine($"{prius.instanceCount} instances");

			for(int i = 0; i < prius.instanceCount; i++)
			{
				//Console.WriteLine($"Instance {i.ToString("X04")} : {prius.instances[i].xpos} {prius.instances[i].ypos} {prius.instances[i].zpos} : {prius.instanceNames[i]}");// with moby {region.mobyTuids[prius.instances[i].mobyIdex].ToString("X016")}");
				EntityManager.entities.Add(new Entity(new Vector3(prius.instances[i].xpos, prius.instances[i].ypos, prius.instances[i].zpos), new Vector3(prius.instances[i].xrot, prius.instances[i].yrot, prius.instances[i].zrot), Vector3.One * 10, AssetManager.LoadIrb(region.mobyTuids[prius.instances[i].mobyIdex])));
			}

			for(int i = 0; i < zone.instanceCount; i++)
			{
				Matrix4 mat = new Matrix4(
					zone.instances[i].float01, zone.instances[i].float02, zone.instances[i].float03, zone.instances[i].float04,
					zone.instances[i].float05, zone.instances[i].float06, zone.instances[i].float07, zone.instances[i].float08,
					zone.instances[i].float09, zone.instances[i].float10, zone.instances[i].float11, zone.instances[i].float12,
					zone.instances[i].float13, zone.instances[i].float14, zone.instances[i].float15, zone.instances[i].float16
				);
				EntityManager.entities.Add(new Entity(mat, AssetManager.LoadIrb(zone.tieTuids[zone.instances[i].tieIndex])));
			}
			zoneFS.Close();
			//Camera.transform.position = EntityManager.entities.Last().transform.position;
			//Console.WriteLine($"{entities.Count} entities");
			//Console.WriteLine($"{Camera.transform.position.X} {Camera.transform.position.Y} {Camera.transform.position.Z}");
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