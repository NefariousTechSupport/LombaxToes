using System.Runtime.InteropServices;

namespace LombaxToes.Editor
{
	public class Model
	{
		int[] VBOs;
		int[] VAOs;
		int[] EBOs;
		int instanceMatrices;

		public Material[] materials;
		int[] indexCounts;

		public List<Transform> transforms = new List<Transform>();
		public Matrix4[] transformMatrices = new Matrix4[0];

		public Model(IrbModel model)
		{
			VBOs = new int[model.meshCount];
			VAOs = new int[model.meshCount];
			EBOs = new int[model.meshCount];
			materials = new Material[model.meshCount];
			indexCounts = new int[model.meshCount];

			GL.GenBuffers(model.meshCount, VBOs);
			GL.GenVertexArrays(model.meshCount, VAOs);
			GL.GenBuffers(model.meshCount, EBOs);

			for(int i = 0; i < model.meshCount; i++)
			{
				float[] vertices = model.ReadVertexBuffer(i);
				uint[] indices = model.ReadIndexBuffer(i);

				try
				{
					Shader shader = AssetManager.shaderGroup.GetShaderFromTuid(model.GetShaderTuid(i));
					uint albedoTuid = 0;
					for(int j = 0; j < shader.textureTuidCount; j++)
					{
						albedoTuid = shader.GetTextureTuid(j);
						if(albedoTuid == 0) continue;
						if(AssetManager.textureGroup.FindTexture(albedoTuid) < 0) continue;
						break;
					}
					materials[i] = new Material(MaterialManager.shaders["standard.vert;standardunlit.frag"], AssetManager.LoadTexture(albedoTuid));		//Temporary
				}
				catch(NullReferenceException)
				{
					materials[i] = new Material(MaterialManager.shaders["standard.vert;standardunlit.frag"], null);
				}

				indexCounts[i] = indices.Length;

				GL.BindBuffer(BufferTarget.ArrayBuffer, VBOs[i]);
				GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

				GL.BindVertexArray(VAOs[i]);
				GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);					//Position data
				GL.EnableVertexAttribArray(0);

				GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));	//UV data
				GL.EnableVertexAttribArray(1);

				GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBOs[i]);
				GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			}
		}

		public Model(float[] vertices, uint[] indices)
		{
			VBOs = new int[1];
			VAOs = new int[1];
			EBOs = new int[1];
			materials = new Material[1];
			indexCounts = new int[1];

			GL.GenBuffers(1, VBOs);
			GL.GenVertexArrays(1, VAOs);
			GL.GenBuffers(1, EBOs);

			materials[0] = new Material(MaterialManager.shaders["standard.vert;standardwhite.frag"], null);		//Temporary
			materials[0].drawType = PrimitiveType.Lines;
			indexCounts[0] = indices.Length;

			GL.BindBuffer(BufferTarget.ArrayBuffer, VBOs[0]);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			GL.BindVertexArray(VAOs[0]);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBOs[0]);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
		}

		public void Render()
		{
			if(transformMatrices.Length != transforms.Count)
			{
				GL.DeleteBuffer(instanceMatrices);
				transformMatrices = new Matrix4[transforms.Count];
				instanceMatrices = GL.GenBuffer();
			}

			for(int i = 0; i < transforms.Count; i++)
			{
				transformMatrices[i] = Matrix4.Transpose(transforms[i].GetLocalToWorldMatrix() * Camera.WorldToView * Camera.ViewToClip);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, instanceMatrices);
			GL.BufferData(BufferTarget.ArrayBuffer, transformMatrices.Length * sizeof(float) * 16, transformMatrices, BufferUsageHint.StaticDraw);

			for(int i = 0; i < VBOs.Length; i++)
			{
				materials[i].Use();

				GL.BindVertexArray(VAOs[i]);

				GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 0);
				GL.VertexAttribDivisor(5, 1);
				GL.EnableVertexAttribArray(5);

				GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 4 * sizeof(float));
				GL.VertexAttribDivisor(6, 1);
				GL.EnableVertexAttribArray(6);

				GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 8 * sizeof(float));
				GL.VertexAttribDivisor(7, 1);
				GL.EnableVertexAttribArray(7);

				GL.VertexAttribPointer(8, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 12 * sizeof(float));
				GL.VertexAttribDivisor(8, 1);
				GL.EnableVertexAttribArray(8);

				GL.DrawElementsInstanced(materials[i].drawType, indexCounts[i], DrawElementsType.UnsignedInt, IntPtr.Zero, transforms.Count);
			}
		}
		public void Dispose()
		{
			GL.DeleteBuffers(VBOs.Length, VBOs);
			GL.DeleteBuffers(VAOs.Length, VAOs);
			GL.DeleteBuffers(EBOs.Length, EBOs);
			GC.Collect();
			materials[0].Dispose();		//Temp
		}
	}
}