using UnityEngine;

public class AttackEnemyMovement : IDroneMovement
{
    private GameObject enemy;
    private Transform weaponTransform;
    private DroneController droneController;

    public AttackEnemyMovement(DroneController droneController, GameObject enemy, Transform weaponTransform)
    {
        this.droneController = droneController;
        this.enemy = enemy;
        this.weaponTransform = weaponTransform;
    }

    public void UpdateMovement(DroneController droneController)
    {
        if (enemy != null)
        {
            // Face the enemy
            Vector3 directionToEnemy = (enemy.transform.position - droneController.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            droneController.transform.rotation = Quaternion.Slerp(droneController.transform.rotation, targetRotation, droneController.rotationSpeed * Time.deltaTime);

            // Point the weapon at the enemy
            Vector3 directionToAim = (enemy.transform.position - weaponTransform.position).normalized;
            Quaternion weaponTargetRotation = Quaternion.LookRotation(directionToAim);
            weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, weaponTargetRotation, droneController.rotationSpeed * Time.deltaTime);

            // Fire the weapon
            // Implement your weapon firing logic here
            droneController.ShootWeapon();
        }
    }
}
