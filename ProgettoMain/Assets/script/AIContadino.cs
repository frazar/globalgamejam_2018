using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AIContadino : MonoBehaviour {
    // Copia incolla dalla documentazione di PolyNav
    private PolyNavAgent _agent;
    private PolyNavAgent agent{
        get {return _agent != null? _agent : _agent = GetComponent<PolyNavAgent>();}
    }

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    public const int VELOCITA_MOVIMENTO = 250;
    private Vector2 verticeDestinazioneAttuale;

    //info contadino
    private int infezione;


	void Start () {		
        // Per avere un percorso sensato, servono almeno due punti
        Assert.IsTrue(arrayVerticiPercorso.Length >= 2); 

        // Aggiungi i callback
        agent.OnDestinationReached += muoviti;
        agent.OnDestinationInvalid += muovitiDestinazioneInvalida;

        // Parti con i primi movimenti
        muoviti();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void muovitiDestinazioneInvalida() {
        Debug.Log("Destinazione invalida" + verticeDestinazioneAttuale);
        muoviti();
    }

    void muoviti() {
        // Seleziona la prossima desitnazione in maniera casuale
        int r = (int) Random.Range(0, arrayVerticiPercorso.Length);
        GameObject gameObjectDestinazione = arrayVerticiPercorso[r];

        // Estrai le coordinate xy del oggetto
        verticeDestinazioneAttuale = gameObjectDestinazione.transform.position;
        Debug.Log("Prossima fermata" + verticeDestinazioneAttuale);

        // Imposta la prossima destinazione
        agent.SetDestination(verticeDestinazioneAttuale);
    }
}
