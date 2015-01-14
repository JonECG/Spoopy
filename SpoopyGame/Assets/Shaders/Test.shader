Shader "Custom/SolidColor" {
	Properties {
		_MainTex ("Pattern", 2D) = "white" {}
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_RimPower ("Rim Highlight Power", Range(0.1,10.0)) = 2
	}
    SubShader {
        Pass {
			Tags { "RenderType"="Opaque" }
			LOD 200

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;
			uniform float _RimPower;
		
			uniform float4 _LightColor0;

			struct VertexInput
			{
				float4 vPosition : POSITION;
				float3 normal : NORMAL;
				float2 uv_MainTex : TEXCOORD0;
			};
		
			struct VertexOutput
			{
				float4 fPosition : SV_POSITION;
				float4 color : COLOR;
				float3 normalDir : TEXCOORD0; 
				float4 wPosition : TEXCOORD1;
			};


			VertexOutput vert (VertexInput i)
			{
				VertexOutput o;
				//float4 c = tex2D(_MainTex, i.uv_MainTex);
				o.fPosition = mul( UNITY_MATRIX_MVP, i.vPosition );
				o.color = _Color;// * c;
				o.normalDir = normalize( mul( float4( i.normal, 0.0 ), _World2Object ).xyz );
				o.wPosition = mul( UNITY_MATRIX_MVP, i.vPosition );
				return o;
			}

            float4 frag (VertexOutput o) : COLOR
			{
				float3 normalDirection = o.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - o.wPosition.xyz );
				float3 lightDirection = normalize( _WorldSpaceLightPos0.xyz );
				float atten = 1.0f;
		
				float rim = 1 - saturate( dot( normalize( viewDirection), normalDirection ) );
				float3 rimLighting = atten * _LightColor0.xyz * saturate( dot( normalDirection, lightDirection ) ) * pow( rim, _RimPower );
				return float4( rimLighting, 1.0 );
			}

            ENDCG
        }
    }
}