using UnityEngine;
public class TransparentObjectController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Range(0f, 1f)] private float transparency = 0.5f;

    private void Start()
    {
        MakeMaterialsTransparent(gameObject, transparency);
    }

    private void MakeMaterialsTransparent(GameObject parent, float alpha)
    {
        MeshRenderer[] childRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in childRenderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a = alpha;
                materials[i].color = color;

                // Enable transparency for the material
                if (materials[i].HasProperty("_Mode"))
                {
                    materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    materials[i].SetInt("_ZWrite", 0);
                    materials[i].DisableKeyword("_ALPHATEST_ON");
                    materials[i].DisableKeyword("_ALPHABLEND_ON");
                    materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
            }
        }
    }
}