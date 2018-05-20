using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called once per frame, but runs after all items in Update() are processed - e.g. after movement
	void LateUpdate () {
        transform.position = player.transform.position + offset;
        //Debug.Log("Offset: " + offset);
	}
}
