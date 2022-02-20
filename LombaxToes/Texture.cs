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

			if(format == TextureFormat.DXT1)
			{
				//System.Diagnostics.Debug.Assert(width * height == data.Length * 2);
				GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgbS3tcDxt1Ext, width, height, 0, data.Length, data);
			}
			else if (format == TextureFormat.DXT5)
			{
				//System.Diagnostics.Debug.Assert(width * height == data.Length);
				GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgbaS3tcDxt5Ext, width, height, 0, data.Length, data);
			}

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

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