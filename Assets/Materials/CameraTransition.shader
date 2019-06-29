Shader "Custom/CameraTransition" {
    Properties {
        _MainTex("LEAVE BLANK", 2D) = "white" {}
        _TransitionTex("Transition Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Transition("Transition", Range(0, 1)) = 0
    }
    // a shader must contain at least 1 subshader
    Subshader {
        // a pass renders an object - multiple passes = multiple renders
        Pass {
            // begin the shader program block
            CGPROGRAM
            #pragma vertex vertFunc
            #pragma fragment fragFunc

            // important shader variables/functions
            #include "UnityCG.cginc"
            // https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html
            // https://docs.unity3d.com/Manual/SL-ShaderSemantics.html
            // not sure why we have both appdata and v2f yet
            sampler2D _MainTex;
            sampler2D _TransitionTex;
            fixed4 _Color;
            float _Transition;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vertFunc(appdata a) {
                v2f output;
                output.pos = UnityObjectToClipPos(a.vertex);
                output.uv = a.uv;
                return output;
            }

            // we want to output the color for 1 pixel
            fixed4 fragFunc(v2f info) : SV_Target {

                // read from the texture that we pass in
                fixed4 color = tex2D(_TransitionTex, info.uv);
                // if our transition pixel is less than the cutoff, render our transition color
                if (color.b < _Transition) {
                    return _Color;
                }
                // otherwise, let the camera continue to render its image
                return tex2D(_MainTex, info.uv);
            }
            
            ENDCG
        }
    }
}
