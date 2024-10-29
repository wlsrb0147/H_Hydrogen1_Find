Shader "Custom/LitTransparentWithBaseMapTint"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {} // 텍스처 속성
        _ColorTint ("Color Tint", Color) = (1, 1, 1, 1) // 색상 조정을 위한 속성
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 설정

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _BaseMap;
        fixed4 _ColorTint;

        struct Input
        {
            float2 uv_BaseMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // BaseMap 텍스처와 Color Tint를 곱하여 최종 색상 결정
            fixed4 texColor = tex2D(_BaseMap, IN.uv_BaseMap) * _ColorTint;
            o.Albedo = texColor.rgb; // 조명 반응을 위해 색상 설정
            o.Alpha = texColor.a; // 투명도 적용
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
