namespace LombaxToes.Editor
{
	public class Material
	{
		public int programId;
		Texture? albedo;
		public PrimitiveType drawType;
		public uint numUsing = 0;

		public Material(int handle, Texture albedo, PrimitiveType primitiveType = PrimitiveType.Triangles)
		{
			this.albedo = albedo;
			this.programId = handle;
			this.drawType = primitiveType;
		}

		public Material(int handle)
		{
			this.albedo = null;
			this.programId = handle;
			this.drawType = PrimitiveType.Triangles;
		}

		public void Use()
		{
			GL.UseProgram(programId);
			if(albedo != null)
			{
				albedo.Use();
				SetInt("albedo", 0);
				SetBool("useTexture", true);
			}
			else
			{
				SetBool("useTexture", false);
			}
		}

		public void SetMatrix4x4(string name, Matrix4 data)
		{
			int dataLocation = GL.GetUniformLocation(programId, name);
			GL.UniformMatrix4(dataLocation, true, ref data);
		}

		public void SetBool(string name, bool data) => SetInt(name, data ? 1 : 0);

		public void SetInt(string name, int data)
		{
			int dataLocation = GL.GetUniformLocation(programId, name);
			GL.Uniform1(dataLocation, data);
		}

		public void Dispose()
		{
			GL.DeleteProgram(programId);
		}
	}
}