Shader "Unlit/Blur"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _BlurSize("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"
        }
        LOD 100
        
        Cull Off
        Lighting On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _Cutoff;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                clip(col.a - _Cutoff);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };


            struct v2f

            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };


            sampler2D _MainTex;
            float _BlurSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 color = half4(0, 0, 0, 0);
                float2 uv = i.uv;
                float blurSize = _BlurSize * 0.01;
                float2 texelSize = float2(blurSize, blurSize);

                color += tex2D(_MainTex, uv + float2(-texelSize.x, -texelSize.y)) * 0.111;
                color += tex2D(_MainTex, uv + float2(0.0, -texelSize.y)) * 0.111;
                color += tex2D(_MainTex, uv + float2(texelSize.x, -texelSize.y)) * 0.111;
                color += tex2D(_MainTex, uv + float2(-texelSize.x, 0.0)) * 0.111;
                color += tex2D(_MainTex, uv) * 0.111;
                color += tex2D(_MainTex, uv + float2(texelSize.x, 0.0)) * 0.111;
                color += tex2D(_MainTex, uv + float2(-texelSize.x, texelSize.y)) * 0.111;
                color += tex2D(_MainTex, uv + float2(0.0, texelSize.y)) * 0.111;
                color += tex2D(_MainTex, uv + float2(texelSize.x, texelSize.y)) * 0.111;

                return color;
            }
            ENDCG

        }

    }
    Fallback "Diffuse"
}