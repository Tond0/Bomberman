using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : IMovement, IPlaceBomb
{
    public Vector2Int movimento { get; private set; }

    public bool wantToPlace { get; private set; }


    public bool ReadInputBomb()
    {
        return Input.GetAxisRaw("Fire1") != 0;
    }

    public void ReadInputMove()
    {
        movimento = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
    }
}
