Shader "Custom/FX_ParticleDissolveUseCustomData"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        [HDR]_Color("Base Color", Color) = (1, 1, 1, 1)
        [Toggle(_MAIN_FLOW_CUSTOMDATA)] _Main_Flow_CustomData("Use Main Flow By Customdata?" , int) = 0
        [Toggle(_MAIN_POLAR_COORDINATE)] _Main_Polar_Coordiante("Use Main Polar Coordinate?" , int) = 0
        _mAngle ("Main Angle", Range(-3.14, 3.14)) = 0.0
        _mSpeed("Main Flow Speed",Range(0,1)) = 0
        _mX("Main Flow X",Range(-1,1)) = 0
        _mY("Main Flow Y",Range(-1,1)) = 0
        
        [Space(20)]
        [Toggle(_USE_ALPHA)] _Use_Alpha("Use Alpha?" , int) = 0
        _AlphaTex("Alpha Texture", 2D) = "white" {}
        [Toggle(_ALPHA_FLOW_CUSTOMDATA)] _Alpha_Flow_CustomData("Use Alpha Flow By Customdata?" , int) = 0
        [Toggle(_ALPHA_POLAR_COORDINATE)] _Alpha_Polar_Coordiante("Use Alpha Polar Coordinate?" , int) = 0  
        _aAngle ("Alpha Angle", Range(-3.14, 3.14)) = 0.0
        _aSpeed("Alpha Flow Speed",Range(0,1)) = 0
        _aX("Alpha Flow X",Range(-1,1)) = 0
        _aY("Alpha Flow Y",Range(-1,1)) = 0
        
        [Space(20)]
        [Toggle(_USE_DISSOLVE)] _Use_Dissolve("Use Dissolve?" , int) = 0
        _DisolveTex("Dissolve Texture", 2D) = "white" {}
        [Toggle(_DISSOLVE_FLOW_CUSTOMDATA)] _Dissolve_Flow_CustomData("Use Dissolve Flow By Customdata?" , int) = 0 
        [Toggle(_DISSOLVE_POLAR_COORDINATE)] _Dissolve_Polar_Coordiante("Use Dissolve Polar Coordinate?" , int) = 0   
        _dAngle ("Dissolve Angle", Range(-3.14, 3.14)) = 0.0
        _dSpeed("Dissolve Flow Speed",Range(0,1)) = 0
        _dX("Dissolve Flow X",Range(-1,1)) = 0
        _dY("Dissolve Flow Y",Range(-1,1)) = 0
        _disOutRange("Dissolve OutLine Range",Range(1,10)) = 1
        [Toggle(_USE_SMOOTH)] _Use_Smooth("Use Smooth?" , int) = 0  
        _smoothRange("Smooth Range",Range(0,1)) = 0.5             
        //[HDR]_outLineColor("Dissolve OutLine Color", Color) = (1, 1, 1, 1)
        
        [Space(20)]
        [Toggle(_USE_NOISE)] _Use_Noise("Use Noise?" , int) = 0    
        _NoiseTex("Noise Texture", 2D) = "black" {}
        [Toggle(_NOISE_POLAR_COORDINATE)] _Noise_Polar_Coordiante("Use Noise Polar Coordinate?" , int) = 0
        _nAngle ("Noise Angle", Range(-3.14, 3.14)) = 0.0
        _nSpeed("Noise Flow Speed",Range(0,1)) = 0
        _nX("Noise Flow X",Range(-1,1)) = 0
        _nY("Noise Flow Y",Range(-1,1)) = 0
        [Space(10)]
        _mFlowIntensity("Main Flow Intensity", Range(-1,1)) = 0
        _aFlowIntensity("Alpha Flow Intensity", Range(-1,1)) = 0
        _dFlowIntensity("Dissolve Flow Intensity", Range(-1,1)) = 0
        
        [Space(20)]
        //[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
        //[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0
        //[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
    }

        SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" 
               "Queue" = "Transparent" 
               "RenderType" = "Transparent" 
               "PreviewType" = "Plane" 
               "IgnoreProjector" = "True" }

        Blend[_SrcBlend][_DstBlend]
        Cull Off         
        ZWrite Off  
        ZTest Off     
        Lighting Off

        Pass
        {
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag	
            #pragma shader_feature _USE_ALPHA
            #pragma shader_feature _USE_DISSOLVE
            #pragma shader_feature _USE_NOISE
            #pragma shader_feature _USE_SMOOTH
            #pragma shader_feature _MAIN_POLAR_COORDINATE
            #pragma shader_feature _ALPHA_POLAR_COORDINATE
            #pragma shader_feature _DISSOLVE_POLAR_COORDINATE
            #pragma shader_feature _NOISE_POLAR_COORDINATE
            #pragma shader_feature _MAIN_FLOW_CUSTOMDATA
            #pragma shader_feature _ALPHA_FLOW_CUSTOMDATA
            #pragma shader_feature _DISSOLVE_FLOW_CUSTOMDATA

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #define UNITY_TWO_PI 6.283185

            struct VertexIntput
            {               
                float4 pos : POSITION;
                float4 customdata1 : TEXCOORD0;
                float4 customdata2 : TEXCOORD1;
                float2 uv0 : TEXCOORD2;               
                float2 uv1 : TEXCOORD3;
                float2 uv2: TEXCOORD4;
                float2 uv3 : TEXCOORD5;
                half4 color : COLOR;               
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float4 customdata1 : TEXCOORD0;
                float4 customdata2 : TEXCOORD1;
                float2 uv0 : TEXCOORD2;               
                float2 uv1 : TEXCOORD3;
                float2 uv2: TEXCOORD4;
                float2 uv3 : TEXCOORD5;
                half4 color : COLOR0;            
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float4 _MainTex_ST;             
                float _mSpeed;
                float _mX;
                float _mY;             
                float _mAngle;
                float _mFlowIntensity;             
                float4 _NoiseTex_ST;
                float _nAngle;
                float _nSpeed;
                float _nX;
                float _nY;              
                float4 _AlphaTex_ST;
                float _aAngle;
                float _aSpeed;
                float _aX;
                float _aY;
                float _aFlowIntensity;
                float4 _DisolveTex_ST;
                float _dAngle;
                float _dSpeed;
                float _dX;
                float _dY;
                float _smoothRange;
                float _disOutRange;
                float _dFlowIntensity;

            CBUFFER_END
                sampler2D _MainTex;
#ifdef _USE_ALPHA
                sampler2D _AlphaTex;
#endif
#ifdef _USE_NOISE
                sampler2D _NoiseTex;
#endif
#ifdef _USE_DISSOLVE
                sampler2D _DisolveTex;
#endif
            float2 toPolar(float2 UV) {
                UV -= float2(0.5, 0.5);
                float polarLength = length(UV);//* _lScale
                float angle = atan2(UV.x, UV.y);
                float polarRadial = angle * (1 / UNITY_TWO_PI);// * _rScale

                return float2(polarRadial, polarLength);
            }

            float2 uvRotate(float2 UV, float angle) {
                float2 pivot = float2(0.5, 0.5);
                // Rotation Matrix
                float cosAngle = cos(angle);
                float sinAngle = sin(angle);
                float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
                // Rotation consedering pivot
                UV -= pivot;
                UV = mul(rot, UV);
                UV += pivot;

                return UV;
            }

            VertexOutput vert(VertexIntput v)
            {
                VertexOutput o;     
                o.color = v.color;
                o.pos = TransformObjectToHClip(v.pos.xyz);
                o.customdata1 = v.customdata1;
                o.customdata2 = v.customdata2;
                o.uv0 = uvRotate(v.uv0, _mAngle);

                o.uv1 = v.uv1;
#ifdef _USE_NOISE
                o.uv1 = uvRotate(v.uv1, _nAngle);
#endif
                o.uv2 = v.uv2;
#ifdef _USE_DISSOLVE
                o.uv2 = uvRotate(v.uv2, _dAngle);
#endif
                o.uv3 = v.uv3;
#ifdef _USE_ALPHA
                o.uv3 = uvRotate(v.uv3, _aAngle);
#endif                      
                return o;
            }

            half4 frag(VertexOutput i) : SV_Target
            {
#ifdef _MAIN_POLAR_COORDINATE               
                i.uv0 = toPolar(i.uv0);
#endif

#ifdef _USE_NOISE                
#ifdef _NOISE_POLAR_COORDINATE
                i.uv1 = toPolar(i.uv1);
#endif
#endif

#ifdef _USE_DISSOLVE    
#ifdef _DISSOLVE_POLAR_COORDINATE
                i.uv2 = toPolar(i.uv2);
#endif 
#endif 

#ifdef _USE_ALPHA          
#ifdef _ALPHA_POLAR_COORDINATE
                i.uv3 = toPolar(i.uv3);
#endif    
#endif   

#ifdef _MAIN_FLOW_CUSTOMDATA //main
                i.uv0 = i.uv0.xy * _MainTex_ST.xy + (_MainTex_ST.zw + float2(_mX, _mY) * i.customdata1.y);
#else
                i.uv0 = i.uv0.xy * _MainTex_ST.xy + fmod((_MainTex_ST.zw + _Time.y * float2(_mX, _mY) * _mSpeed), float2(1, 1));
#endif
        
#ifdef _USE_NOISE //Noise
                i.uv1 = i.uv1.xy * _NoiseTex_ST.xy + fmod((_NoiseTex_ST.zw + _Time.y * float2(_nX, _nY) * _nSpeed),float2(1,1));
#endif   
        
#ifdef _USE_DISSOLVE
#ifdef _DISSOLVE_FLOW_CUSTOMDATA //Dissolve
                i.uv2 = i.uv2.xy * _DisolveTex_ST.xy + (_DisolveTex_ST.zw + float2(_dX, _dY) * i.customdata1.w);
#else
                i.uv2 = i.uv2.xy * _DisolveTex_ST.xy + fmod((_DisolveTex_ST.zw + _Time.y * float2(_dX, _dY) * _dSpeed), float2(1,1));
#endif 
#endif 

#ifdef _USE_ALPHA
#ifdef _ALPHA_FLOW_CUSTOMDATA //alpha
                i.uv3 = i.uv3.xy * _AlphaTex_ST.xy + (_AlphaTex_ST.zw + float2(_aX, _aY) * i.customdata1.z);
#else
                i.uv3 = i.uv3.xy * _AlphaTex_ST.xy + fmod((_AlphaTex_ST.zw + _Time.y * float2(_aX, _aY) * _aSpeed), float2(1,1));
#endif 
#endif
             
#ifdef _USE_NOISE //Noise
                float4 noiseTex = tex2D(_NoiseTex, i.uv1);
#else
                float4 noiseTex = float4(0,0,0,0);
#endif
                //main
                float4 mainTex = tex2D(_MainTex, i.uv0 + (noiseTex.r * _mFlowIntensity));
                
#ifdef _USE_ALPHA //alpha
                float4 alphaTex = tex2D(_AlphaTex, i.uv3 + (noiseTex.r * _aFlowIntensity));
#else
                float4 alphaTex = float4(1,1,1,1);
#endif

#ifdef _USE_DISSOLVE //dissolve
                float4 disolveTex = tex2D(_DisolveTex , i.uv2 + (noiseTex.r * _dFlowIntensity)); 
#ifdef _USE_SMOOTH
                float outsmoothAlpha = smoothstep(disolveTex.r, disolveTex.r + (_disOutRange * 0.1), 1-i.customdata1.x);
                float inSmoothAlpha = smoothstep(1-disolveTex.r - (_disOutRange * 0.1) * _smoothRange, (1-disolveTex.r) + (_disOutRange * 0.1) * _smoothRange, i.customdata1.x);
                float OutLine = (1-inSmoothAlpha) * (1-outsmoothAlpha);
                float4 finalOutLine = float4(OutLine, OutLine, OutLine, OutLine);
                float mainAlpha = inSmoothAlpha;
#else 
                float disAlphaOut1 = step(1-i.customdata1.x, disolveTex.r);
                float disAlphaOut2 = step(1-i.customdata1.x, (disolveTex.r * _disOutRange));
                float OutLine = disAlphaOut2 - disAlphaOut1;
                float4 finalOutLine = float4(OutLine, OutLine, OutLine, OutLine) * i.customdata2;
                float mainAlpha = disAlphaOut2;
#endif   
#else
                float finalOutLine = float(0);
                float mainAlpha = float(1);
#endif
                float4 finalCol = float4(mainTex.rgb * i.color.rgb * _Color.rgb , mainTex.a) * (1-finalOutLine.r);
                return (finalCol + finalOutLine) * mainTex.a * alphaTex.a * alphaTex.r * i.color.a * _Color.a * mainAlpha;
            }
            ENDHLSL
        }
    }
    CustomEditor "FX_ParticleDissolveUseCustomDataGUI"
}

