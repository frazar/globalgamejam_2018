using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(SpriteRenderer))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 5;
    const int INTERVALLO_DESTINAZIONE_INVALIDA = 3;

    // Copia incolla dalla documentazione di PolyNav
    private PolyNavAgent _agent;
    GameObject fov;
    private PolyNavAgent agent{
        get {return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>();}
    }

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    private Vector2 verticeDestinazioneAttuale;

    // Indice dell'elemento di arrayVerticiPercorso settato come prossima destinazione
    private int indiceDestinazioneAttuale = -1;

    // Numero di eventi DestinazioneInvalida consecutivi
    private int counterDestinazioneInvalida = 0;

    // Valore dell'infezione del contadino
    private int infezione = 0;
    private bool morto = false;

    // Flag per la modalità inseguimento
    bool inseguimento = false;

    void Start () {
        // Per avere un percorso sensato, servono almeno due punti
        Assert.IsTrue(arrayVerticiPercorso.Length >= 2);

        fov = gameObject.transform.Find("fov").gameObject;
        
        // Verifica che tutti i punti siano impostati
        for (int i = 1; i < arrayVerticiPercorso.Length; i++) {
            Assert.IsNotNull(arrayVerticiPercorso[i]);
        }

        // Aggiungi i callback
        agent.OnDestinationReached += muovitiDestinazioneValida;
        agent.OnDestinationInvalid += muovitiDestinazioneInvalida;

        // Parti con il primo movimento
        muoviti();
    }
    
    // Update is called once per frame
    void Update () {
        if (this.morto) { 
            agent.Stop();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite tomba = Resources.Load("tomba") as Sprite;
            spriteRenderer.sprite = tomba;
        }
    }

    void muovitiDestinazioneInvalida() {
        Debug.Log("Destinazione invalida" + verticeDestinazioneAttuale);
        muoviti();
        if (agent.isActiveAndEnabled )
        {
           
            Vector3 dirNemico = agent.movingDirection;
           // fov.transform.rotation = Quaternion.Euler(dirNemico);
            float rotationZ = Mathf.Atan2(dirNemico.y, dirNemico.x) * Mathf.Rad2Deg;
            fov.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+180);
        }
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

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;
        if (this.infezione >= 100) {
            this.morto = true;
        } 
    }

    IEnumerator aspettaERiprova() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_DESTINAZIONE_INVALIDA);

        muoviti();
    }

    // Eseguito quando la destinazione impostata è invalida, irraggiungibile 
    void muovitiDestinazioneInvalida() {
        counterDestinazioneInvalida ++;
        Debug.Log("Destinazione invalida: '" + arrayVerticiPercorso[indiceDestinazioneAttuale] + "' (tentativo #" + counterDestinazioneInvalida + ")");

        StartCoroutine(aspettaERiprova());
    }

    IEnumerator aspettaInEdificio() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_CONTADINO_IN_CASA);

        // Fai riapparire lo sprite
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        fov.SetActive(true);
        // Setta una nuova destinazione
        muoviti();
    }

    // Eseguito quando una destinazione valida viene raggiunta
    void muovitiDestinazioneValida() {
        counterDestinazioneInvalida = 0;
        if (!inseguimento)
        {
            GameObject posizioneRaggiunta = arrayVerticiPercorso[indiceDestinazioneAttuale];
            if (posizioneRaggiunta.tag == GestoreTag.Edifici)
            {
                Edificio edificio = posizioneRaggiunta.GetComponentInParent<Edificio>();
                Debug.Log("Entro in '" + edificio + "'");

                // Aumenta l'infezione del contadino
                int valoreInfezioneEdificio = edificio.valoreInfezione;
                aumentaInfezione(valoreInfezioneEdificio);

                // Fai sparire temporaneamnete lo sprite del contadino
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                fov.SetActive(false);
                // Stoppa il navigatore
                
                agent.Stop();

                // Fai partire il countdown
                StartCoroutine(aspettaInEdificio());
            }
            else
            {
                muoviti();
            }
        }else
        {
            // raggiunto ultimo punto conosciuto del nemico 
            
            muoviti();
        }
    }

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;
        if (this.infezione >= 100) {
            this.morto = true;
        }
    }

    void NemicoAvvistato(Vector3 PosNemico)
    {
        //rincorrro il nemico 
        agent.maxSpeed = 4;
        agent.SetDestination(PosNemico);
   
        inseguimento = true;

    }
}
