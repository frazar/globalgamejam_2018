using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {

    // Use this for initialization    
    [Range(1,2)]
    public int playerIndex = 1;
    [Range(1,2000)]
    public float Moltiplicatore; // moltiplicagtore del movimento
    Rigidbody2D phy;
    float TempoIterazioneIniziale;
	void Start () {
        phy = GetComponent<Rigidbody2D>();

        // Check that the playerIndex property was set correctly
        Assert.IsTrue(this.name == "player" + playerIndex);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movimento = Vector3.zero;

        if (Input.GetButton("su" + playerIndex))
        {
            movimento += Vector3.up * Time.deltaTime;
        }

        if (Input.GetButton("sotto" + playerIndex)) 
        {
            movimento += Vector3.down * Time.deltaTime;
        }
        if (Input.GetButton("destra" + playerIndex))
        {
            gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);
            movimento += Vector3.right * Time.deltaTime;
        }
        if (Input.GetButton("sinistra" + playerIndex))
        {
            gameObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
            movimento += Vector3.left * Time.deltaTime;
        }
        phy.velocity= movimento * Moltiplicatore;
	}

    void OnTriggerStay2D(Collider2D ColliderIn)
  	{
    
        switch (ColliderIn.gameObject.tag)
        {
            
            case GestoreTag.Edifici:    
                //
                ProcessoInfezioneEdifici(ColliderIn.gameObject);
            break;
        }
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
            Debug.Log("Infettato");

            Edificio edificio = Edificio.GetComponentInParent<Edificio>();
            if (Time.time - TempoIterazioneIniziale >= edificio.SecondiPerInfezione && !edificio.infetto)
            {   
                edificio.infetta();
            }
        }
    }

}
