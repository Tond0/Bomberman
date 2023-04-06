using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bomba : MonoBehaviour
{
    [SerializeField] private int range;
    [SerializeField] private float previewTime;
    [SerializeField] private float previewWarnings;

    private Sequence sequenzaAnimazione;

    [HideInInspector] public int posX;
    [HideInInspector] public int posY;

    // Start is called before the first frame update
    void Start()
    {
        PreviewAnimation();
    }

    void PreviewAnimation()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        float fadeDuration = previewTime / (previewWarnings * 2);

        sequenzaAnimazione = DOTween.Sequence();
        sequenzaAnimazione.SetAutoKill(false);

        for (int i = 0; i < previewWarnings; i++)
        {
            //Cambiamo il colore della bomba
            sequenzaAnimazione.Append(sprite.DOColor(Color.red, fadeDuration));
            sequenzaAnimazione.Append(sprite.DOColor(Color.black, fadeDuration));
        }

        sequenzaAnimazione.onComplete = EsplosionePazzesca;

        sequenzaAnimazione.Play();
    }

    void EsplosionePazzesca()
    {
        
        for(int direzioni = 0; direzioni < 4; direzioni++)
        {
            for(int cella = 1; cella <  range + 1; cella++)
            {
                switch (direzioni)
                {
                    case 0:

                        if (griglia[posX, posY + cella].tileType == Tile.TileType.Muro_Distruttibile
                            || griglia[posX, posY + cella].tileType == Tile.TileType.Player
                                || griglia[posX, posY + cella].tileType == Tile.TileType.PlayerSuBomba)
                        {
                            GridManager.instance.ChangeTile(posX, posY + cella, Tile.TileType.Pavimento);
                            cella = range;
                        }
                        else if (griglia[posX, posY + cella].tileType == Tile.TileType.Muro_Indistruttibile)
                        {
                            cella = range;
                        }

                        break;
                    case 1:

                        if (griglia[posX, posY - cella].tileType == Tile.TileType.Muro_Distruttibile
                            || griglia[posX, posY - cella].tileType == Tile.TileType.Player
                                || griglia[posX, posY - cella].tileType == Tile.TileType.PlayerSuBomba)
                        {
                            GridManager.instance.ChangeTile(posX, posY - cella, Tile.TileType.Pavimento);
                            cella = range;
                        }
                        else if (griglia[posX, posY - cella].tileType == Tile.TileType.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;
                    case 2:

                        if (griglia[posX + cella, posY].tileType == Tile.TileType.Muro_Distruttibile)
                        {
                            GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);
                            cella = range;
                        }
                        else if (griglia[posX + cella, posY].tileType == Tile.TileType.Player
                                || griglia[posX + cella, posY].tileType == Tile.TileType.PlayerSuBomba
                                    || griglia[posX + cella, posY].tileType == Tile.TileType.PlayerSuBomba)
                        {
                            GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);

                        }
                        else if (griglia[posX + cella, posY].tileType == Tile.TileType.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;
                    case 3:

                        cella = CheckEsplosione(posX - cella, posY, cella);

                        if (griglia[posX - cella, posY].tileType == Tile.TileType.Muro_Distruttibile
                            || griglia[posX - cella, posY].tileType == Tile.TileType.Player
                                || griglia[posX - cella, posY].tileType == Tile.TileType.PlayerSuBomba)
                        {
                            GridManager.instance.ChangeTile(posX - cella, posY, Tile.TileType.Pavimento);
                            cella = range;
                        }
                        else if (griglia[posX - cella, posY].tileType == Tile.TileType.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;

                }
            }
        }

        GridManager.instance.ChangeTile(posX, posY, Tile.TileType.Pavimento);
        Destroy(gameObject);
        
    }

    int CheckEsplosione(int x, int y, int cella)
    {
        var grigliaTileType = GridManager.instance.GrigliaTile[x,y].tileType;

        switch (grigliaTileType)
        {
            case Tile.TileType.Muro_Distruttibile:
                GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);
                cella = range;
                break;


            //Nel caso colpisse il giocatore 

            case Tile.TileType.Player:
                //GameOver
                GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);
                break;

            case Tile.TileType.PlayerSuBomba:
                GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);
                break;



            case Tile.TileType.Nemico:
                GridManager.instance.ChangeTile(posX + cella, posY, Tile.TileType.Pavimento);
                break;

                case Tile.TileType.Muro_Indistruttibile:
                cella = range;
                break;
        }

        return cella;
    }
}
