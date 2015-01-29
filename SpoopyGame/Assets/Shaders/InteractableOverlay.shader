Shader "Custom/SolidColor" {
	Properties {
		_MainTex ("Pattern", 2D) = "white" {}
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_RimPower ("Rim Highlight Power", Range(0.1,10.0)) = 2
		_RimVibrance ("Rim Vibrance", Range(0.0,50.0)) = 1
	}
    SubShader {
        Pass {
			Tags { "Queue"="Transparent" "RenderType"="Transparent" }
			Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;
			uniform float _RimPower;
			uniform float _RimVibrance;
		
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
				float2 vTexPos : TEXCOORD2;
				float3 normalDir : TEXCOORD0; 
				float4 wPosition : TEXCOORD1;
			};


			VertexOutput vert (VertexInput i)
			{
				VertexOutput o;
				o.vTexPos = i.uv_MainTex;
				o.fPosition = mul( UNITY_MATRIX_MVP, i.vPosition );
				o.normalDir = normalize( mul( float4( i.normal, 0.0 ), _World2Object ).xyz );
				o.wPosition = mul( _Object2World, i.vPosition );
				return o;
			}

            float4 frag (VertexOutput o) : COLOR
			{
				float4 texColor = tex2D(_MainTex, 
				float2( o.wPosition.x * 0.3 + o.wPosition.y*0.5 + o.wPosition.z, o.wPosition.x + o.wPosition.y*0.3 + o.wPosition.z*0.5 ) / 2 + _Time.xz / 4);
				float3 normalDirection = normalize( o.normalDir );
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - o.wPosition.xyz );
				float3 lightDirection = normalize( _WorldSpaceLightPos0.xyz );
				float atten = 1.0f;
		
				float rim = 1 - saturate( dot( viewDirection, normalDirection ) );
				float3 rimLighting = _RimVibrance * atten * _LightColor0.xyz * pow( rim, _RimPower ) * _Color;
				//float3 rimLighting = atten * _LightColor0.xyz * saturate( dot( normalDirection, lightDirection ) ) * pow( rim, _RimPower );
				float2 test = o.fPosition.xy / _ScreenParams.xy;
				return float4( ( 1 / ( 1 + length( _WorldSpaceCameraPos.xyz - o.wPosition.xyz )/2 ) ) * ( rimLighting + ( texColor * _Color).xyz * (_CosTime.w/2 + 0.5) ), texColor.x );
			}

            ENDCG
        }
    }
}