using System.Text;

namespace LibLombaxToes.IO
{
	//Heavily edited and renamed version of https://github.com/AdventureT/TrbMultiTool/blob/opengl/TrbMultiTool/TrbMultiTool/EndiannessAwareBinaryReader.cs

	public class StreamHelper : BinaryReader
	{
		public enum Endianness
		{
			Little,
			Big,
		}

		public Endianness _endianness = Endianness.Little;

		public StreamHelper(Stream input) : base(input)
		{
		}

		public StreamHelper(Stream input, Encoding encoding) : base(input, encoding)
		{
		}

		public StreamHelper(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
		{
		}

		public StreamHelper(Stream input, Endianness endianness) : base(input)
		{
			_endianness = endianness;
		}

		public StreamHelper(Stream input, Encoding encoding, Endianness endianness) : base(input, encoding)
		{
			_endianness = endianness;
		}

		public StreamHelper(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness) : base(input, encoding, leaveOpen)
		{
			_endianness = endianness;
		}

		public override string ReadString()
		{
			var sb = new StringBuilder();
			while (true)
			{
				var newByte = ReadByte();
				if (newByte == 0) break;
				sb.Append((char)newByte);
			}
			return sb.ToString();
		}

		public string ReadUnicodeString()
		{
			var sb = new StringBuilder();
			while (true)
			{
				byte newByte;
				byte newByte2;

				try
				{
					newByte = ReadByte();
					newByte2 = ReadByte();
				}
				catch (EndOfStreamException)
				{
					break;
				}
				if (newByte == 0 && newByte2 == 0) break;
				string convertedChar;
				if (_endianness == Endianness.Big) convertedChar = Encoding.Unicode.GetString(new byte[] { newByte2, newByte });
				else convertedChar = Encoding.Unicode.GetString(new byte[] { newByte, newByte2 });
				sb.Append(convertedChar);
			}
			return sb.ToString();
		}

		public string ReadUnicodeString(uint size)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				byte newByte;
				byte newByte2;

				try
				{
					newByte = ReadByte();
					newByte2 = ReadByte();
				}
				catch (EndOfStreamException)
				{
					break;
				}
				if (newByte == 0 && newByte2 == 0) break;
				string convertedChar;
				if (_endianness == Endianness.Big) convertedChar = Encoding.Unicode.GetString(new byte[] { newByte2, newByte });
				else convertedChar = Encoding.Unicode.GetString(new byte[] { newByte, newByte2 });
				sb.Append(convertedChar);
			}
			return sb.ToString();
		}

		public new byte ReadByte() => ReadByte((uint)BaseStream.Position);
		public byte ReadByte(uint offset)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			byte[] buffer = new byte[1];
			BaseStream.Read(buffer, 0x00, 0x01);
			return buffer[0];
		}

		public string ReadStringFromOffset(uint offset)
		{
			var pos = BaseStream.Position;
			BaseStream.Seek(offset, SeekOrigin.Begin);
			string str = ReadString();
			BaseStream.Seek(pos, SeekOrigin.Begin);
			return str;
		}

		public byte[] ReadFromOffset(uint bytesToRead, uint offset)
		{
			var pos = BaseStream.Position;
			BaseStream.Seek(offset, SeekOrigin.Begin);
			var buffer = new byte[bytesToRead];
			var bytesRead = Read(buffer, 0x00, (int)bytesToRead);
			BaseStream.Seek(pos, SeekOrigin.Begin);
			return buffer;
		}
		public unsafe T ReadStruct<T>()
		{
			byte[] data = ReadBytes(Marshal.SizeOf(typeof(T)));

			foreach (var field in typeof(T).GetFields())
			{
				var fieldType = field.FieldType;
				if (field.IsStatic)
					// don't process static fields
					continue;

				if (fieldType == typeof(string)) 
					// don't swap bytes for strings
					continue;

				var offset = Marshal.OffsetOf(typeof(T), field.Name).ToInt32();

				// handle enums
				if (fieldType.IsEnum)
					fieldType = Enum.GetUnderlyingType(fieldType);

				// check for sub-fields to recurse if necessary
				var subFields = fieldType.GetFields().Where(subField => subField.IsStatic == false).ToArray();

				var effectiveOffset = offset;

				if (subFields.Length == 0)
				{
					Array.Reverse(data, effectiveOffset, Marshal.SizeOf(fieldType));
				}
			}

	        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			T read = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),typeof(T));
			handle.Free();

			return read;
		}
		public override float ReadSingle() => ReadSingle(_endianness);
		public float ReadSingle(Endianness endianness) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), endianness), 0);
		public override Half ReadHalf() => ReadHalf(_endianness);
		public Half ReadHalf(Endianness endianness) => BitConverter.ToHalf(ReadForEndianness(sizeof(float) / 2, endianness), 0);
		public override short ReadInt16() => ReadInt16(_endianness);
		public short ReadInt16(uint offset, Endianness endianness)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			return ReadInt16(endianness);
		}
		public override int ReadInt32() => ReadInt32(_endianness);
		public int ReadInt32WithOffset(uint offset)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			return ReadInt32(_endianness);
		}

		public override ushort ReadUInt16() => ReadUInt16(_endianness);
		public uint ReadUInt16WithOffset(uint offset) => ReadUInt16WithOffset(offset, _endianness);
		public uint ReadUInt16WithOffset(uint offset, Endianness endianness)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			return ReadUInt16(endianness);
		}
		public override uint ReadUInt32() => ReadUInt32(_endianness);
		public uint ReadUInt32WithOffset(uint offset) => ReadUInt32WithOffset(offset, _endianness);
		public uint ReadUInt32WithOffset(uint offset, Endianness endianness)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			return ReadUInt32(endianness);
		}
		public override ulong ReadUInt64() => ReadUInt64(_endianness);

		public short ReadInt16(Endianness endianness) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), endianness), 0);

		public int ReadInt32(Endianness endianness) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), endianness), 0);

		public override long ReadInt64() => ReadInt64(_endianness);
		public long ReadInt64(Endianness endianness) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), endianness), 0);

		public ushort ReadUInt16(Endianness endianness) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), endianness), 0);

		public uint ReadUInt32(Endianness endianness) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), endianness), 0);

		public ulong ReadUInt64(Endianness endianness) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), endianness), 0);

		public void WriteUInt32WithOffset(uint dat, uint offset)
		{
			BaseStream.Seek(offset, SeekOrigin.Begin);
			WriteUInt32(dat);
		}
		public void WriteUInt32(uint dat) => WriteUInt32(dat, _endianness);
		public void WriteUInt32(uint dat, Endianness endianness)
		{
			if((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
			{
				BaseStream.Write(BitConverter.GetBytes(dat).Reverse().ToArray(), 0x00, 0x04);
			}
			else
			{
				BaseStream.Write(BitConverter.GetBytes(dat), 0x00, 0x04);
			}
		}

		public void WriteString(string dat) => BaseStream.Write(Encoding.ASCII.GetBytes(dat), 0x00, dat.Length);

		public byte[] ReadForEndianness(int bytesToRead, Endianness endianness)
		{
			byte[] bytesRead = new byte[bytesToRead];
			int res = BaseStream.Read(bytesRead, 0, bytesToRead);
			if(res != bytesToRead)
			{
				throw new Exception($"Read {res} instead of {bytesToRead}");
			}
			switch (endianness)
			{
				case Endianness.Little:
					if (!BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytesRead);
					}
					break;

				case Endianness.Big:
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytesRead);
					}
					break;
			}

			return bytesRead;
		}
	}
}