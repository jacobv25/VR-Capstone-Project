using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnDistance : MonoBehaviour
{
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float yOffset = 0.1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 resetPosition;
    private List<Renderer> objectRenderers;
    private List<Material> objectMaterials;
    private Rigidbody objectRigidbody;

    public void SetResetPosition(Vector3 pos)
    {
        resetPosition = pos + new Vector3(0f, yOffset, 0f); ;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        objectRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        objectMaterials = new List<Material>();
        foreach (var renderer in objectRenderers)
        {
            objectMaterials.Add(renderer.material);
        }

        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(initialPosition, transform.position) > maxDistance)
        {
            StartCoroutine(ResetObject());
        }
    }

    private IEnumerator ResetObject()
    {
        // Dematerialize effect
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            SetMaterialsAlpha(alpha);
            yield return null;
        }
        SetMaterialsAlpha(0f);

        // Reset position, rotation, and physics
        transform.position = resetPosition;
        transform.rotation = initialRotation;
        if (objectRigidbody != null)
        {
            objectRigidbody.velocity = Vector3.zero;
            objectRigidbody.angularVelocity = Vector3.zero;
        }

        // Materialize effect
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            SetMaterialsAlpha(alpha);
            yield return null;
        }
        SetMaterialsAlpha(1f);
    }

    private void SetMaterialsAlpha(float alpha)
    {
        for (int i = 0; i < objectMaterials.Count; i++)
        {
            Color color = objectMaterials[i].color;
            color.a = alpha;
            objectMaterials[i].color = color;
        }
    }
}

