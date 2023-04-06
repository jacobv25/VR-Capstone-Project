using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    private List<Light> childLights;

    private void Awake()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent object not assigned in LightController.");
            return;
        }

        childLights = new List<Light>();
        GetChildLights(parentObject.transform);
    }

    private void GetChildLights(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Light light = child.GetComponent<Light>();
            if (light != null)
            {
                childLights.Add(light);
            }

            if (child.childCount > 0)
            {
                GetChildLights(child);
            }
        }
    }

    public void SetLightIntensity(float intensity)
    {
        foreach (Light light in childLights)
        {
            light.intensity = intensity;
        }
    }

    public void SetLightColor(Color color)
    {
        foreach (Light light in childLights)
        {
            light.color = color;
        }
    }

    public void SetLightRange(float range)
    {
        foreach (Light light in childLights)
        {
            light.range = range;
        }
    }

    public void SetLightSpotAngle(float spotAngle)
    {
        foreach (Light light in childLights)
        {
            if (light.type == LightType.Spot)
            {
                light.spotAngle = spotAngle;
            }
        }
    }
}
