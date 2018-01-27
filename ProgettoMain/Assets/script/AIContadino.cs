using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(SpriteRenderer))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 5;

    // Copia incolla dalla documentazione di PolyNav
    private PolyNavAgent _agent;
    private PolyNavAgent agent{
        get {return _agent != null? _agent : _agent = GetComponent<PolyNavAgent>();}
    }

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    private Vector2 verticeDestinazioneAttuale;


    //valore dell'infezione del contadino
    private int infezione;
    private bool morto;

    //valori velocita del contadino
    public const int VELOCITA_MOVIMENTO = 250;
    public float moltiplicatoreVelocita;
    public int velocita; 

    void Start () {
        //setto i parametri del contadino
        this.infezione = 0;
        this.morto = false;
        this.moltiplicatoreVelocita = 1.2f;
        this.velocita = (int) moltiplicatoreVelocita * VELOCITA_MOVIMENTO;

        // Per avere un percorso sensato, servono almeno due punti
        Assert.IsTrue(arrayVerticiPercorso.Length >= 2); 

        // Aggiungi i callback
        agent.OnDestinationReached += muoviti;
        agent.OnDestinationInvalid += muovitiDestinazioneInvalida;

        // Parti con il primo movimento
        muoviti();
    }
    
    // Update is called once per frame
    void Update () {
        if (this.morto) { 
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite tomba = Resources.Load("tomba") as Sprite;
            spriteRenderer.sprite = tomba;
            agent.Stop();
        }
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

        // Imposta la prossima destinazione
        agent.SetDestination(verticeDestinazioneAttuale);
    }

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;
        if (this.infezione >= 100) {
            this.morto = true;
        }
        if (this.infezione >=50){
            this.moltiplicatoreVelocita = 0.8f;
            this.velocita = (int) this.moltiplicatoreVelocita * VELOCITA_MOVIMENTO;
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

            // Stoppa il navigatore
            agent.Stop();

            // Aspetta un intervallo di tempo prima di far riapparire il contadino
            yield return new WaitForSeconds(INTERVALLO_CONTADINO_IN_CASA);

            // Fai riapparire lo sprite e riprendi il percorso
            GetComponent<SpriteRenderer>().enabled = true;
            muoviti();
        }
    }
}
