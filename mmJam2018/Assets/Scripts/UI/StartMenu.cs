using System.Collections;
using UnityEngine;
using Assets.Scripts.States;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject title;

    [SerializeField]
    private GameObject hud;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject gameCamera;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private StateMachine stateMachine;

    [SerializeField]
    private float restartDelay;

    [SerializeField]
    private bool endingRunning;

    void Start()
    {
        endingRunning = false;
        cameraController = gameCamera.GetComponent<CameraController>();
        Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        // Cannot check if coroutine is already running, so we're stuck with this for now.
        if ((stateMachine.CurrentState is EndingState) && !endingRunning) {
            StartCoroutine(ResetUIWithDelay());
        }
    }

    private void Initialize()
    {
        hud.SetActive(false);
        player.SetActive(false);

        title.SetActive(true);

        stateMachine.CurrentState = new StartMenuState();
        stateMachine.CurrentState.OnEnter(stateMachine);
    }

    private void StartGame()
    {
        hud.SetActive(true);
        player.SetActive(true);
        title.SetActive(false);

        cameraController.PlayIntro();
        stateMachine.CurrentState.AdvanceState(stateMachine);
    }

    private IEnumerator ResetUIWithDelay()
    {
        endingRunning = true;
        yield return new WaitForSeconds(restartDelay);
        Initialize();
        endingRunning = false;
    }
}
