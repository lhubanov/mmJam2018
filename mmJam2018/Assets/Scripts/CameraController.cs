using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    //FIXME: Add intro motion here (can just e a left-to-right motion with a vector.lerp?)


    void Start()
    {
        offset = transform.position - player.transform.position;
	}
	
	void LateUpdate()
    {
        transform.position = player.transform.position + offset;
	}
}
