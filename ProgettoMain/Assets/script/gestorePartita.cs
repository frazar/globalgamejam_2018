using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestorePartita {

    public static GameObject menuwin;
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

    public static void AggingiNPCMorto()
    {
        npcMorti++;
        if (npcMorti == numeroNPC)
        {
            GameObject.Find("Canvas").SendMessage("Vittoria");
          
        }
    }

    
}
