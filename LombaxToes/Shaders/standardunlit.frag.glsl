#version 330

out vec4 colour;

in vec2 UVs;

uniform sampler2D albedo;
uniform bool useTexture;

void main()
{
	if(useTexture)
	{
		colour = texture(albedo, UVs);
		if(colour.a < 0.1f) discard;
	}
	else
	{
		colour = vec4(1.0, 0.0, 1.0, 1.0);
	}
}