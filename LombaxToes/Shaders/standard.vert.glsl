#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 UVs;

//Uncomment both things to enable instancing

uniform mat4 transform;
//uniform mat4 transforms[256];

void main()
{
	UVs = aTexCoord;
	//gl_Position = vec4(aPosition, 1.0) * transforms[gl_InstanceID];
	gl_Position = vec4(aPosition, 1.0) * transform;
}