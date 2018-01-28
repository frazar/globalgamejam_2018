using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 5;
    const int INTERVALLO_DESTINAZIONE_INVALIDA = 3;

    // Copia incolla dalla documentazione di PolyNav
    Animator animazioniController;
    private PolyNavAgent _agent;
    GameObject fov;
    SpriteRenderer spriteController;
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
    bool inseguimento = false;
    int direzioneVecchia = -1;
    void Start () {
        spriteController = GetComponent<SpriteRenderer>();
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
        animazioniController = GetComponent<Animator>();
        // Parti con il primo movimento
        muoviti();
    }
    
    // Update is called once per frame
    void Update () {
        if (agent.isActiveAndEnabled )
        {
            Vector3 dirNemico = agent.movingDirection;
           // fov.transform.rotation = Quaternion.Euler(dirNemico);
            float rotationZ = Mathf.Atan2(dirNemico.y, dirNemico.x) * Mathf.Rad2Deg;
            fov.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+180);
            gestisciAnimazioni();


        }

       
    }

    void gestisciAnimazioni() {
        Vector2 direzioneMovimento = agent.movingDirection;
        bool siMuove=true;
        int direzione = -1;
        bool uscita = false;
        if (direzioneMovimento == Vector2.zero)
        {
            siMuove = false;
        }
        else
        {
            if (Mathf.Abs(direzioneMovimento.x) > Mathf.Abs(direzioneMovimento.y))
            {
                direzione = 2;
                if (direzioneMovimento.x > 0)
                {
                    spriteController.flipX = false;
                }
                else
                {
                    spriteController.flipX = true;
                }

            }else
            {

                if (direzioneMovimento.y > 0)
                {
                    direzione =1;
                }
                else
                {
                    direzione =0;
                }
            }
            uscita = !siMuove || (direzioneVecchia != direzione);
            direzioneVecchia = direzione;
            animazioniController.SetBool("IsMoving",siMuove);
            animazioniController.SetInteger("Direction", direzione);
            animazioniController.SetBool("Exit", uscita);
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

    IEnumerator aspettaERiprova() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_DESTINAZIONE_INVALIDA);

        muoviti();
    }

    // Eseguito quando la destinazione impostata è invalida, irraggiungibile 
    void muovitiDestinazioneInvalida() {
        cicliDestinazioneInvalida ++;
        Debug.Log("Destinazione invalida: '" + arrayVerticiPercorso[indiceDestinazioneAttuale] + "' (tentativo #" + cicliDestinazioneInvalida + ")");

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
        cicliDestinazioneInvalida = 0;
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
                inseguimento = false;
                modalitaStandard();
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
        agent.maxSpeed = 6;
        agent.decelerationRate = 0;
        agent.SetDestination(PosNemico);
        inseguimento = true;

    }

    void modalitaInseguimento()
    {
        agent.maxSpeed = 7;
        agent.slowingDistance = 2;
        
    }

    void CheckAnimazioni()
    {
       
    }

    void modalitaStandard() {
        agent.maxSpeed = 3.5f;
        agent.slowingDistance = 2;
    }
}
