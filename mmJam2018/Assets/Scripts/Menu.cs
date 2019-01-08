using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update()
    {
        // If enter is pressed
        //hud.SetActive(true);
        //player.SetActive(true);
        //cameraController.PlayIntro();
        //title.SetActive(false);
    }
}
