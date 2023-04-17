using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private Transform parentObject;
    [SerializeField] private Material newMaterial;

    void Start()
    {
        if (parentObject == null || newMaterial == null)
        {
            Debug.LogWarning("Parent object or new material is not set. Please set them in the Inspector.");
            return;
        }

        ChangeMaterials(parentObject, newMaterial);
    }

    private void ChangeMaterials(Transform parent, Material targetMaterial)
    {
        List<Renderer> renderers = new List<Renderer>();

        GetAllRenderersInChildren(parent, ref renderers);

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = targetMaterial;
            }

            renderer.sharedMaterials = materials;
        }
    }

    private void GetAllRenderersInChildren(Transform parent, ref List<Renderer> renderers)
    {
        Renderer renderer = parent.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderers.Add(renderer);
        }

        foreach (Transform child in parent)
        {
            GetAllRenderersInChildren(child, ref renderers);
        }
    }
}
