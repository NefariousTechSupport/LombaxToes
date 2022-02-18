namespace LombaxToes.Editor
{
	public class Transform
	{
		public Vector3 position = Vector3.Zero;
		public Quaternion rotation = Quaternion.Identity;
		public Vector3 scale = Vector3.One;

		public Vector3 Forward
		{
			get
			{
				return Quaternion.Invert(rotation) * Vector3.UnitZ;
			}
		}

		public Vector3 Right
		{
			get
			{
				return Quaternion.Invert(rotation) * Vector3.UnitX;
			}
		}

		public Transform(){}
		public Transform(Matrix4 mat)
		{
			position = mat.ExtractTranslation();
			rotation = mat.ExtractRotation();
			scale = mat.ExtractScale();
		}
		public Matrix4 GetLocalToWorldMatrix()
		{
			return Matrix4.Identity * Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
		}
	}
}