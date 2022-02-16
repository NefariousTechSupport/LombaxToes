namespace LombaxToes.Editor
{
	public class Material
	{
		public int programId;
		Texture albedo;

		public Material(string vertexShaderPath, string fragmentShaderPath, Texture albedo)
		{
			this.albedo = albedo;

			string vertexSource = File.ReadAllText(vertexShaderPath);
			string fragmentSource = File.ReadAllText(fragmentShaderPath);

			int vertexProgramId = GL.CreateShader(ShaderType.VertexShader);
			int fragmentProgramId = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(vertexProgramId, vertexSource);
			GL.CompileShader(vertexProgramId);

			GL.ShaderSource(fragmentProgramId, fragmentSource);
			GL.CompileShader(fragmentProgramId);

			GL.GetShader(vertexProgramId, ShaderParameter.CompileStatus, out int res);
			if(res != (int)All.True)
			{
				string infoLog = GL.GetShaderInfoLog(vertexProgramId);
				throw new Exception($"Error when compiling vertex shader at {vertexShaderPath}.\nError: {infoLog}");
			}

			GL.GetShader(fragmentProgramId, ShaderParameter.CompileStatus, out res);
			if(res != (int)All.True)
			{
				string infoLog = GL.GetShaderInfoLog(fragmentProgramId);
				throw new Exception($"Error when compiling fragment shader at {fragmentShaderPath}.\nError: {infoLog}");
			}

			programId = GL.CreateProgram();
			GL.AttachShader(programId, vertexProgramId);
			GL.AttachShader(programId, fragmentProgramId);

			GL.LinkProgram(programId);

			GL.GetProgram(fragmentProgramId, GetProgramParameterName.LinkStatus, out res);
			if(res != (int)All.True)
			{
				string infoLog = GL.GetProgramInfoLog(programId);
				throw new Exception($"Error when linking program. Error: {infoLog}");
			}

			GL.DetachShader(programId, vertexProgramId);
			GL.DetachShader(programId, fragmentProgramId);

			GL.DeleteShader(vertexProgramId);
			GL.DeleteShader(fragmentProgramId);
		}

		public void Use()
		{
			if(albedo != null)
			{
				albedo.Use();
				SetInt("albedo", 0);
			}
			GL.UseProgram(programId);
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