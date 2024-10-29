Shader "Custom/UnlitTransparentWithBaseMapTint"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {} // 텍스처 속성
        _ColorTint ("Color Tint", Color) = (1, 1, 1, 1) // 색상 조정을 위한 속성
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 설정

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            };

            sampler2D _BaseMap;
            fixed4 _ColorTint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // BaseMap 텍스처와 Color Tint를 곱하여 최종 색상 결정
                fixed4 texColor = tex2D(_BaseMap, i.uv);
                return texColor * _ColorTint;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
