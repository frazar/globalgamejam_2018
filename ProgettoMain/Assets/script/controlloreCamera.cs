using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class controlloreCamera : MonoBehaviour {

    public float CAMERA_SPEED_FACTOR = 0.1f;
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
        // Calcola la posizione target come media delle posizione dei player attivi
        Vector3 targetPosition = Vector3.zero;
        int playerCounter = 0;

        if (player1 != null) {
            targetPosition += player1.transform.position;
            playerCounter++;
        } 

        if (player2 != null) {
            targetPosition += player2.transform.position;
            playerCounter++;
        }

        Assert.IsTrue(playerCounter > 0);

        targetPosition = targetPosition / playerCounter;
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, CAMERA_SPEED_FACTOR);
        newPosition[2] = -10; // Always stay above the scene
        transform.position = newPosition;
	}


    void playerMorto(int playerIndex) {
        if (playerIndex == 1) {
            player1 = null;            
        } 

        if (playerIndex == 2) {
            player2 = null;
        }
    }
}
