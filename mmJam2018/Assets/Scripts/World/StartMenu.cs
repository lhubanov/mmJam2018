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

        // if World.CurrentState is StartMenuState
        // Initialize()
    }

    private void Initialize()
    {
        hud.SetActive(false);
        player.SetActive(false);

        title.SetActive(true);

        stateMachine.CurrentState = new StartState();
        stateMachine.CurrentState.OnEnter(stateMachine);
    }

    private void StartGame()
    {
        hud.SetActive(true);
        player.SetActive(true);
        title.SetActive(false);

        cameraController.PlayIntro();
    }
}
