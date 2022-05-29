namespace LombaxToes.Editor
{
	public static class Camera
	{
		public static Transform transform = new Transform();

		public static Matrix4 WorldToView
		{
			get
			{
				return Matrix4.CreateTranslation(transform.Position) * Matrix4.CreateFromQuaternion(transform.Rotation);
			}
		}
		public static Matrix4 ViewToClip;

		public static void CreatePerspective(float fov)
		{
			ViewToClip = Matrix4.CreatePerspectiveFieldOfView(fov, 1280f/720f, 0.1f, 10000f);
		}
	}
}