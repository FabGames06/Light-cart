Shader "FabDev06/UnlitWithLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Lighting On
            SetTexture [_MainTex] { combine texture * primary }
        }
    }
}
