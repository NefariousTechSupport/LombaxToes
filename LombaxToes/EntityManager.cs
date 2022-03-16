namespace LombaxToes.Editor
{
	public static class EntityManager
	{
		public static List<Entity> entities = new List<Entity>();

		public static GP_Prius prius;
		public static Region region;
		public static Zone[] zones;

		static FileStream priusFS;
		static FileStream regionFS;
		static FileStream zoneFS;

		public static async void LoadLevel(string folderPath)
		{
			priusFS = new FileStream(folderPath + "/default/gp_prius.dat", FileMode.Open, FileAccess.ReadWrite);
			regionFS = new FileStream(folderPath + "/default/region.dat", FileMode.Open, FileAccess.ReadWrite);
			zoneFS = new FileStream(folderPath + "/zones.dat", FileMode.Open, FileAccess.ReadWrite);

			prius = new GP_Prius(priusFS);
			region = new Region(regionFS);
			zones = AssetManager.assetlookup.ReadZones(zoneFS);

			for(int i = 0; i < prius.instanceCount; i++)
			{
				EntityManager.entities.Add(new Entity(new Vector3(prius.instances[i].xpos, prius.instances[i].ypos, prius.instances[i].zpos), new Vector3(prius.instances[i].xrot, prius.instances[i].yrot, prius.instances[i].zrot), Vector3.One * prius.instances[i].scale, AssetManager.LoadIrb(region.mobyTuids[prius.instances[i].mobyIdex])));
			} 

			for(int j = 0; j < zones.Length; j++)
			{
				for(int i = 0; i < zones[j].instanceCount; i++)
				{
					Matrix4 mat = new Matrix4(
						zones[j].instances[i].float01, zones[j].instances[i].float02, zones[j].instances[i].float03, zones[j].instances[i].float04,
						zones[j].instances[i].float05, zones[j].instances[i].float06, zones[j].instances[i].float07, zones[j].instances[i].float08,
						zones[j].instances[i].float09, zones[j].instances[i].float10, zones[j].instances[i].float11, zones[j].instances[i].float12,
						zones[j].instances[i].float13, zones[j].instances[i].float14, zones[j].instances[i].float15, zones[j].instances[i].float16
					);

					//Note: Extract the postiion, rotation, and scale, then apply it, make sure to apply the rotation in ZYX order

					Entity currentTie = new Entity(mat, AssetManager.LoadIrb(zones[j].tieTuids[zones[j].instances[i].tieIndex]));
					
					EntityManager.entities.Add(currentTie);
				}
			}
			zoneFS.Close();
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