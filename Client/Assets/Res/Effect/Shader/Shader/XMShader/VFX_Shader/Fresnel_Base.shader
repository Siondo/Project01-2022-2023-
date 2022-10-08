Shader "SNKShader/VFX_Effect/Fresnel_Base" {
    Properties {
        [HDR]_MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _Brightness ("Brightness", Float ) = 1
        _Contrast ("Contrast", Float ) = 1
        _MainTexPannerX ("Main Tex Panner X", Float ) = 0
        _MainTexPannerY ("Main Tex Panner Y", Float ) = 0
        _FrenselColor ("Frensel Color", Color) = (1,1,1,1)
        _FrenselValue ("Frensel Value", Float ) = 1
        _FresnelBrightness ("Fresnel Brightness", Float ) = 1
        [MaterialToggle] _FresnelMultiplyAlpha ("Fresnel Multiply Alpha", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		[HideInInspector]_SeparateAlpha("SeparateAlpha",float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Tags {
                "LightMode"="Always"
            }
            Blend SrcAlpha OneMinusSrcAlpha            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			//SEPARATEALPHA(分离Alpha通道为单张图)
			//#pragma multi_compile __ SEPARATEALPHA

            #include "UnityCG.cginc"

            sampler2D _MainTex; 
            float4 _MainTex_ST;
			sampler2D _MainTex_Alpha;
			float _SeparateAlpha;
			/*#ifdef SEPARATEALPHA
				uniform sampler2D _MainTex_Alpha;
			#endif*/
			fixed _MainTexPannerX;
			fixed _MainTexPannerY;
			fixed _FresnelBrightness;
			fixed _FrenselValue;
			fixed _Brightness;
            fixed _FresnelMultiplyAlpha;
            fixed4 _MainColor;
            fixed _Contrast;
            fixed4 _FrenselColor;
            struct appdate {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            v2f vert (appdate v) {
                v2f o = (v2f)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(v2f i) : SV_Target {
				clip(i.posWorld.y);
                half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				half3 normalDirection = i.normalDir;
                float4 time = _Time;
                half2 panner = (i.uv0+(float2(_MainTexPannerX,_MainTexPannerY)*time.g));
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(panner, _MainTex));
				/*#ifdef SEPARATEALPHA
					_MainTex_var.a = tex2D(_MainTex_Alpha, TRANSFORM_TEX(panner, _MainTex));
				#endif */  
				if(_SeparateAlpha)
				{
					_MainTex_var.a = tex2D(_MainTex_Alpha, TRANSFORM_TEX(panner, _MainTex));
				}
				half power = pow(1.0-max(0,dot(normalDirection, viewDirection)),_FrenselValue);
                half3 emissive = (_MainColor.rgb*(pow((_Brightness*(_MainTex_var.rgb+((_FresnelBrightness*power)*_FrenselColor.rgb))),_Contrast)*i.vertexColor.rgb));
				//DoubleHDR
				return fixed4(0.5 * emissive, saturate((_MainColor.a*(i.vertexColor.a*lerp(_MainTex_var.a, (_MainTex_var.a*power), _FresnelMultiplyAlpha)))));
            }
            ENDCG
        }
    }
	Fallback Off
}
