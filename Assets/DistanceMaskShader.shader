Shader "Custom/DistanceMaskShader"
{
    Properties
    {
        _InsideColor("Inside Color", Color) = (1,1,1,1)
        _OutsideColor("Outside Color", Color) = (0.2,0.2,0.2,1)
        _Radius("Radius", Float) = 5
        _Softness("Edge Softness", Float) = 1
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _InsideColor;
            float4 _OutsideColor;
            float _Radius;
            float _Softness;

            float3 _PlayerPos; // Set globally from C#

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.worldPos, _PlayerPos);

            // Smooth transition at edge
            float t = saturate((dist - _Radius) / _Softness);

            return lerp(_InsideColor, _OutsideColor, t);
        }
        ENDHLSL
    }
    }
}
