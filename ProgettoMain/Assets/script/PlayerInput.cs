using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerInput : MonoBehaviour {

    // Use this for initialization    
    [Range(1,2)]
    public int playerIndex = 1;
    [Range(1,12000)]
    public float Moltiplicatore; // moltiplicagtore del movimento
    Rigidbody2D phy;
    float TempoIterazioneIniziale;
    private GameObject testoSuggerimentoInfezione;

    // Animation
    SpriteRenderer sprite;
    Animator animator;
    int oldDirection = -1;
    int direction = 0;
    bool moving = false;
    bool exit = false;

	void Start () {
        phy = GetComponent<Rigidbody2D>();

        testoSuggerimentoInfezione = GameObject.Find("/Main Camera/Canvas/TestoSuggerimentoInfezione");
        Assert.IsNotNull(testoSuggerimentoInfezione);

        // Check that the playerIndex property was set correctly
        Assert.IsTrue(this.name == "player" + playerIndex);

        sprite = GetComponents<SpriteRenderer>()[0];
        animator = GetComponent<Animator>();
	}


	
	// Update is called once per frame
	void Update () {
        exit = false;

        Vector3 movimento = Vector3.zero;

        if (Input.GetButton("su" + playerIndex))
        {
            // Animazione
            direction = 1;
            moving = true;

            // Movimento
            movimento += Vector3.up;
        }

        if (Input.GetButton("sotto" + playerIndex)) 
        {
            // Animazione
            direction = 0;
            moving = true;

            // Movimento
            movimento += Vector3.down;
        }

        if (Input.GetButton("destra" + playerIndex))
        {
            // Animazione
            direction = 2;
            sprite.flipX = false;
            moving = true;

            // Muovimento
            movimento += Vector3.right;
        }
        if (Input.GetButton("sinistra" + playerIndex))
        {
            // Animazione
            direction = 2;
            sprite.flipX = true;
            moving = true;

            // Movimento
            movimento += Vector3.left;
        }

        if (movimento == Vector3.zero) {
            // Fermo
            moving = false;
            exit = true;
        }

        // Movimento
        phy.velocity = Vector3.Normalize(movimento) * Moltiplicatore  * Time.deltaTime;

        // Animazione
        exit = exit || (oldDirection != direction);
        oldDirection = direction;
        animator.SetBool("Exit", exit);
        animator.SetBool("IsMoving", moving);
        animator.SetInteger("Direction", direction);
	}

    void OnTriggerStay2D(Collider2D ColliderIn)
  	{
    
        switch (ColliderIn.gameObject.tag)
        {
            
            case GestoreTag.Edifici:    
                ProcessoInfezioneEdifici(ColliderIn.gameObject);
            break;
        }
    }

    void OnTriggerEnter2D(Collider2D ColliderIn) {
        switch (ColliderIn.gameObject.tag)
        {
            case GestoreTag.Edifici:  
                // L'edificio può essere infettato, mostra all'utente il suggerimento
                testoSuggerimentoInfezione.GetComponent<Text>().enabled = true;
            break;
        }
    }

    void OnTriggerExit2D(Collider2D ColliderIn) {
        switch (ColliderIn.gameObject.tag)
        {
            case GestoreTag.Edifici:  
                // L'edificio non può più essere infettato, nascondi all'utente il suggerimento
                testoSuggerimentoInfezione.GetComponent<Text>().enabled = false;
            break;
        }        
    }


    private void OnCollisionEnter2D(Collision collision)
    {
      //  if (collision.gameObject.CompareTag("Contadini")) {
      //      if (collision.gameObject.GetComponent<AIContadino>().) { }
        //}   
    }

    void ProcessoInfezioneEdifici(GameObject Edificio)
    {
        
        // Prima pressione del tasto azione, inizializza timer
        if (Input.GetButtonDown("azione" + playerIndex))
        {
            // appena premuto il tasto azione          
            TempoIterazioneIniziale = Time.time;
        } 
        else if (Input.GetButton("azione" + playerIndex))
        {          
            // Il tasto viene tenuto giù 

            Edificio edificio = Edificio.GetComponentInParent<Edificio>();
            if (Time.time - TempoIterazioneIniziale >= edificio.SecondiPerInfezione && !edificio.infetto)
            {   
                Debug.Log("'" + edificio.name + "' è stato infettato!");
                edificio.infetta();
            }
        }
    }

}
