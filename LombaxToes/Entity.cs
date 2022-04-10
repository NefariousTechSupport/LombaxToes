namespace LombaxToes.Editor
{
	public class Entity
	{
		public Model model;
		public Transform transform;
		public Vector3 boundingCentre;
		public float boundingRadius;

		public Entity(Vector3 position, Vector3 rotation, Vector3 scale, Model model, Vector3 centre = new Vector3(), float radius = 0)
		{
			transform = new Transform(position, rotation, scale);

			model.transforms.Add(transform);
			
			boundingCentre = centre;
			boundingRadius = radius;
		}
		public Entity(Matrix4 transformation, Model model, Vector3 centre = new Vector3(), float radius = 0)
		{
			transform = new Transform(transformation);

			this.model = model;

			model.transforms.Add(transform);

			boundingCentre = centre;
			boundingRadius = radius;
		}
	}
}