using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private float zoomRate;

    void Start()
    {
        gameCamera = GetComponent<Camera>();
        offset = transform.position - player.transform.position;
	}

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
	}

    public void PlayIntro()
    {
        StartCoroutine(ZoomInCamera());
    }

    private IEnumerator ZoomInCamera()
    {
        while (Camera.main.fieldOfView > 70)
        {
            Camera.main.fieldOfView -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
