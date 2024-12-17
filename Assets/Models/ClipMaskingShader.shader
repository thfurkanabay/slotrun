Shader "Custom/ClipMaskingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MinHeight ("Min Height", Float) = 0.0
        _MaxHeight ("Max Height", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos; // Dünya pozisyonunu almak için
        };

        sampler2D _MainTex;
        float _MinHeight;
        float _MaxHeight;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Yükseklik sınırlandırması
            if (IN.worldPos.y < _MinHeight || IN.worldPos.y > _MaxHeight)
            {
                clip(-1); // Belirtilen aralığın dışındaki pikselleri maskele
            }

            // Geriye kalan kısmı normal şekilde render et
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent"
}