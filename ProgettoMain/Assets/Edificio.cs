using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
	//determina se un edificio è infetto
	private bool infetto;

	//Indica l'incremento di infezione per i contadini che ci entrano
	private int valoreInfezione;

	// Use this for initialization
	void Start () {
		infetto = false;
		valoreInfezione = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setInfetto () {
		infetto = true;
	}

	public int getValoreInfezione(){
		return this.valoreInfezione;
	}
}
