using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        SpinCoin();
    }

    private void SpinCoin()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit...");
        if (other.CompareTag("Car"))
        {
            Debug.Log("THE CAR");
            CollectCoin();
        }

    }

    private void CollectCoin()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length);
    }
}
