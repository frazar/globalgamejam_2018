using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
	//determina se un edificio è infetto
	private bool infetto;
    [Range(0.5f, 20)]
    public float SecondiPerInfezione;// numero di secondi necessari per infettare l`edificio
	// Use this for initialization
	void Start () {
		infetto = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setInfetto (bool tmp) {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
		infetto = tmp;
	}

    public bool getInfetto()
    {
        return infetto;
    }
}
