using UnityEngine;
using TMPro;

public class SimplePainting : MonoBehaviour
{
    [SerializeField]
    private Texture2D paintingTexture;

    [SerializeField]
    private Renderer targetRenderer;


    private void OnValidate()
    {
        if (paintingTexture != null)
        {
            ApplyTexture(paintingTexture);
        }
    }


    private void ApplyTexture(Texture2D texture)
    {
        if (targetRenderer != null)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetTexture("_BaseColorMap", texture);
            propertyBlock.SetVector("_BaseColorMap_ST", new Vector4(0.99f, 0.99f, 0.005f, 0.005f));
            targetRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
