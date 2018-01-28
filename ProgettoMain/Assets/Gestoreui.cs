using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestoreui : MonoBehaviour {

    public GameObject menuwin;
    public GameObject  menulose;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Vittoria () {
        menuwin.SetActive(true);
       
	}
    void Sconfitta()
    {
        menulose.SetActive(true);
    }

}
