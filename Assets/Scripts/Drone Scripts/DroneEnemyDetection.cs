using UnityEngine;

public class DroneEnemyDetection : MonoBehaviour
{
    public LayerMask enemyLayer;
    public DroneController droneController;
    public float detectionRange = 50f;
    public float detectionInterval = 0.5f;
    public int numberOfRays = 8;
    public float angleBetweenRays = 20f;

    private GameObject currentEnemy;

    private void Start()
    {
        InvokeRepeating("DetectEnemy", detectionInterval, detectionInterval);
    }

    private void DetectEnemy()
    {
        GameObject newEnemy = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * angleBetweenRays - ((numberOfRays - 1) * angleBetweenRays) / 2;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange, enemyLayer))
            {
                if (hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                    newEnemy = hit.collider.gameObject;
                }

                Debug.DrawRay(transform.position, direction * hit.distance, Color.red, detectionInterval); // Visualize the raycasts in the editor
            }
            else
            {
                Debug.DrawRay(transform.position, direction * detectionRange, Color.green, detectionInterval); // Visualize the raycasts in the editor
            }
        }

        if (newEnemy != null)
        {
            if (newEnemy != currentEnemy)
            {
                if (currentEnemy != null)
                {
                    EnemyLost(currentEnemy);
                }

                currentEnemy = newEnemy;
                EnemyDetected(currentEnemy);
            }
        }
        else if (currentEnemy != null)
        {
            EnemyLost(currentEnemy);
            currentEnemy = null;
        }
    }

    private void EnemyDetected(GameObject enemy)
    {
        // Change the drone's state to FightState
        droneController.EngageEnemy(enemy);
    }

    private void EnemyLost(GameObject enemy)
    {
        // Reset the drone's state to a non-fight state, e.g. FollowPlayerState
        droneController.SetState(new FollowPlayerState(droneController));
    }
}
