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

        // FIXME: Cannot check if coroutine is already running, but could use a smarter solution
        if ((stateMachine.CurrentState is EndingState) && !endingRunning) {
            StartCoroutine(ResetUIWithDelay());
            endingRunning = true;
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
        yield return new WaitForSeconds(restartDelay);
        Initialize();
        endingRunning = false;
    }
}
