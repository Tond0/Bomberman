using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Pavimento, Muro_Distruttibile, Muro_Indistruttibile, Player, Bomba, PlayerSuBomba, Nemico }
    public TileType tileType;

    [SerializeField] private Color Pavimento;
    [SerializeField] private Color Muro_Distruttibile;
    [SerializeField] private Color Muro_Indistruttibile;
    //Per Debug
    [SerializeField] private Color Player;

    public void ChangeTile(TileType tileType)
    {
        this.tileType = tileType;

        switch (tileType)
        {
            case TileType.Pavimento:
                GetComponent<SpriteRenderer>().color = Pavimento;
                break;

            case TileType.Muro_Distruttibile:
                GetComponent<SpriteRenderer>().color = Muro_Distruttibile;
                break;

            case TileType.Muro_Indistruttibile:
                GetComponent<SpriteRenderer>().color = Muro_Indistruttibile;
                break;

            case TileType.Player:
                GetComponent<SpriteRenderer>().color = Player;
                break;


        }
    }
}
