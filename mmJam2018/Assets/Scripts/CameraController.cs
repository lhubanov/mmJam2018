using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
	}

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
	}

    public void PlayIntro()
    {
        // FIXME: Add zoom in script
    }
}
