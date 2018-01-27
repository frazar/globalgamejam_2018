using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edificio : MonoBehaviour {
    //determina se un edificio è infetto
    private bool infetto;

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
