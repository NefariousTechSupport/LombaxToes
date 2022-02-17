namespace LombaxToes.Editor
{
	public class Material
	{
		public int programId;
		Texture albedo;

		public Material(int handle, Texture albedo)
		{
			this.albedo = albedo;
			this.programId = handle;
		}

		public void Use()
		{
			GL.UseProgram(programId);
			if(albedo != null)
			{
				albedo.Use();
				SetInt("albedo", 0);
			}
		}

		public void SetMatrix4x4(string name, Matrix4 data)
		{
			int dataLocation = GL.GetUniformLocation(programId, name);
			GL.UniformMatrix4(dataLocation, true, ref data);
		}

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