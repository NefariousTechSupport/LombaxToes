namespace LombaxToes.Editor
{
	public class Texture
	{
		public int textureId;

		public Texture(byte[] data, TextureFormat format, int width, int height, int mipmaps)
		{
			textureId = GL.GenTexture();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			unsafe
			{
				fixed (byte* b = data)
				{
					uint offset = 0;
					for(int i = 0; i < mipmaps; i++)
					{
						if(format == TextureFormat.DXT1)
						{
							int size = (Math.Max( 1, ((width / (int)Math.Pow(2, i))+3)/4) * Math.Max(1, ((height / (int)Math.Pow(2, i)) +3)/4)) * 8;
							GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbS3tcDxt1Ext, width, height, 0, size, (IntPtr)(b + offset));
							offset += (uint)size;
						}
						else if (format == TextureFormat.DXT5)
						{
							int size = (Math.Max( 1, ((width / (int)Math.Pow(2, i))+3)/4) * Math.Max(1, ((height / (int)Math.Pow(2, i)) +3)/4)) * 16;
							GL.CompressedTexImage2D(TextureTarget.Texture2D, i, InternalFormat.CompressedRgbaS3tcDxt5Ext, width, height, 0, size, (IntPtr)(b + offset));
							offset += (uint)size;
						}
					}					
				}
			}

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void Use()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureId);
		}
	}
}