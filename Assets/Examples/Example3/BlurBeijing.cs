using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 模糊背景
/// </summary>
public class BlurBeijing : MonoBehaviour
{
    private static string ShaderName = "Custom/GaussianBlur";

    //模糊半径
    public float BlurRadius = 5.0f;

    //降分辨率
    public int downSample = 1;

    //迭代次数
    public int iteration = 5;

    private Material material;
    private RenderTexture renderTexture;
    private RawImage rawImage;

    public void Start()
    {
        rawImage = transform.GetComponentInChildren<RawImage>(true);
        material = new Material(Shader.Find(ShaderName));
        renderTexture = GrabToTexture.Render;
    }

    public RenderTexture generateBlurTexture(RenderTexture source)
    {
        //申请RenderTexture，RT的分辨率按照downSample降低
        RenderTexture rt1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0,
            RenderTextureFormat.Default);
        RenderTexture rt2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0,
            RenderTextureFormat.Default);

        Graphics.Blit(source, rt1);

        //进行迭代高斯模糊
        for (int i = 0; i < iteration; i++)
        {
            //第一次高斯模糊，设置offsets，竖向模糊
            material.SetVector("_offsets", new Vector4(0, BlurRadius, 0, 0));
            Graphics.Blit(rt1, rt2, material);
            //第二次高斯模糊，设置offsets，横向模糊
            material.SetVector("_offsets", new Vector4(BlurRadius, 0, 0, 0));
            Graphics.Blit(rt2, rt1, material);
        }

        Graphics.Blit(rt1, source);

        //释放申请的两块RenderBuffer内容
        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
        return source;
    }

    public void Update()
    {
        rawImage.color = Color.white;
        rawImage.texture = generateBlurTexture(renderTexture);
    }
}
