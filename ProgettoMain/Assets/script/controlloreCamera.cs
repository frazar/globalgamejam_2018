using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class controlloreCamera : MonoBehaviour {

    private Vector3 pos1, pos2;

    public GameObject player1, player2;
    // Use this for initialization
	void Start () {
        player1 = GameObject.Find("player1");
        player2 = GameObject.Find("player2");

        Assert.IsNotNull(player1);
        Assert.IsNotNull(player2);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 pos1 = player1.transform.position;
        Vector3 pos2 = player2.transform.position;
        Vector3 media = new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, -10);
        transform.position = media;
	}
}
