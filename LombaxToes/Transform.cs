namespace LombaxToes.Editor
{
	public class Transform
	{
		public Vector3 position = Vector3.Zero;
		public Quaternion rotation = Quaternion.Identity;
		public Vector3 scale = Vector3.One;

		private Matrix4 modelMatrix;
		public bool useMatrix = false;

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

		public Transform()
		{
			position = Vector3.Zero;	
			rotation = Quaternion.Identity;
			scale = Vector3.One;	
		}

		public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, rotation.Z) * Quaternion.FromAxisAngle(Vector3.UnitY, rotation.Y) * Quaternion.FromAxisAngle(Vector3.UnitX, rotation.X);
			this.scale = scale;
		}
		public Transform(Matrix4 mat)
		{
			useMatrix = true;
			modelMatrix = mat;
			position = mat.ExtractTranslation();
			scale = mat.ExtractScale();
			rotation = mat.ExtractRotation();
		}
		public Matrix4 GetLocalToWorldMatrix()
		{
			if(useMatrix) return modelMatrix;
			return Matrix4.Identity * Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
		}
	}
}