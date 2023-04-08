using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : IMovement
{
    public Vector2Int movimento { get; private set; }

    public void ReadInputMove()
    {
        //50% di probabilità che si muova o in avanti o indietro
        int direzione = Random.value > 0.5f ? 1 : -1;

        //50% di probabilità che si muova sull'asse delle x o delle y
        if(Random.value >= 0.5f)
            movimento = new Vector2Int(direzione, 0);
        else
            movimento = new Vector2Int(0, direzione);
        
    }
}
