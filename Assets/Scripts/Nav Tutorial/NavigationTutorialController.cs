using System.Collections;
using UnityEngine;
using BNG;
public class NavigationTutorialController : MonoBehaviour
{
    [SerializeField] private LocomotionManager locomotionManager;
    [SerializeField] private GameObject table;
    [SerializeField] private CustomVRPauseMenu pauseMenu;

    [Header("Buttons")]
    [SerializeField] private GameObject turnButton;
    [SerializeField] private GameObject smoothLocButton;
    [SerializeField] private GameObject teleportButton;
    [SerializeField] private GameObject finishButton;

    [Header("Movement Canvas")]
    [SerializeField] private GameObject smoothLocCanvas;
    [SerializeField] private GameObject teleCanvas;


    [Header("Audio Clips")]
    [SerializeField] private AudioClip navigationTutorialAudio;
    [SerializeField] private AudioClip snapTurn1Audio;
    [SerializeField] private AudioClip snapTurn2Audio;
    [SerializeField] private AudioClip takeAMomentAudio;
    [SerializeField] private AudioClip locomotionSelectionAudio;
    [SerializeField] private AudioClip exitAudio;


    [Header("Left Button References")]
    [SerializeField] private ButtonController trackPadLeft;
    [SerializeField] private ButtonController trackPadRight;

    [Header("Debugging")]
    public bool debug;
    [HideInInspector] public int currentStep = 0;

    private bool turnEnabled = false;
    private bool enableTurnButtonPressed = false;

    private void OnEnable()
    {
        InputBridge.OnInputsUpdated += CheckInputs;
    }

    private void OnDisable()
    {
        InputBridge.OnInputsUpdated -= CheckInputs;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private void CheckInputs()
    {

    }

    private IEnumerator PlayIntro()
    {
        if (!debug)
        {
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.PlayAudioClip(navigationTutorialAudio);
            yield return new WaitForSeconds(11);
            AudioManager.Instance.PlayAudioClip(snapTurn1Audio);
            trackPadRight.SetGlow();
            table.SetActive(true);
            table.GetComponent<RaiseAndLowerObject>().Raise();
            yield return new WaitForSeconds(2);
            turnButton.SetActive(true);
            finishButton.SetActive(true);
            turnButton.GetComponent<RaiseAndLowerObject>().Raise();
            finishButton.GetComponent<RaiseAndLowerObject>().Raise();
            yield return new WaitForSeconds(5);

        }
        else
        {
            table.SetActive(true);
            table.GetComponent<RaiseAndLowerObject>().Raise();
            yield return new WaitForSeconds(2);
            turnButton.SetActive(true);
            finishButton.SetActive(true);
            turnButton.GetComponent<RaiseAndLowerObject>().Raise();
            finishButton.GetComponent<RaiseAndLowerObject>().Raise();
        }
        

    }

    public void LookSelected()
    {
        if (!enableTurnButtonPressed)
        {
            enableTurnButtonPressed = true;
            StartCoroutine(PlayTakeAMoment());
        }

        turnEnabled = !turnEnabled;
       
        if (turnEnabled)
        {
            pauseMenu.UseSnapTurn();
        }
        else
        {
            pauseMenu.DisableTurn();
        }
    }

    private IEnumerator PlayTakeAMoment()
    {
        AudioManager.Instance.PlayAudioClip(snapTurn2Audio);
        yield return new WaitForSeconds(10f);
        AudioManager.Instance.PlayAudioClip(takeAMomentAudio);
    }

    public void FinishTurning()
    {
        StartCoroutine(PlayFinishTurning());
    }

    private IEnumerator PlayFinishTurning()
    {
        turnButton.GetComponent<RaiseAndLowerObject>().LowerAndDeactivate();
        finishButton.GetComponent<RaiseAndLowerObject>().LowerAndDeactivate();
        trackPadRight.ClearGlow();

        yield return new WaitForSeconds(1f);

        smoothLocButton.SetActive(true);
        teleportButton.SetActive(true);
        smoothLocButton.GetComponent<RaiseAndLowerObject>().Raise();
        teleportButton.GetComponent<RaiseAndLowerObject>().Raise();

        yield return new WaitForSeconds(1f);
        smoothLocCanvas.SetActive(true);
        teleCanvas.SetActive(true);
        trackPadLeft.SetGlow();

        yield return new WaitForSeconds(.5f);

        AudioManager.Instance.PlayAudioClip(locomotionSelectionAudio);
    
        yield return new WaitForSeconds(13f);

        AudioManager.Instance.PlayAudioClip(exitAudio);
    }

}
