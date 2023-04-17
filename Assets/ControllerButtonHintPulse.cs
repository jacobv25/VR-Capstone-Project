using System.Collections;
using UnityEngine;

public class ControllerButtonHintPulse : MonoBehaviour
{
    public Material buttonHintsMaterial;
    public float pulseSpeed = 1.0f;
    public Color pulseColor;

    private Color originalColor;

    private void Start()
    {
        if (buttonHintsMaterial == null)
        {
            Debug.LogWarning("Button hints material not set. Please set it in the Inspector.");
            return;
        }

        originalColor = buttonHintsMaterial.GetColor("_Tint");
        StartCoroutine(PulseMaterial());
    }

    private IEnumerator PulseMaterial()
    {
        while (true)
        {
            // Fade in
            for (float t = 0; t <= 1; t += Time.deltaTime * pulseSpeed)
            {
                Color newColor = Color.Lerp(originalColor, pulseColor, t);
                buttonHintsMaterial.SetColor("_Tint", newColor);
                yield return null;
            }

            // Fade out
            for (float t = 0; t <= 1; t += Time.deltaTime * pulseSpeed)
            {
                Color newColor = Color.Lerp(pulseColor, originalColor, t);
                buttonHintsMaterial.SetColor("_Tint", newColor);
                yield return null;
            }
        }
    }
}
