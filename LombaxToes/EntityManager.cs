namespace LombaxToes.Editor
{
	public static class EntityManager
	{
		public static List<Entity> entities = new List<Entity>();

		public static void Render()
		{
			for(int i = 0; i < entities.Count; i++)
			{
				entities[i].Render();
			}
		}
	}
}