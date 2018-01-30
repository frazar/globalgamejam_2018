using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestorePartita {

   
    static int numeroNPC;
    static int npcInfetti;
    static int npcMorti;

    public static void AggingiNPC()
    {
        numeroNPC++;
    }

    public static void AggiungiNPCInfetto() {
        npcInfetti++;
    }
    public static void AggiuntinpcMorto()
    {
        npcMorti++;
        if (npcMorti ==numeroNPC)
        {
            Debug.Log("VITTORIA");  
        }
    }
}
