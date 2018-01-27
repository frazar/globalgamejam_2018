using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 5;
    const int INTERVALLO_DESTINAZIONE_INVALIDA = 3;

    // Copia incolla dalla documentazione di PolyNav
    private PolyNavAgent _agent;
    private PolyNavAgent agent{
        get {return _agent != null? _agent : _agent = GetComponent<PolyNavAgent>();}
    }

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    public const int VELOCITA_MOVIMENTO = 250;
    private int indiceDestinazioneAttuale = -1;
    private int cicliDestinazioneInvalida = 0;

    //valore dell'infezione del contadino
    private int infezione;
    private bool morto;


    void Start () {
        //setto i parametri del contadino
        this.infezione = 0;
        this.morto = false;

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
        // if (this.morto) { 
        //     //cosa succede se il contadino muore?
        // }
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


    IEnumerator waitAndMuoviti() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_DESTINAZIONE_INVALIDA);

        muoviti();
    }

    void muovitiDestinazioneInvalida() {
        cicliDestinazioneInvalida++;
        Debug.Log("Destinazione invalida" + arrayVerticiPercorso[indiceDestinazioneAttuale] + "(volta #" + cicliDestinazioneInvalida + ")");

        StartCoroutine(waitAndMuoviti());        
    }

    void muovitiDestinazioneValida() {
        cicliDestinazioneInvalida = 0;
        muoviti();
    }

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;
        if (this.infezione >= 100) {
            this.morto = true;
        }
    }

    // Cosa succede se entro in un edificio
    IEnumerator OnTriggerEnter2D(Collider2D collider2D) { 
        Debug.Log("Entro in " + collider2D.gameObject.tag);

        if (collider2D.gameObject.tag == GestoreTag.Edifici) {
            Edificio edificio = collider2D.gameObject.GetComponentInParent<Edificio>();
            Debug.Log("Entro in " + edificio);

            // Aumenta l'infezione del contadino
            int valoreInfezioneEdificio = edificio.valoreInfezione;
            aumentaInfezione(valoreInfezioneEdificio);

            // Fai sparire temporaneamnete lo sprite del contadino
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false; // Disabilita la box così che anche altri contadinin possano entrare

            // Stoppa il navigatore
            agent.Stop();

            // Aspetta un intervallo di tempo prima di far riapparire il contadino
            yield return new WaitForSeconds(INTERVALLO_CONTADINO_IN_CASA);

            // Fai riapparire lo sprite e riprendi il percorso
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
            muoviti();
        }
    }
}
