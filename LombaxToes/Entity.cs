namespace LombaxToes.Editor
{
	public class Entity
	{
		public Model model;
		public Transform transform;

		public Entity(Vector3 position, Vector3 rotation, Vector3 scale, Model model)
		{
			transform = new Transform();
			transform.position = position;
			transform.rotation = Quaternion.FromEulerAngles(rotation);
			transform.scale = scale;

			this.model = model;
		}
		public Entity(Matrix4 transformation, Model model)
		{
			transform = new Transform(transformation);

			this.model = model;
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