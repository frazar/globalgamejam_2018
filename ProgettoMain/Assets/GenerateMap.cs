using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNav2D))]
public class GenerateMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PolyNav2D.current.GenerateMap();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
