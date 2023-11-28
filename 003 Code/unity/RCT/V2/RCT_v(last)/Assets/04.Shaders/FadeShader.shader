Shader "Custom/FadeInOutShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "black" { }
    }
    
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            // 중심에서 시작하여 위아래로 투명해지도록 수정
            fixed alpha = 1.0 - abs(IN.uv_MainTex.y - 0.5) * 2.0;
            
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            c.a *= alpha;
            
            o.Albedo = c.rgb;
            o.Alpha = alpha; // 투명도를 새롭게 계산한 alpha로 설정
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
