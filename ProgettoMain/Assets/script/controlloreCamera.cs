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
        Vector3 pos1,pos2,target;
        target = Vector3.zero;
      
        pos2 = player2.transform.position;
        pos1 = player1.transform.position;

        if(player1.activeSelf && player2.activeSelf)
        {
            target= new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, -10); 
        }else
        {
            if (player1.activeSelf)
            {
                target = new Vector3(pos1.x, pos1.y, -10);
            }else  if (player2.activeSelf)
            {
                target = new Vector3(pos2.x, pos2.y, -10);
            }
         }
        transform.position = target;
	}
}
