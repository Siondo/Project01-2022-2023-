Shader "SNKShader/VFX_Effect/Particle_Base" {
    Properties {
		[HDR]_FixColor("FixColor", COLOR) = (1,1,1,1)
		[HDR]_InColor("Interior Color", COLOR) = (1,1,1,1)
        [MaterialToggle] _DoubleFaceColor ("Double Face Color", Float ) = 0.0
        _MainTex ("Main Tex", 2D) = "white" {}
        _Brightness ("Brightness", Float ) = 1
        _Contrast ("Contrast", Float ) = 1
        _MainTexPannerX ("Main Tex Panner X", Float ) = 0
        _MainTexPannerY ("Main Tex Panner Y", Float ) = 0

        [Header(Mesh Depth Setting)]
        [MaterialToggle] _MeshDepth ("Mesh Depth", Float ) = 0
        _MeshDepthIntensity ("Mesh Depth Intensity", Range(0, 1)) = 0

		[Header(Blend Setting)]
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Enum(UnityEngine.Rendering.BlendMode)] SrcBlend ("SrcBlend", Float) = 5    //SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] DstBlend ("DstBlend", Float) = 10   //OneMinusSrcAlpha
		[HideInInspector]_SeparateAlpha("SeparateAlpha",float) = 0
		[HideInInspector]_UIFXClipRange0("", vector) = (-100.0, -100.0, 100.0, 100.0)
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
            Blend [SrcBlend] [DstBlend]
			ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UICLIP_ON
			//SEPARATEALPHA(分离Alpha通道为单张图)
			//#pragma multi_compile __ SEPARATEALPHA
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; 
            uniform float4 _MainTex_ST;
			/*#ifdef SEPARATEALPHA
				uniform sampler2D _MainTex_Alpha;
			#endif*/
			sampler2D _MainTex_Alpha;
			float _SeparateAlpha;
			uniform fixed4 _FixColor;
            uniform fixed _DoubleFaceColor;
			uniform fixed4 _InColor;
            uniform float _Brightness;
            uniform float _Contrast;
            uniform float _MainTexPannerX;
            uniform float _MainTexPannerY;
            uniform float _MeshDepthIntensity;
            uniform fixed _MeshDepth;
			float4 _UIFXClipRange0 = float4(-100.0, -100.0, 100.0, 100.0); // minx maxx miny maxy

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = (TRANSFORM_TEX(v.texcoord0,_MainTex) + half2(_MainTexPannerX,_MainTexPannerY) * _Time.y);
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _MainTex_var = tex2D(_MainTex,i.uv0);
				/*#ifdef SEPARATEALPHA
					_MainTex_var.a = tex2D(_MainTex_Alpha, i.uv0);
				#endif*/ 
				if (_SeparateAlpha)
				{
					_MainTex_var.a = tex2D(_MainTex_Alpha, i.uv0);
				}
                float3 doubleFaceColor = lerp( _FixColor.rgb, lerp(_InColor.rgb,_FixColor.rgb,isFrontFace), _DoubleFaceColor );
                float3 emissive = pow((_Brightness * _MainTex_var.rgb * i.vertexColor.rgb * doubleFaceColor.rgb),_Contrast);
                float vAlpha = (_FixColor.a*(i.vertexColor.a*_MainTex_var.a));
                #if UICLIP_ON
                // Softness factor
                bool inArea = i.posWorld.x >= _UIFXClipRange0.x && i.posWorld.x <= _UIFXClipRange0.z && i.posWorld.y >= _UIFXClipRange0.y && i.posWorld.y <= _UIFXClipRange0.w;
                if(!inArea){
					vAlpha = 0;
                }
                #endif
                float MD = saturate(lerp( vAlpha, (vAlpha*pow(0.5*dot(normalDirection,viewDirection)+0.5,exp2(lerp(1,11,_MeshDepthIntensity)))), _MeshDepth ));
                return fixed4(0.5 * emissive, MD);
            }
            ENDCG
        }
    }
    FallBack "Off"
}
