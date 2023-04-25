using System.Collections;
using UnityEngine;

public class RaiseAndLowerObject : MonoBehaviour
{
    [SerializeField] private float raiseDistance = 5f;
    [SerializeField] private float raiseDuration = 2f;
    private ResetOnDistance resetPositionOnDistance;

    private void Start()
    {
        resetPositionOnDistance = GetComponent<ResetOnDistance>();
    }

    public void Raise()
    {
        StartCoroutine(Raise(gameObject.transform, raiseDistance, raiseDuration));
    }

    public void LowerAndDeactivate()
    {
        StartCoroutine(Lower(gameObject.transform, raiseDistance, raiseDuration));
    }

    private IEnumerator Raise(Transform objTransform, float distance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = objTransform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, distance, 0f);

        while (elapsedTime < duration)
        {
            objTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objTransform.position = targetPosition;
        if(resetPositionOnDistance != null)
        {
            Debug.Log("reset position set!");
            resetPositionOnDistance.SetResetPosition( objTransform.position );
        }
    }

    private IEnumerator Lower(Transform objTransform, float distance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = objTransform.position;
        Vector3 targetPosition = startPosition - new Vector3(0f, distance, 0f);

        while (elapsedTime < duration)
        {
            objTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objTransform.position = targetPosition;
        objTransform.gameObject.SetActive(false);
    }
}
