using UnityEngine;

/// <summary>
/// 把GrabBackground转化为RenderTexture
/// 需要添加到摄像机上
/// </summary>
[RequireComponent(typeof(Camera))]
public class GrabToTexture : MonoBehaviour
{
    private string ShaderName = "Custom/GrabToTexture";
    private Material _material;

    public static RenderTexture Render;

    private void Awake()
    {
        _material = new Material(Shader.Find(ShaderName));
        Render = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
    }

    /// <summary>
    /// 系统方法，只有摄像机组件汇之星
    /// </summary>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, Render, _material);
        Graphics.Blit(src, dest);
    }
}
