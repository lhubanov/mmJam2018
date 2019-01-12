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

    void Start()
    {
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

        //if (stateMachine.CurrentState is EndingState) {
        //    Initialize();
        //    stateMachine.CurrentState = new StartMenuState();
        //}
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
}
