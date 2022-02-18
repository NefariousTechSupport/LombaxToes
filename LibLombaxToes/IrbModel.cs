using System.Text;

namespace LibLombaxToes
{
	// This is basically entirely lifted from the work of Gh0stblade and chroxx

	public class IrbModel : IGHW
	{
		int type = -1;		//0 is Moby, 1 is Tie, -1 means no mesh
		MeshMetadata[] metadatas;
		float meshScaleX;
		float meshScaleY;
		float meshScaleZ;
		public int meshCount;
		public string path;

		public ulong[] shaderTuids;

		public IrbModel(Stream input) : base(input)
		{
			ReadMeshMetadata();
			ReadMeshScale();
			
			IGHWSectionHeader shaderTuidsSection = GetSectionHeader(IGHWSectionIdentifier.MeshShaders);
			sh.BaseStream.Seek(shaderTuidsSection.offset, SeekOrigin.Begin);
			shaderTuids = new ulong[(int)shaderTuidsSection.itemCount & ~0x10000000];
			for(int i = 0; i < shaderTuids.Length; i++)
			{
				shaderTuids[i] = sh.ReadUInt64();
			}

			ReadMeshPath();
		}

		void ReadMeshPath()
		{
			IGHWSectionHeader pathSection;
			if(type == 0)
			{
				pathSection = GetSectionHeader(IGHWSectionIdentifier.MobyFilePath);
			}
			else
			{
				pathSection = GetSectionHeader(IGHWSectionIdentifier.TieFilePath);
			}
			path = sh.ReadStringFromOffset(pathSection.offset);
		}

		void ReadMeshScale()
		{
			if(type == 0)
			{
				IGHWSectionHeader mobyScaleSection = GetSectionHeader(IGHWSectionIdentifier.MobyScale);
				sh.BaseStream.Seek(mobyScaleSection.offset + 0x70, SeekOrigin.Begin);
				//meshScaleX = meshScaleY = meshScaleZ = BitConverter.ToSingle(BitConverter.GetBytes(sh.ReadUInt32() + 0x07800000));
				meshScaleX = meshScaleY = meshScaleZ = BitConverter.ToSingle(BitConverter.GetBytes(sh.ReadUInt32() + 0x06800000));
			}
			else if (type == 1)
			{
				IGHWSectionHeader mobyScaleSection = GetSectionHeader(IGHWSectionIdentifier.TieScale);
				sh.BaseStream.Seek(mobyScaleSection.offset + 0x20, SeekOrigin.Begin);
				//meshScaleX = sh.ReadSingle();
				//meshScaleY = sh.ReadSingle();
				//meshScaleZ = sh.ReadSingle();
				meshScaleX = meshScaleY = meshScaleZ = 1;
			}
		}
		void ReadMeshMetadata()
		{
			if(sections.Any(x => x.identifier == IGHWSectionIdentifier.MobyMetadata))
			{
				type = 0;

				IGHWSectionHeader mobyMetadataSection = GetSectionHeader(IGHWSectionIdentifier.MobyMetadata);
				meshCount = (int)mobyMetadataSection.itemCount & ~0x10000000;
				metadatas = new MeshMetadata[meshCount];

				for(int i = 0; i < meshCount; i++)
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

				IGHWSectionHeader tieMetadataSection = GetSectionHeader(IGHWSectionIdentifier.TieMetadata);
				meshCount = (int)tieMetadataSection.itemCount & ~0x10000000;
				metadatas = new MeshMetadata[meshCount];

				for(int i = 0; i < meshCount; i++)
				{
					sh.BaseStream.Seek(tieMetadataSection.offset + i*0x40, SeekOrigin.Begin);

					metadatas[i].indicesStart = sh.ReadUInt32();
					metadatas[i].verticesStart = sh.ReadUInt16();
					sh.ReadUInt16();
					metadatas[i].vertexCount = sh.ReadUInt16();
					sh.ReadByte();
					sh.ReadByte();
					metadatas[i].type = sh.ReadByte();
					sh.ReadByte();
					sh.ReadByte();
					sh.ReadByte();

					sh.ReadUInt16();
					metadatas[i].indexCount = sh.ReadUInt16();
					sh.ReadUInt32();
					sh.ReadUInt32();

					metadatas[i].boneMapOffset = sh.ReadUInt32();
					sh.ReadUInt32();
				}
			}
			else
			{
				type = -1;
			}
		}

		public void ExportToObj(Stream output, int index)
		{
			uint currentIndex = 0;

			output.Write(Encoding.ASCII.GetBytes($"g\nmtllib untitled.mtl\n"));

			for(int i = 0; i < meshCount; i++)
			{
				float[] vertices = ReadVertexBuffer(i);
				uint[] indices = ReadIndexBuffer(i);

				
				for(int j = 0; j < vertices.Length; j += 0x05)
				{
					output.Write(Encoding.ASCII.GetBytes($"v {vertices[j + 0].ToString("F")} {vertices[j + 1].ToString("F")} {vertices[j + 2].ToString("F")}\n"));
					output.Write(Encoding.ASCII.GetBytes($"vt {vertices[j + 3].ToString("F")} {vertices[j + 4].ToString("F")}\n"));
				}

				output.Write(Encoding.ASCII.GetBytes($"g Mesh_{i}\nusemtl Mesh_{i}\n"));

				for(int j = 0; j < indices.Length; j += 3)
				{
					output.Write(Encoding.ASCII.GetBytes($"f {currentIndex + indices[j + 0]+1}/{currentIndex + indices[j + 0]+1} {currentIndex + indices[j + 1]+1}/{currentIndex + indices[j + 1]+1} {currentIndex + indices[j + 2]+1}/{currentIndex + indices[j + 2]+1}\n"));
				}

				currentIndex += indices.Max() + 1;

				output.Write(Encoding.ASCII.GetBytes($"g\n"));
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

				vertices[i * 5 + 0] = (sh.ReadInt16() / (float)0xFFFF) * meshScaleX;
				vertices[i * 5 + 1] = (sh.ReadInt16() / (float)0xFFFF) * meshScaleY;
				vertices[i * 5 + 2] = (sh.ReadInt16() / (float)0xFFFF) * meshScaleZ;

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
		public ulong GetShaderTuid(int index)
		{
			return shaderTuids[metadatas[index].shaderIndex];
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