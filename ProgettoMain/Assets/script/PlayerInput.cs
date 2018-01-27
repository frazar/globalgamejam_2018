using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {

    // Use this for initialization    
    [Range(1,2)]
    public int Player = 1;
    public float Moltiplicatore; // moltiplicagtore del movimento
    Rigidbody2D phy;
    float TempoIterazioneIniziale;
	void Start () {
        phy = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movimento = Vector3.zero;

        if (Input.GetButton("su" + Player))
        {
            movimento += Vector3.up * Time.deltaTime;
        }

        if (Input.GetButton("sotto" + Player)) 
        {
            movimento += Vector3.down * Time.deltaTime;
        }
        if (Input.GetButton("destra" + Player))
        {
            gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);
            movimento += Vector3.right * Time.deltaTime;
        }
        if (Input.GetButton("sinistra" + Player))
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
                ProcessoInfezioneEdifici(ColliderIn.gameObject);
            break;
        }
    }

    void ProcessoInfezioneEdifici(GameObject Edificio)
    {
        
        if (Input.GetButtonDown("azione" + Player))
        {
            // appena premuto il tasto azione
          
            TempoIterazioneIniziale = Time.time;
        }
        
        if (Input.GetButton("azione" + Player))
        {
          
            // Il tasto viene tenuto giù 

            Edificio edificio = Edificio.GetComponentInParent<Edificio>();
            if (Time.time - TempoIterazioneIniziale >= edificio.SecondiPerInfezione && !edificio.infetto)
            {   
                edificio.infetto = true;
            }
        }
    }

}
