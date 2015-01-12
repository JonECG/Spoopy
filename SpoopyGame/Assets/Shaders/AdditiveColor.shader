Shader "Custom/AdditiveColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1)
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Blend OneMinusDstColor One
		Pass {Color [_Color]}
	}
}
