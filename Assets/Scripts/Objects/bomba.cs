using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bomba : MonoBehaviour
{
    [SerializeField] private int range;
    [SerializeField] private float previewTime;
    [SerializeField] private float previewWarnings;

    private Sequence sequenzaAnimazioneBomba;
    private Sequence sequenzaAnimazioneEsplosione;

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

        sequenzaAnimazioneBomba = DOTween.Sequence();
        sequenzaAnimazioneBomba.SetAutoKill(false);

        for (int i = 0; i < previewWarnings; i++)
        {
            //Cambiamo il colore della bomba
            sequenzaAnimazioneBomba.Append(sprite.DOColor(Color.red, fadeDuration));
            sequenzaAnimazioneBomba.Append(sprite.DOColor(Color.black, fadeDuration));
        }

        sequenzaAnimazioneBomba.onComplete = EsplosionePazzesca;

        sequenzaAnimazioneBomba.Play();
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

                        cella = CheckEsplosione(posX, posY + cella, cella);
                        break;

                    case 1:

                        cella = CheckEsplosione(posX, posY - cella, cella);
                        break;

                    case 2:

                        cella = CheckEsplosione(posX + cella, posY, cella);
                        break;

                    case 3:

                        cella = CheckEsplosione(posX - cella, posY, cella);
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
                GridManager.instance.ChangeTile(x, y, Tile.TileType.Pavimento);
                cella = range;
                break;


            //Nel caso colpisse il giocatore 
            //GameOver

            case Tile.TileType.Player:
                GridManager.instance.ChangeTile(x, y, Tile.TileType.Pavimento);
                break;

            case Tile.TileType.PlayerSuBomba:
                GridManager.instance.ChangeTile(x, y, Tile.TileType.Pavimento);
                break;



            case Tile.TileType.Nemico:
                GridManager.instance.ChangeTile(x, y, Tile.TileType.Pavimento);
                break;

            case Tile.TileType.Muro_Indistruttibile:
                cella = range;
                break;
        }

        return cella;
    }
}
