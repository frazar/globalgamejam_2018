using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HidePathVertex : MonoBehaviour {

	void Start () {
        if (Application.isPlaying) {
            GetComponent<SpriteRenderer>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
