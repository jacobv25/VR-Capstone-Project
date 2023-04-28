using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval;

    private float timeSinceLastSpawn;

    private void Start()
    {
        timeSinceLastSpawn = 0f;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObject();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObject()
    {
        GameObject newObject = Instantiate(objectToSpawn, transform.position, Quaternion.Euler(90f, 0f, 0f));
    }
}
