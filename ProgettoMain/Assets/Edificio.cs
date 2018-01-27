using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
	//determina se un edificio è infetto
	private bool infetto;

	// Use this for initialization
	void Start () {
		infetto = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setInfetto () {
		infetto = true;
	}
}
