Shader "Custom/DissolveTesty"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTexture("Texture", 2D) = "white" {}
        _DissolveTexture("Texture", 2D) = "white" {}
        _DissolveCutoff("Dissolve Cutoff", Range(0, 1)) = 0
        _Speed("Speed", Range(-10, 10)) = 0
        _Rate("Rate", Range(-10, 10)) = 0
        _Scale("Scale", Range(-10, 10)) = 0
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            //include external files
            #include "UnityCG.cginc"

            struct VertexToFragment
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uvDissolve : TEXCOORD1;
            };

            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            //declare
            float4 _Color;
            sampler2D _MainTexture, _DissolveTexture; 
            float4 _MainTexture_ST, _DissolveTexture_ST;
            float _DissolveCutoff;
            float _Speed, _Rate, _Scale;


            //float4 represents position
            VertexToFragment MyVertexProgram(VertexData vert)
            {
                VertexToFragment v2f;
                //Cool to change position
                // vert.position.x += sin(_Time.y * 3 + vert.position.y);
                vert.position.x += sin((_Time.y * _Rate) + (vert.position.y * _Scale)) * _Speed * cos((_Time.y * _Rate) + (vert.position.y * _Scale));
                //convert from world coordinates to camera space
                v2f.position = UnityObjectToClipPos(vert.position);
                v2f.uv = vert.uv * _MainTexture_ST.xy + _MainTexture_ST.zw;
                v2f.uvDissolve = vert.uv * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
                return v2f;
            }

            //float4 represents color
            float4 MyFragmentProgram(VertexToFragment v2f) : SV_TARGET
            {
                float4 color = tex2D(_MainTexture, v2f.uv) * _Color;
                float4 dissolveColor = tex2D(_DissolveTexture, v2f.uvDissolve);
                clip(dissolveColor - _DissolveCutoff);
                // color *= tex2D(_DissolveTexture, v2f.uvDissolve) * unity_ColorSpaceDouble;
                // color *= tex2D(_MainTexture, v2f.uv * 5);
                // color *= tex2D(_MainTexture, v2f.uv * 25);
                // color *= tex2D(_MainTexture, v2f.uv * 100);
                return color;
            }
            ENDCG
        }
    }
}

// Shader "Custom/TheFirstShaderino"
// {
//     Properties
//     {
//         _Color("Color", Color) = (1, 1, 1, 1)
//         _Texture("Texture", 2D) = "Tex"
//     }

//     SubShader
//     {
//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex MyVertexProgram
//             #pragma fragment MyFragmentProgram

//             //include external files
//             #include "UnityCG.cginc"

//             struct VertexToFragment
//             {
//                 float4 position : POSITION;
//                 float4 localPosition : TEXCOORD0;
//             };

//             struct VertexData
//             {
//                 float4 position : SV_POSITION;
//                 float4 localPosition : TEXCOORD0;
//             };
            
//             //declare
//             float4 _Color;
//             float4 _Texture;

//             //float4 represents position
//             VertexToFragment MyVertexProgram(float4 position : POSITION)
//             {
//                 VertexToFragment v2f;
//                 //convert from world coordinates to camera space
//                 v2f.position = UnityObjectToClipPos(position);
//                 v2f.localPosition = position;
//                 return v2f;
//             }

//             //float4 represents color
//             float4 MyFragmentProgram(VertexToFragment v2f) : SV_TARGET
//             {
//                 return float4(v2f.localPosition.xyz, 1) * _Color;
//             }
//             ENDCG
//         }
//     }
// }
