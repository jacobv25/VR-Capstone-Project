using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] private float glowIntensity = 2.0f;

    private MeshRenderer buttonRenderer;

    private void Awake()
    {
        buttonRenderer = GetComponent<MeshRenderer>();
    }

    public void SetGlow()
    {
        if (buttonRenderer != null)
        {
            Material[] materials = buttonRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].HasProperty("_EmissionColor"))
                {
                    materials[i].SetColor("_EmissionColor", glowColor * glowIntensity);
                    materials[i].EnableKeyword("_EMISSION");
                }
            }
        }
    }

    public void ClearGlow()
    {
        if (buttonRenderer != null)
        {
            Material[] materials = buttonRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].HasProperty("_EmissionColor"))
                {
                    materials[i].SetColor("_EmissionColor", Color.black);
                    materials[i].DisableKeyword("_EMISSION");
                }
            }
        }
    }
}