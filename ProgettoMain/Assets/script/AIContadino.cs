using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AIContadino : MonoBehaviour {
    const int INTERVALLO_CONTADINO_IN_CASA = 2;
    const int INTERVALLO_DESTINAZIONE_INVALIDA = 3;
    const int INTERVALLO_MORTE = 2;
    const int TIMEOUT_ARRIVO_DESTINAZIONE = 20;
    const float THRESHOLD_DISTANZA_RAGGIUNTA = 0.7f; 
    const float MAX_SPEED_REDUCTION_FRACTION = 0.5f;

    // Copia incolla dalla documentazione di PolyNav
    Animator animazioniController;
    private PolyNavAgent _agent;
    GameObject fov;
    SpriteRenderer spriteController;
    GameObject target;
    private PolyNavAgent agent{
        get {return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>();}
    }
    
    public GameObject tombaAstratto;

    // Array dei vertici del percorso
    public GameObject[] arrayVerticiPercorso; 
    private Vector2 verticeDestinazioneAttuale;

    // Indice dell'elemento di arrayVerticiPercorso settato come prossima destinazione
    private int indiceDestinazioneAttuale = -1;

    // Numero di eventi DestinazioneInvalida consecutivi
    private int counterDestinazioneInvalida = 0;

    // Valore dell'infezione del contadino
    [Range(0, 100)]
    private int infezione = 0;
    private bool morto = false;

    // Flag per la modalità inseguimento
    bool inseguimento = false;
    int direzioneVecchia = -1;


    void Start () {
        Assert.IsNotNull(tombaAstratto);

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
        if (this.morto) { 
        } 
        else if (agent.isActiveAndEnabled )
        {           
            Vector3 dirNemico = agent.movingDirection;
            float rotationZ = Mathf.Atan2(dirNemico.y, dirNemico.x) * Mathf.Rad2Deg;
            fov.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+180);
            gestisciAnimazioni();
        }       
    }

    void gestisciAnimazioni() {
        Vector2 direzioneMovimento = agent.movingDirection;
        bool siMuove = true;
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

    void muori() {
        Debug.Log("'" + this.gameObject.name + "' è morto!");
        
        this.morto = true;            
        
        agent.Stop(); // Ferma il navigatore

        Instantiate(tombaAstratto, 
                    this.gameObject.transform.position, 
                    Quaternion.identity);

        // Deactivate the contadino gameObject               
        this.gameObject.SetActive(false); // Disattiva il fov 
    }

    IEnumerator aspettaEMuori() {
        yield return new WaitForSeconds(INTERVALLO_MORTE + INTERVALLO_CONTADINO_IN_CASA);

        muori();
    }

    void aumentaInfezione(int valoreInfezione) {
        this.infezione += valoreInfezione;

        if (this.infezione >= 100) {
            StartCoroutine(aspettaEMuori());
        } 

        Debug.Log("'" + this.gameObject.name + "' ha ora livello infezione " + this.infezione);
        aggiornaVelocitaMovimento();
    }

    IEnumerator aspettaERiprova() {
        // Aspetta un intervallo di tempo prima di ripartire dopo una destinazione invalida
        yield return new WaitForSeconds(INTERVALLO_DESTINAZIONE_INVALIDA);

        if (!this.morto) {
            muoviti();
        }

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

        if (!this.morto) {
            // Fai riapparire lo sprite
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            fov.SetActive(true);

            // Setta una nuova destinazione
            muoviti();
        }
    }

    // Eseguito quando una destinazione valida viene raggiunta
    // ATTENZIONE: se la destinazione impostata non è valida, il navigatore 
    // imposta una definizione valida più vicina come destinazione. 
    // Una volta raggiunta questa, muovitiDestinazioneValida() viene chiamata, 
    // ma la destinazione raggiunta non è esattamente quella impostata 
    // originariamente!
    void muovitiDestinazioneValida() {
        if (morto) return;

        counterDestinazioneInvalida = 0;
        if (inseguimento)
        {
            // raggiunto ultimo punto conosciuto del nemico 
            muoviti();
        }
        else
        {
            GameObject desinazioneImpostata = arrayVerticiPercorso[indiceDestinazioneAttuale];
            // Calcola la distanza effettiva dalla destinazione impostata
            float disttanzaDaDestinazione = (desinazioneImpostata.transform.position - this.gameObject.transform.position).magnitude;
            

            if (desinazioneImpostata.tag == GestoreTag.Edifici && disttanzaDaDestinazione < THRESHOLD_DISTANZA_RAGGIUNTA)
            {
                Edificio edificio = desinazioneImpostata.GetComponentInParent<Edificio>();
                Debug.Log("Entro in '" + edificio + "'");

                // Aumenta l'infezione del contadino se l'edificio è infetto
                if (edificio.infetto) 
                {
                    int valoreInfezioneEdificio = edificio.valoreInfezione;
                    aumentaInfezione(valoreInfezioneEdificio);
                }

                // Fai sparire temporaneamnete lo sprite del contadino
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
                fov.SetActive(false);
                // Stoppa il navigatore
                
                agent.Stop();

                // Fai partire il countdown
                StartCoroutine(aspettaInEdificio());
            }
            else
            {
                Debug.Log("'" + this.gameObject.name + "' non è davvero arrivato, la sua distanza dalla destinazione è " + disttanzaDaDestinazione);
                muoviti();
            }
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

    // Aggiorna la velocità di movimento del contadino in base a modalità 
    // (inseguimento o non) e livello malattia 
    void aggiornaVelocitaMovimento() {

        if (inseguimento) {
            agent.maxSpeed = 5.5f;
            agent.slowingDistance = 0;
        } else {
            agent.maxSpeed = 3.5f;
            agent.slowingDistance = 2;
        }

        agent.maxSpeed *= 1f - MAX_SPEED_REDUCTION_FRACTION * this.infezione / 100;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            // il player è nel fov adesso devo controllare se è visibile
            if (target == null || target == collision.gameObject) {
                target = collision.gameObject;
                Vector3 posizionePlayer = Vector3.zero;

                if (playerVisibile(collision.gameObject, ref posizionePlayer))
                {
                    // player visibile 
                    if (inseguimento)
                    {
                        // attacco il player
                        NemicoAvvistato(posizionePlayer);
                    }
                }
                else
                {
                    target = null;
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            if (collision.gameObject == target)
            {
                target = null;
            }         
        }
    }

    bool playerVisibile(GameObject player, ref Vector3 posNemico)
    {
        Vector2 direzione = player.transform.position-transform.position;
        //Debug.DrawRay(transform.position,direzione,Color.red,4);
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position,direzione);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("player"))
            {
                // il player è in vista       
         
                posNemico = hit.point;
                return true;
            }
            
        }
        return false;
    }
}
