using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightController))]
public class LightControllerEditor : Editor
{
    private LightController lightController;

    private float intensity;
    private Color color;
    private float range;
    private float spotAngle;

    private void OnEnable()
    {
        lightController = (LightController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        intensity = EditorGUILayout.FloatField("Intensity", intensity);
        if (GUILayout.Button("Set Intensity"))
        {
            lightController.SetLightIntensity(intensity);
        }

        color = EditorGUILayout.ColorField("Color", color);
        if (GUILayout.Button("Set Color"))
        {
            lightController.SetLightColor(color);
        }

        range = EditorGUILayout.FloatField("Range", range);
        if (GUILayout.Button("Set Range"))
        {
            lightController.SetLightRange(range);
        }

        spotAngle = EditorGUILayout.Slider("Spot Angle", spotAngle, 1f, 179f);
        if (GUILayout.Button("Set Spot Angle"))
        {
            lightController.SetLightSpotAngle(spotAngle);
        }
    }
}
