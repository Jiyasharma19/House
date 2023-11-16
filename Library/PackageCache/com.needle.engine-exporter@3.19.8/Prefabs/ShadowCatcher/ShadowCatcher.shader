// Thanks, as usual, to bgolus for sharing
// https://forum.unity.com/threads/water-shader-graph-transparency-and-shadows-universal-render-pipeline-order.748142/#post-5518747
// BiRP based on https://github.com/keijiro/ShadowDrawer/blob/master/Assets/ShadowDrawer.shader
Shader "Needle/Shadow Catcher"
{
    Properties
    {
        // _ShadowColor ("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
    }
 
    SubShader
    {
        PackageRequirements
        {
            "com.unity.render-pipelines.universal": "10.0"
        }

        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent-1"
        }
 
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
 
            Blend DstColor Zero, Zero One
            Cull Back
            ZTest LEqual
            ZWrite Off
   
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
 
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog
 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 
            CBUFFER_START(UnityPerMaterial)
            float4 _ShadowColor;
            CBUFFER_END
 
            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct Varyings
            {
                float4 positionCS               : SV_POSITION;
                float3 positionWS               : TEXCOORD0;
                float fogCoord                  : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
 
            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;
 
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
 
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
 
                return output;
            }
 
            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
 
                half4 color = half4(1,1,1,1);
 
            #ifdef _MAIN_LIGHT_SHADOWS
                VertexPositionInputs vertexInput = (VertexPositionInputs)0;
                vertexInput.positionWS = input.positionWS;
 
                float4 shadowCoord = GetShadowCoord(vertexInput);
                half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
                color = lerp(half4(1,1,1,1), _ShadowColor, (1.0 - shadowAttenutation) * _ShadowColor.a);
                color.rgb = MixFogColor(color.rgb, half3(1,1,1), input.fogCoord);
            #endif
                
                return color;
            }
 
            ENDHLSL
        }
    }
    
    SubShader
    {
        Tags { "Queue"="AlphaTest+49" }

        CGINCLUDE

        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "UnityLightingCommon.cginc"

        struct v2f_shadow {
            float4 pos : SV_POSITION;
            fixed3 diff : COLOR0;
            LIGHTING_COORDS(0, 1)
        };

        half4 _Color;

        v2f_shadow vert_shadow(appdata_full v)
        {
            v2f_shadow o;
            o.pos = UnityObjectToClipPos(v.vertex);
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            TRANSFER_VERTEX_TO_FRAGMENT(o);
            o.diff = ShadeSH9(half4(worldNormal,1));
            return o;
        }

        half4 frag_shadow(v2f_shadow IN) : SV_Target
        {
            half atten = LIGHT_ATTENUATION(IN);
            return half4(0,0,0, lerp(1, 0, atten)) + half4(IN.diff.rgb, 0) * 0.05; // some approximated tinting
        }

        ENDCG
        
        // Depth fill pass
        Pass
        {
            ColorMask 0

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                return o;
            }

            half4 frag(v2f IN) : SV_Target
            {
                return (half4)1;
            }

            ENDCG
        }

        // Forward base pass
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_fwdbase
            ENDCG
        }

        /* only for additional lights; we only support 1 directional light at runtime right now
        // Forward add pass
        Pass
        {
            Tags { "LightMode" = "ForwardAdd" }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_fwdadd_fullshadows
            ENDCG
        }
        */
    }
    
    FallBack "Mobile/VertexLit"
}