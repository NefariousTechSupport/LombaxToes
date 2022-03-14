namespace LombaxToes.Editor
{
	public class Entity
	{
		public Model model;
		public Transform transform;
		Vector3 boundingCentre;
		float boundingRadius;

		public Entity(Vector3 position, Vector3 rotation, Vector3 scale, Model model, Vector3 centre, float radius)
		{
			transform = new Transform(position, rotation, scale);

			this.model = model;
			boundingCentre = centre;
			boundingRadius = radius;
		}
		public Entity(Matrix4 transformation, Model model, Vector3 centre, float radius)
		{
			transform = new Transform(transformation);

			this.model = model;
			boundingCentre = centre;
			boundingRadius = radius;
		}

		public void Render()
		{
			if(model != null)
			{
				model.Render(transform);
			}
		}
	}
}