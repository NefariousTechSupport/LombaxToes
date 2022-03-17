#version 330 core

out vec4 colour;

in vec2 UVs;

uniform sampler2D albedo;
uniform bool useTexture;
uniform sampler2D noTextureTexture;

void main()
{
	if(useTexture)
	{
		//vec4 texel = texture(albedo, UVs);
		colour = texture(albedo, UVs);
	}
	else
	{
		//vec4 texel = texture(noTextureTexture, UVs);
		colour = vec4(1.0, 0.0, 1.0, 1.0);
	}
}