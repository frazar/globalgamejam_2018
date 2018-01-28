using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolyNavAgent))]
public class ricercaEdInseguimento : MonoBehaviour {

    // Use this for initialization
     static bool ricercati=true;// se true i player vengono attaccati
    PolyNavAgent agent;
    Rigidbody2D phy;
    GameObject target;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            // il player è nel fov adesso devo controllare se è visibile
            if (target==null || target==collision.gameObject) {
                target = collision.gameObject;
                Vector3 posizionePlayer = Vector3.zero;

                if (playerVisibile(collision.gameObject, ref posizionePlayer))
                {
                    // player visibile 
                    if (ricercati)
                    {
                        // attacco il player

                        this.gameObject.SendMessage("NemicoAvvistato", posizionePlayer);
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

            void Awake()
    {
        agent = GetComponent<PolyNavAgent>();
        phy = GetComponent<Rigidbody2D>();
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
         
                posNemico= hit.point;
                return true;
            }
            
        }
        return false;
    }
}
