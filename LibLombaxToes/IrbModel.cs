using System.Text;

namespace LibLombaxToes
{
	public class IrbModel : IGHW
	{
		int type = -1;		//0 is Moby, 1 is Tie, -1 means no mesh
		MeshMetadata[] metadatas;

		public IrbModel(Stream input) : base(input)
		{
			ReadMeshMetadata();
		}

		void ReadMeshMetadata()
		{
			if(sections.Any(x => x.identifier == IGHWSectionIdentifier.MobyMetadata))
			{
				type = 0;

				IGHWSectionHeader mobyMetadataSection = GetSectionHeader(IGHWSectionIdentifier.MobyMetadata);
				metadatas = new MeshMetadata[mobyMetadataSection.itemCount & ~0x10000000];


				for(int i = 0; i < metadatas.Length; i++)
				{
					sh.BaseStream.Seek(mobyMetadataSection.offset + i*0x40, SeekOrigin.Begin);

					metadatas[i].indicesStart = sh.ReadUInt32();
					metadatas[i].verticesStart = sh.ReadUInt32();
					metadatas[i].shaderIndex = sh.ReadUInt16();
					metadatas[i].vertexCount = sh.ReadUInt16();
					metadatas[i].boneMapIndexCount = sh.ReadByte();
					metadatas[i].type = sh.ReadByte();
					metadatas[i].boneMapIndex = sh.ReadByte();
					sh.ReadByte();

					sh.ReadUInt16();
					metadatas[i].indexCount = sh.ReadUInt16();
					sh.ReadUInt32();
					sh.ReadUInt32();
					sh.ReadUInt32();

					metadatas[i].boneMapOffset = sh.ReadUInt32();
					sh.ReadUInt32();
					metadatas[i].vertexType = sh.ReadByte();
					sh.ReadByte();
					sh.ReadUInt16();
					sh.ReadUInt32();
				}
			}
			else if(sections.Any(x => x.identifier == IGHWSectionIdentifier.TieMetadata))
			{
				type = 1;
			}
			else
			{
				type = -1;
			}
		}

		public void ExportToObj(Stream output, int index)
		{
			float[] vertices = ReadVertexBuffer(index);
			uint[] indices = ReadIndexBuffer(index);

			for(int j = 0; j < vertices.Length; j += 0x05)
			{
				output.Write(Encoding.ASCII.GetBytes($"v {vertices[j + 0].ToString("F")} {vertices[j + 1].ToString("F")} {vertices[j + 2].ToString("F")}\n"));
				output.Write(Encoding.ASCII.GetBytes($"vt {vertices[j + 3].ToString("F")} {vertices[j + 4].ToString("F")}\n"));
			}

			for(int j =0; j < indices.Length; j += 3)
			{
				output.Write(Encoding.ASCII.GetBytes($"f {indices[j + 0]+1}/{indices[j + 0]+1} {indices[j + 1]+1}/{indices[j + 1]+1} {indices[j + 2]+1}/{indices[j + 2]+1}\n"));
			}
		}

		public float[] ReadVertexBuffer(int index)
		{
			IGHWSectionHeader vertexSection;

			if(type == 0)
			{
				vertexSection = GetSectionHeader(IGHWSectionIdentifier.MobyVertices);
			}
			else
			{
				vertexSection = GetSectionHeader(IGHWSectionIdentifier.TieVertices);
			}

			float[] vertices = new float[metadatas[index].vertexCount * 0x05];

			for(int i = 0; i < metadatas[index].vertexCount; i++)
			{
				if(type == 1)                       sh.BaseStream.Seek(vertexSection.offset + (metadatas[index].verticesStart + i) * 0x14, SeekOrigin.Begin);
				else if(metadatas[index].type == 0) sh.BaseStream.Seek(vertexSection.offset +  metadatas[index].verticesStart + i  * 0x14, SeekOrigin.Begin);
				else if(metadatas[index].type == 1) sh.BaseStream.Seek(vertexSection.offset +  metadatas[index].verticesStart + i  * 0x1C, SeekOrigin.Begin);

				vertices[i * 5 + 0] = sh.ReadInt16();
				vertices[i * 5 + 1] = sh.ReadInt16();
				vertices[i * 5 + 2] = sh.ReadInt16();

				if(type == 1)                       sh.BaseStream.Seek(vertexSection.offset + (metadatas[index].verticesStart + i) * 0x14 + 0x08, SeekOrigin.Begin);
				else if(metadatas[index].type == 0) sh.BaseStream.Seek(vertexSection.offset +  metadatas[index].verticesStart + i  * 0x14 + 0x08, SeekOrigin.Begin);
				else if(metadatas[index].type == 1) sh.BaseStream.Seek(vertexSection.offset +  metadatas[index].verticesStart + i  * 0x1C + 0x10, SeekOrigin.Begin);

				vertices[i * 5 + 3] = (float)sh.ReadHalf();
				vertices[i * 5 + 4] = (float)sh.ReadHalf();
			}

			return vertices;
		}

		public uint[] ReadIndexBuffer(int index)
		{
			IGHWSectionHeader indexSection;

			if(type == 0)
			{
				indexSection = GetSectionHeader(IGHWSectionIdentifier.MobyIndices);
			}
			else
			{
				indexSection = GetSectionHeader(IGHWSectionIdentifier.TieIndices);
			}

			uint[] indices = new uint[metadatas[index].indexCount];
			
			sh.BaseStream.Seek(indexSection.offset + metadatas[index].indicesStart * 0x02, SeekOrigin.Begin);

			for(int i = 0; i < metadatas[index].indexCount; i++)
			{
				indices[i] = sh.ReadUInt16();
			}

			return indices;
		}
	}
	public struct MeshMetadata
	{
		public uint type;
		public uint indicesStart;
		public uint verticesStart;
		public uint shaderIndex;
		public uint vertexCount;
		public uint vertexType;
		public uint boneMapOffset;
		public uint boneMapIndex;
		public uint boneMapIndexCount;
		public uint indexCount;
	}
}