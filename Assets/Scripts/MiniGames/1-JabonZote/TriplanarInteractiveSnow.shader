Shader "Custom/Snow Interactive" {
    Properties{
        [Header(Main)]
    
        
        _Noise("Snow Noise", 2D) = "gray" {}
        
        _NoiseScale("Noise Scale", Range(0,2)) = 0.1
        _NoiseWeight("Noise Weight", Range(0,2)) = 0.1
        [HDR]_ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
 
        [Space]
        [Header(Tesselation)]
        _MaxTessDistance("Max Tessellation Distance", Range(10,100)) = 50
        _Tess("Tessellation", Range(1,500)) = 20
    
 
        [Space]
        [Header(Snow)]
        [HDR]_Color("Snow Color", Color) = (0.5,0.5,0.5,1)
        [HDR]_PathColorIn("Snow Path Color In", Color) = (0.5,0.5,0.7,1)
        [HDR]_PathColorOut("Snow Path Color Out", Color) = (0.5,0.5,0.7,1)
        _PathBlending("Snow Path Blending", Range(0,3)) = 0.3
        _MainTex("Snow Texture", 2D) = "white" {}
        _SnowHeight("Snow Height", Range(0,2)) = 0.3
        _SnowDepth("Snow Path Depth", Range(-2,2)) = 0.3
        _SnowTextureOpacity("Snow Texture Opacity", Range(0,1)) = 0.3
        _SnowTextureScale("Snow Texture Scale", Range(0,2)) = 0.3
        _Normal("Snow Normal", 2D) = "bump" {}
        _SnowNormalStrength("Snow Normal Strength", Range(0,1)) = 0.3
 
        [Space]
        [Header(Sparkles)]
        _SparkleScale("Sparkle Scale", Range(0,10)) = 10
        _SparkCutoff("Sparkle Cutoff", Range(0,2)) = 0.8
        _SparkleNoise("Sparkle Noise", 2D) = "gray" {}
 
        [Space]
        [Header(Rim)]
        _RimPower("Rim Power", Range(0,20)) = 20
        [HDR]_RimColor("Rim Color Snow", Color) = (0.5,0.5,0.5,1)
    }
    HLSLINCLUDE
 
    // Includes
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
    #include "SnowTessellation.hlsl"
    #pragma vertex TessellationVertexProgram
    #pragma hull hull
    #pragma domain domain
    #pragma instancing_options renderinglayer
    // Keywords
    
    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
    #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
    #pragma multi_compile _ _SHADOWS_SOFT
    #pragma multi_compile_fragment _ _LIGHT_LAYERS
    #pragma multi_compile_fog
    #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
 
    ControlPoint TessellationVertexProgram(Attributes2 v)
    {
        ControlPoint p;
        p.vertex = v.vertex;
        p.uv = v.uv;
        p.normal = v.normal;
        p.tangent = v.tangent;
        return p;
    }
    ENDHLSL
 
    SubShader{
        Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
 
        Pass{
            Tags { "LightMode" = "UniversalForward" }
 
            HLSLPROGRAM
            // vertex happens in snowtessellation.hlsl
            #pragma require tessellation tessHW
            #pragma fragment frag
            #pragma target 4.0
            
 
            sampler2D _MainTex, _SparkleNoise;
            float4 _Color, _RimColor;
            float _RimPower;
            float4 _PathColorIn, _PathColorOut;
            float _PathBlending;
            float _SparkleScale, _SparkCutoff;
            float _SnowTextureOpacity, _SnowTextureScale;
            float4 _ShadowColor;
            float _SnowNormalStrength;
            
 
            half4 frag(Varyings2 IN) : SV_Target{
 
                // Effects RenderTexture Reading
                float3 worldPosition = mul(unity_ObjectToWorld, IN.vertex).xyz;
                float2 uv = IN.worldPos.xz - _Position.xz;
                uv /= (_OrthographicCamSize * 2);
                uv += 0.5;
 
                // effects texture              
                float4 effect = tex2D(_GlobalEffectRT, uv);
 
                // mask to prevent bleeding
                effect *=  smoothstep(0.99, 0.9, uv.x) * smoothstep(0.99, 0.9,1- uv.x);
                effect *=  smoothstep(0.99, 0.9, uv.y) * smoothstep(0.99, 0.9,1- uv.y);
 
                
                // worldspace Snow texture
                float3 snowtexture = tex2D(_MainTex, IN.worldPos.xz * _SnowTextureScale).rgb;
 
                // snow normal
                float3 snownormal = UnpackNormal(tex2D(_Normal, IN.worldPos.xz * _NoiseScale)).rgb;
                snownormal = snownormal.r * IN.tangent + snownormal.g * IN.bitangent + snownormal.b * IN.normal;
                
                //lerp between snow color and snow texture
                float3 snowTex = lerp(_Color.rgb,snowtexture * _Color.rgb, _SnowTextureOpacity);
                
                
                //lerp the colors using the RT effect path 
                float3 path = lerp(_PathColorOut.rgb * effect.g, _PathColorIn.rgb, saturate(effect.g * _PathBlending));
                float3 mainColors = lerp(snowTex,path, saturate(effect.g));
 
                // lighting and shadow information
                float shadow = 0;
                half4 shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                
                #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
                    Light mainLight = GetMainLight(shadowCoord);
                    shadow = mainLight.shadowAttenuation;
                #else
                    Light mainLight = GetMainLight();
                #endif
 
                // extra point lights support
                float3 extraLights;
                int pixelLightCount = GetAdditionalLightsCount();
                for (int j = 0; j < pixelLightCount; ++j) {
                    Light light = GetAdditionalLight(j, IN.worldPos, half4(1, 1, 1, 1));
                    float3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
                    extraLights += attenuatedLightColor;            
                }
 
                float4 litMainColors = float4(mainColors,1) ;
                extraLights *= litMainColors.rgb;
                // add in the sparkles
                float sparklesStatic = tex2D(_SparkleNoise, IN.worldPos.xz * _SparkleScale).r;
                float cutoffSparkles = step(_SparkCutoff,sparklesStatic);               
                litMainColors += lerp(cutoffSparkles * 4, 0,saturate(effect.g * 2));
                
                
                // add rim light
                half rim = 1.0 - dot(lerp(snownormal, IN.normal, _SnowNormalStrength), normalize(IN.viewDir)) ;
                // no rim inside of the path
                rim = lerp(rim, 0, saturate(effect.g));
                litMainColors += _RimColor * pow(rim, _RimPower);
 
                // ambient and mainlight colors added
                half4 extraColors;
                extraColors.rgb = litMainColors.rgb * mainLight.color.rgb * (shadow + unity_AmbientSky.rgb);
                extraColors.a = 1;
                
                // colored shadows
                float3 coloredShadows = shadow + lerp(_ShadowColor, 0, shadow);
                litMainColors.rgb = litMainColors.rgb * mainLight.color * (coloredShadows);
                // everything together
                float4 final = litMainColors+ extraColors + float4(extraLights,0);
                // add in fog
                final.rgb = MixFog(final.rgb, IN.fogFactor);
                return final;
 
            }
            ENDHLSL
 
        }
 
 
        // depth only pass to fix invisiblity when turning on Depth Priming mode
           Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
 
            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull[_Cull]
 
            HLSLPROGRAM
            #pragma target 2.0
 
            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
 
            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
 
            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LOD_FADE_CROSSFADE
 
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
 
            // -------------------------------------
            // Includes
            //#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
 
        // casting shadows is a little glitchy, I've turned it off, but maybe in future urp versions it works better?
        // Shadow Casting Pass
        Pass
        {
                Name "ShadowCaster"
                Tags { "LightMode" = "ShadowCaster" }
                ZWrite On
            ZTest LEqual
            ColorMask 0
                HLSLPROGRAM
                #pragma target 3.0
            
                // Support all the various light  ypes and shadow paths
                #pragma multi_compile_shadowcaster
                // Unity defined keywords
                #pragma multi_compile _ LOD_FADE_CROSSFADE
                   // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
                   #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
                // Register our functions
            
                #pragma fragment frag
                // A custom keyword to modify logic during the shadow caster pass
 
                half4 frag(Varyings2 IN) : SV_Target{
                        return 0;
                }
            
                ENDHLSL
        }
    }
}