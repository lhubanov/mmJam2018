using UnityEngine;
using Assets.Scripts.States;

public class Menu : MonoBehaviour
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
        // FIXME: Generate world

        hud.SetActive(false);
        player.SetActive(false);

        title.SetActive(true);

        stateMachine.CurrentState = new StartState();
        stateMachine.CurrentState.OnEnter(stateMachine);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            hud.SetActive(true);
            player.SetActive(true);
            title.SetActive(false);

            //stateMachine.CurrentState = new StartState();
            //stateMachine.CurrentState.OnEnter(stateMachine);

            cameraController.PlayIntro();
        }
    }
}
