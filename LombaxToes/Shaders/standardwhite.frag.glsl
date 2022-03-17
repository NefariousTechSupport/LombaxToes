#version 330

out vec4 colour;

in vec2 UVs;

uniform bool nullTexture;

void main()
{
	colour = vec4(1.0, 1.0, 1.0, 1.0);
}