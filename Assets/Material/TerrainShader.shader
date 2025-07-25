Shader "Custom/TerrainLit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 200
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            //Object local space attributes
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            //Data which is passed to fragment shader, world space
            struct Varying
            {
                float4 positionHCS : SV_POSITION; //Where the vertex ends up on screen
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1; //Position world space
            };

            //Convert vert data from object local space to world space to clip space
            Varying vert(Attributes IN)
            {
                Varying OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            float4 _Color; //Named same as attribute in Properties
            
            half4 frag(Varying IN) : SV_Target
            {
                //Get the main light
                Light mainLight = GetMainLight();

                //Lambert diffuse N * L - Acerola
                float lambert = max(0, dot(IN.normalWS, mainLight.direction));

                //Base Color * Mainlight Color * lambertion diffuse 
                float3 lighting = _Color.rgb * mainLight.color.rgb * lambert;

                return half4(lighting, 1);
            }
            
            ENDHLSL
        }
    }
}
