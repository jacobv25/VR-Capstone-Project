using UnityEngine;
using TMPro;
using BNG;

public class FootstepController : MonoBehaviour
{
    [System.Serializable]
    public class FootstepSFX
    {
        public SurfaceType surfaceType;
        public AudioClip[] footstepClips;
    }

    public FootstepSFX[] footstepSFX;
    private CharacterController characterController;
    private AudioManager audioManager;
    private SurfaceType currentSurfaceType = SurfaceType.Default;


    [SerializeField]
    [Tooltip("Factor to adjust the overall footstep cooldown while running.")]
    private float footstepCooldownFactor = 1.0f;

    private float footstepCooldown = 0.3f;
    private float lastFootstepTime = -1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioManager = AudioManager.Instance;
    }

    private void Update()
    {

        Vector2 leftThumbstickAxis = InputBridge.Instance.LeftThumbstickAxis;
        bool isMoving = (Mathf.Abs(leftThumbstickAxis.x) >= 0.2f || Mathf.Abs(leftThumbstickAxis.y) >= 0.2f);

        if (characterController.isGrounded && isMoving)
        {
            float maxAxisValue = Mathf.Max(Mathf.Abs(leftThumbstickAxis.x), Mathf.Abs(leftThumbstickAxis.y));

            if (Time.time - lastFootstepTime >= footstepCooldown)
            {
                PlayFootstepSFX(currentSurfaceType);
                lastFootstepTime = Time.time;

                footstepCooldown = Mathf.Lerp(0.5f, 0.1f, maxAxisValue) * footstepCooldownFactor;
            }
        }
    }

    private void PlayFootstepSFX(SurfaceType surfaceType)
    {
        AudioClip[] clips = null;

        foreach (FootstepSFX sfx in footstepSFX)
        {
            if (sfx.surfaceType == surfaceType)
            {
                clips = sfx.footstepClips;
                break;
            }
        }

        if (clips != null && clips.Length > 0)
        {
            int clipIndex = Random.Range(0, clips.Length);
            audioManager.PlayAudioClip(clips[clipIndex]);
        }
    }

    public void SetCurrentSurfaceType(SurfaceType surfaceType)
    {
        currentSurfaceType = surfaceType;
    }
}
