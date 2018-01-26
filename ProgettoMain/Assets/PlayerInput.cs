using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // Use this for initialization
    
    [Range(1,2)]
    public int player;
    public float moltiplicatore;// moltiplicagtore del movimento
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movimento = Vector3.zero;
        if (Input.GetButton("su"+player))
        {
            movimento += Vector3.up * Time.deltaTime;
        }

        if (Input.GetButton("sotto" + player))
        {
            movimento += Vector3.down * Time.deltaTime;
        }
        if (Input.GetButton("destra" + player))
        {
            movimento += Vector3.right * Time.deltaTime;
        }
        if (Input.GetButton("sinistra" + player))
        {
            movimento += Vector3.left * Time.deltaTime;
        }


        transform.position += movimento * moltiplicatore;
	}
}
