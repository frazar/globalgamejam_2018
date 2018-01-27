using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {

    // Use this for initialization    
    [Range(1,2)]
    public int player;
    public float MOLTIPLICATORE;// moltiplicagtore del movimento

    Rigidbody2D phy;
	void Start () {
        phy = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 movimento = Vector3.zero;
        if (Input.GetButton("su" + player)) {
            movimento += Vector3.up * Time.deltaTime;
        }

        if (Input.GetButton("sotto" + player)) {
            movimento += Vector3.down * Time.deltaTime;
        }
        if (Input.GetButton("destra" + player)) {
            gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);
            movimento += Vector3.right * Time.deltaTime;
        }
        if (Input.GetButton("sinistra" + player)) {
            gameObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
            movimento += Vector3.left * Time.deltaTime;
        }

        phy.velocity = movimento * MOLTIPLICATORE;
	}
}
