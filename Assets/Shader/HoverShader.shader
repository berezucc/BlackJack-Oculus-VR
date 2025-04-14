Shader "Unlit/HoverShader"
{
    Properties
    {
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
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sin function oscillates over Time.y (seconds of game) between range of 0.0-1.0 (thats why 0.5 + 0.5 * ...)
                float intensity = 0.5 + 0.5 * sin(_Time.y * 10.0);

                // Lerp intensity between 0.5 and 1.0 brightness for colour (to have flashing it needs to change intensities)
                intensity = lerp(0.5, 1.0, intensity);
    
                // Apply the calculated yellow colour (since yellow rgba is 1,1,0,1 --> r and g values are replaced with intensity to change shade of yellow)
                fixed4 col = fixed4(intensity, intensity, 0, 1); 

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
