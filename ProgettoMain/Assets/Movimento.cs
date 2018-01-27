using UnityEngine;
using System.Collections.Generic;
using Random;
using System;

[RequireComponent(typeof(PolyNavAgent))]

public class Movimento: MonoBehaviour{
    private PolyNavAgent _agent;
	private PolyNavAgent agent{
		get {return _agent != null? _agent : _agent = GetComponent<PolyNavAgent>();}
	}
    private Vector2[] destinazioniPossibili; //Vettore delle destinazioni possibili
    private Vector2 destinazioneAttuale; // destinazione attualmente selezionata
    private Random rnd; // oggetto per accedere a funzioni random
    private inMovimento = false; // boolean per capire se l'oggetto è in movimento

    void Start(destinazioni){
        this.rnd = new Random();
        this.destinazioniPossibili = destinazioni;
        muoviti();
    }

    void Update() {
        if (!inMovimento){
            muoviti()
        }
    }
	
    Vector2 muoviti(){
        int r = rnd.Next(destinazioniPossibili.Length);
        this.destinazioneAttuale = destinazioniPossibili[r];
        agent.SetDestination(destinazioneAttuale);
        this.inMovimento = true;
    }
}