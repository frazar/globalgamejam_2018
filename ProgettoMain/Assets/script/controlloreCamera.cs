using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlloreCamera : MonoBehaviour {

    // Use this for initialization
    public GameObject player1;
    public GameObject player2;
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 pos1 = player1.transform.position;
        Vector3 pos2 = player2.transform.position;
        Vector3 media = new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, -10);
        transform.position = media;
	}
}
