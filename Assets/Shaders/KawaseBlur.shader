Shader "Custom/KawaseBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 kernelTLTR : TEXCOORD1;
                float4 kernelBLBR : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.kernelTLTR.xy = o.uv + _MainTex_TexelSize.xy * fixed2(_offset+0.5,_offset+0.5);
                o.kernelTLTR.zw = o.uv + _MainTex_TexelSize.xy * fixed2(-_offset-0.5,-_offset-0.5);
                o.kernelBLBR.xy = o.uv + _MainTex_TexelSize.xy * fixed2(_offset+0.5,-_offset-0.5);
                o.kernelBLBR.zw = o.uv + _MainTex_TexelSize.xy * fixed2(-_offset-0.5,_offset+0.5);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0.,0.,0.,1.);
                col += tex2D(_MainTex, i.kernelTLTR.xy);
                col += tex2D(_MainTex, i.kernelTLTR.zw);
                col += tex2D(_MainTex, i.kernelBLBR.xy);
                col += tex2D(_MainTex, i.kernelBLBR.zw);
                return col * 0.25;
            }
            ENDCG
        }
    }
}
