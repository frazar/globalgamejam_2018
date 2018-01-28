using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
    // Determina se un edificio è infetto
    public bool infetto = false;

    [Range(0.5f, 20)]
    public float SecondiPerInfezione;// numero di secondi necessari per infettare l`edificio

    //Indica l'incremento di infezione per i contadini che ci entrano
    public int valoreInfezione;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {        
    }

    public void infetta() {
        this.infetto = true;
        // Show the red cross on the first door
        this.gameObject.transform.Find("bloodcross").GetComponent<SpriteRenderer>().enabled = true;

        // Show the red cross on the second door, if anyW
        if (this.gameObject.transform.Find("bloodcross2") != null) {
            this.gameObject.transform.Find("bloodcross2").GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
