

Shader "xinmuShader/VFX_Effect/UV_Effect_Base" {
    Properties {
        _SoftHard ("Soft Hard", Range(0, 0.5) ) = 0.5
        [HDR]_MainColor ("Main Color", Color) = (1,1,1,1)
        [HDR]_InColor("Interior Color", COLOR) = (1,1,1,1)
        [MaterialToggle] _DoubleFaceColor ("Double Face Color", Float ) = 0.0
        _MainTex ("Main Tex", 2D) = "white" {}
        _MainTexBrightness ("Main Tex Brightness", Float ) = 1
        _MainTexPannerX ("Main Tex Panner X", Float ) = 0
        _MainTexPannerY ("Main Tex Panner Y", Float ) = 0
        _MainTexContrast ("Main Tex Contrast", Float ) = 1
        _TurbulenceTex ("Turbulence Tex", 2D) = "black" {}
        _UVEffectPower ("UV Effect Power", Float ) = 0
        _NormalTexPannerX ("Normal Tex Panner X", Float ) = 0
        _NormalTexPannerY ("Normal Tex Panner Y", Float ) = 0
        _Dis ("Dis", Float ) = 1

        [Header(Mesh Depth Setting)]
        [MaterialToggle] _MeshDepth ("Mesh Depth", Float ) = 0
        _MeshDepthIntensity ("Mesh Depth Intensity", Range(0, 1)) = 0

        [Header(Blend Setting)]
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Enum(UnityEngine.Rendering.BlendMode)] SrcBlend ("SrcBlend", Float) = 5    //SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] DstBlend ("DstBlend", Float) = 10  //OneMinusSrcAlpha

        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Name "FORWARD"
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
			//SEPARATEALPHA(����Alphaͨ��Ϊ����ͼ)
			//#pragma multi_compile __ SEPARATEALPHA
            #include "UnityCG.cginc"

            sampler2D _MainTex; 
            float4 _MainTex_ST;
			/*#ifdef SEPARATEALPHA
				uniform sampler2D _MainTex_Alpha;
			#endif*/
			sampler2D _MainTex_Alpha;
			float _SeparateAlpha;
            float _MainTexPannerX;
            float _MainTexPannerY;
            float _MainTexBrightness;
            float _MainTexContrast;
            sampler2D _TurbulenceTex; 
            float4 _TurbulenceTex_ST;
            float _UVEffectPower;
            float _NormalTexPannerX;
            float _NormalTexPannerY;
            float4 _MainColor;
            float4 _InColor;
            float _DoubleFaceColor;
            float _Dis;
            float _MeshDepthIntensity;
            fixed _MeshDepth;
            fixed _SoftHard;
			float4 _UIFXClipRange0 = float4(-100.0, -100.0, 100.0, 100.0); // minx maxx miny maxy
            
            
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
            float4 frag(v2f i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float3 doubleFaceColor = lerp( _MainColor.rgb, lerp(_InColor.rgb,_MainColor.rgb,isFrontFace), _DoubleFaceColor );

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 time1 = _Time; 
                float2 noisePanner = (i.uv0+(float2(_NormalTexPannerX,_NormalTexPannerY)*time1.y));
                float4 _TurbulenceTex_var = tex2D(_TurbulenceTex,TRANSFORM_TEX(noisePanner, _TurbulenceTex));
                //clip((_TurbulenceTex_var.r+_Dis) - 0.5);

                float4 time2 = _Time;
                float2 col = ((_TurbulenceTex_var.r*_UVEffectPower)+(i.uv0+(float2(_MainTexPannerX,_MainTexPannerY)*time2.y)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(col, _MainTex));
				/*#ifdef SEPARATEALPHA
					_MainTex_var.a = tex2D(_MainTex_Alpha, TRANSFORM_TEX(col, _MainTex));
				#endif */
				if (_SeparateAlpha)
				{
					_MainTex_var.a = tex2D(_MainTex_Alpha, TRANSFORM_TEX(col, _MainTex));
				}
                float3 emissive = (doubleFaceColor.rgb*(pow((_MainTexBrightness*_MainTex_var.rgb),_MainTexContrast)*i.vertexColor.rgb));
                float3 finalColor = emissive;
                float vAlpha = (_MainColor.a*(i.vertexColor.a*_MainTex_var.a));
                #if UICLIP_ON
                // Softness factor
                bool inArea = i.posWorld.x >= _UIFXClipRange0.x && i.posWorld.x <= _UIFXClipRange0.z && i.posWorld.y >= _UIFXClipRange0.y && i.posWorld.y <= _UIFXClipRange0.w;
                if(!inArea){
					vAlpha = 0;
                }
                #endif
                float MD =  saturate(lerp( vAlpha, (vAlpha*pow(0.5*dot(normalDirection,viewDirection)+0.5,exp2(lerp(1,11,_MeshDepthIntensity)))), _MeshDepth ));
                float Alpha = saturate(smoothstep( _SoftHard, (1.0 - _SoftHard), saturate( _TurbulenceTex_var.r+_Dis))) * MD;
                return fixed4(0.5 * finalColor,Alpha);
            } 
            ENDCG
        }
    }
    FallBack "Off"
}
