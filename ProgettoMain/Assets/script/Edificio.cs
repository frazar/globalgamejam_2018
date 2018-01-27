using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
    // Determina se un edificio è infetto
    public bool infetto;

    [Range(0.5f, 20)]
    public float SecondiPerInfezione;// numero di secondi necessari per infettare l`edificio

    //Indica l'incremento di infezione per i contadini che ci entrano
    public int valoreInfezione;

    // Use this for initialization
    void Start () {
        infetto = false;
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
