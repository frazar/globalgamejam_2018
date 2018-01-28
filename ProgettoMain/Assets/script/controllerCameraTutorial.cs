using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerCameraTutorial : MonoBehaviour
{

    private Vector3 pos1, pos2;

    public GameObject palyer1, player2;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos1 = palyer1.transform.position;
        Vector3 pos2 = player2.transform.position;
        Vector3 media = new Vector3((pos1.x + pos2.x) / 2,540, -10);
        transform.position = media;
    }
}
