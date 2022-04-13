#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 5) in mat4 aWorld;

out vec2 UVs;

void main()
{
	UVs = aTexCoord;
	gl_Position = vec4(aPosition, 1.0) * aWorld;
}