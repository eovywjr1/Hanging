Shader "Custom/hallu_outline"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB)", 2D) = "white" {}
		_Brightness("Brightness",Range(-1,1))=0
		_Noise("Noise",Range(0,1))=1
		_NoiseSpeed("NoiseSpeed",Range(0,10))=1
		_MoveSpeed("MoveSpeed",Range(0,1))=1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
		sampler2D _MainTex2;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

		float _Brightness;
		float _Noise;
		float _NoiseSpeed;
		float _MoveSpeed;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 fPos=float2(0,-_Time.y*_NoiseSpeed);
			float2 fMoveSpeed=float2(0,_Time.y*_MoveSpeed);

			fixed d =tex2D(_MainTex2,IN.uv_MainTex2 + fPos);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex + d.r * _Noise);

            o.Emission = c.rgb;
			o.Alpha=c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
