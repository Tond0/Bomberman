using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    Vector2Int movimento { get; }

    void ReadInputMove();
}

public interface IPlaceBomb
{
    bool wantToPlace { get; }
    bool ReadInputBomb();
}
