﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class scriptCartelli : MonoBehaviour {
    
    int nofplayer=0;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="player")
        {
            SceneManager.LoadScene("level_1", LoadSceneMode.Single);
        }
    }
    
}
