using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 5;
    const int INTERVALLO_DESTINAZIONE_INVALIDA = 3;

    // Copia incolla dalla documentazione di PolyNav
    private PolyNavAgent _agent;
    private PolyNavAgent agent{
        get {return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>();}
    }

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    private int indiceDestinazioneAttuale = -1;
    private int cicliDestinazioneInvalida = 0;

    // Valore dell'infezione del contadino
    private int infezione = 0;
    private bool morto = false;

    void Start () {
        // Per avere un percorso sensato, servono almeno due punti
        Assert.IsTrue(arrayVerticiPercorso.Length >= 2); 

        // Aggiungi i callback
        agent.OnDestinationReached += muovitiDestinazioneValida;
        agent.OnDestinationInvalid += muovitiDestinazioneInvalida;

        // Parti con il primo movimento
        muoviti();
    }
    
    // Update is called once per frame
    void Update () {
    }

    void muoviti() {
        // Aggiorna indiceDestinazioneAttuale con un numero random che sia tra 0 
        // e arrayVerticiPercorso.Length, e che sia diverso dal valore attuale
        while (true) {
            int r = (int) Random.Range(0, arrayVerticiPercorso.Length);
            if (indiceDestinazioneAttuale != r) {
                indiceDestinazioneAttuale = r; 
                break;            
            }
        }

        // Seleziona la prossima desitnazione in maniera casuale
        GameObject gameObjectDestinazione = arrayVerticiPercorso[indiceDestinazioneAttuale];

        // Estrai le coordinate xy del oggetto
        Vector2 verticeDestinazioneAttuale = gameObjectDestinazione.transform.position;

        // Imposta la prossima destinazione
        agent.SetDestination(verticeDestinazioneAttuale);
    }

    IEnumerator aspettaERiprova() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_DESTINAZIONE_INVALIDA);

        muoviti();
    }

    // Eseguito quando la destinazione impostata è invalida, irraggiungibile 
    void muovitiDestinazioneInvalida() {
        Debug.Log("Destinazione invalida: '" + arrayVerticiPercorso[indiceDestinazioneAttuale] + "' (tentativo #" + cicliDestinazioneInvalida + ")");

        StartCoroutine(aspettaERiprova());
    }

    IEnumerator aspettaInEdificio() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_CONTADINO_IN_CASA);

        // Fai riapparire lo sprite
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        // Setta una nuova destinazione
        muoviti();
    }

    // Eseguito quando una destinazione valida viene raggiunta
    void muovitiDestinazioneValida() {
        GameObject posizioneRaggiunta = arrayVerticiPercorso[indiceDestinazioneAttuale];

        if (posizioneRaggiunta.tag == GestoreTag.Edifici) {
            Edificio edificio = posizioneRaggiunta.GetComponentInParent<Edificio>();
            Debug.Log("Entro in '" + edificio + "'");

            // Aumenta l'infezione del contadino
            int valoreInfezioneEdificio = edificio.valoreInfezione;
            aumentaInfezione(valoreInfezioneEdificio);

            // Fai sparire temporaneamnete lo sprite del contadino
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false; // Disabilita la box così che anche altri contadinin possano entrare

            // Stoppa il navigatore
            agent.Stop();

            // Fai partire il countdown
            StartCoroutine(aspettaInEdificio());            
        } else {
            muoviti();
        }
    }

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;
        if (this.infezione >= 100) {
            this.morto = true;
        }
    }
}
