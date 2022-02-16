namespace LombaxToes.Editor
{
	public class Model
	{
		int[] VBOs;
		int[] VAOs;
		int[] EBOs;

		Material[] materials;
		int[] indexCounts;

		Transform transform;

		public Model(IrbModel model)
		{
			VBOs = new int[model.meshCount];
			VAOs = new int[model.meshCount];
			EBOs = new int[model.meshCount];
			materials = new Material[model.meshCount];
			indexCounts = new int[model.meshCount];

			transform = new Transform();

			materials[0] = new Material("Shaders/standard.vert.glsl", "Shaders/standardunlit.frag.glsl", null);	//Temp

			GL.GenBuffers(model.meshCount, VBOs);
			GL.GenVertexArrays(model.meshCount, VAOs);
			GL.GenBuffers(model.meshCount, EBOs);

			for(int i = 0; i < model.meshCount; i++)
			{
				float[] vertices = model.ReadVertexBuffer(i);
				uint[] indices = model.ReadIndexBuffer(i);

				materials[i] = materials[0];		//Temporary
				indexCounts[i] = indices.Length;

				GL.BindBuffer(BufferTarget.ArrayBuffer, VBOs[i]);
				GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

				GL.BindVertexArray(VAOs[i]);
				GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
				GL.EnableVertexAttribArray(0);

				GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBOs[i]);
				GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			}
		}

		public void Render()
		{
			for(int i = 0; i < VBOs.Length; i++)
			{
				materials[i].Use();
				
				materials[i].SetMatrix4x4("model", transform.GetLocalToWorldMatrix());
				materials[i].SetMatrix4x4("view", Camera.WorldToView);
				materials[i].SetMatrix4x4("projection", Camera.ViewToClip);

				GL.BindVertexArray(VAOs[i]);
				GL.DrawElements(PrimitiveType.Triangles, indexCounts[i], DrawElementsType.UnsignedInt, 0);
			}
		}
		public void Dispose()
		{
			materials[0].Dispose();		//Temp
		}
	}
}