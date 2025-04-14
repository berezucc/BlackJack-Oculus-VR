Shader "PolygonExperts/GridInterpolationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _EmissionTex ("Emission Texture", 2D) = "white" { }
        _GridNumbers ("Grid Numbers", Range(1, 1024)) = 1
        _EmissionStrength ("Emission Strength", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _EmissionTex;
        fixed _GridNumbers;
        fixed _EmissionStrength;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Calculate grid coordinates for both main texture and emission texture
            fixed2 gridCoords = floor(IN.uv_MainTex * _GridNumbers) / _GridNumbers;
            
            // Interpolate within each grid area for the main texture
            o.Albedo = tex2D(_MainTex, gridCoords);

            // Interpolate within each grid area for the emission texture
            fixed3 emissionColor = tex2D(_EmissionTex, gridCoords).rgb;

            // Apply emissive color
            o.Emission = _EmissionStrength * emissionColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
