using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlloreCamera : MonoBehaviour {

    private Vector3 pos1, pos2;

    // Use this for initialization
	void Start () {
        Vector3 pos1 = GameObject.Find("player1").transform.position;
        Vector3 pos2 = GameObject.Find("player2").transform.position;		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 media = new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, -10);
        transform.position = media;
	}
}
