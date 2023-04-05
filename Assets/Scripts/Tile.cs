using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Pavimento, Muro_Distruttibile, Muro_Indistruttibile, Player, Bomba }
    public TileType tileType;

    [SerializeField] private Color Pavimento;
    [SerializeField] private Color Muro_Distruttibile;
    [SerializeField] private Color Muro_Indistruttibile;

    // Start is called before the first frame update
    void Start()
    {
        ColoreTile();
    }

    void ColoreTile()
    {
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


        }
    }

    private void OnValidate()
    {
        ColoreTile();
    }
}
