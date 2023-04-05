using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bomba : MonoBehaviour
{
    [SerializeField] private int range;
    [SerializeField] private int previewTime;
    [SerializeField] private int previewWarnings;

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

        float fadeDuration = previewTime / previewWarnings;

        sequenzaAnimazione = DOTween.Sequence();
        sequenzaAnimazione.SetAutoKill(false);

        for (int i = 0; i < previewWarnings; i++)
        {
            //Cambiamo il colore della bomba
            sequenzaAnimazione.Append(sprite.DOColor(Color.red, 0.6f));
            sequenzaAnimazione.Append(sprite.DOColor(Color.black, 0.4f));
        }

        sequenzaAnimazione.onComplete = EsplosionePazzesca;

        sequenzaAnimazione.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void EsplosionePazzesca()
    {

        var griglia = GridManager.instance.GrigliaOggetti;

        for(int direzioni = 0; direzioni < 4; direzioni++)
        {
            for(int cella = 1; cella < range; cella++)
            {
                switch(direzioni)
                {
                    case 0:

                        if (griglia[posX, posY + cella].CompareTag("bombabile"))
                        {
                            GridManager.instance.DistruzioneMuro(posX, posY - cella, griglia[posX, posY - cella].transform.parent.gameObject);
                            Destroy(griglia[posX, posY + cella]);
                            Debug.Log(griglia[posY, posX + cella].name + " in [" + posY + " " + (posX + cella) + "]");
                        }
                        else if (griglia[posX, posY + cella] == GridManager.instance.Muro_Indistruttibile)
                        {
                            cella = range;
                        }

                        break;
                    case 1:

                        if (griglia[posX, posY - cella].CompareTag("bombabile"))
                        {
                            GridManager.instance.DistruzioneMuro(posX, posY - cella, griglia[posX, posY - cella].transform.parent.gameObject);
                            Debug.Log(griglia[posX, posY - cella].name + " in [" + posY + " " + (posX - cella) + "]");
                            Destroy(griglia[posX, posY - cella]);
                        }
                        else if (griglia[posX, posY - cella] == GridManager.instance.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;
                    case 2:

                        if (griglia[posX + cella, posY].CompareTag("bombabile"))
                        {
                            GridManager.instance.DistruzioneMuro(posX + cella, posY, griglia[posX + cella, posY].transform.parent.gameObject);
                            Destroy(griglia[posX + cella, posY]);
                            Debug.Log(griglia[posX + cella, posY].name + " in [" + posY + cella + " " + posX + "]");
                        }
                        else if (griglia[posX + cella, posY] == GridManager.instance.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;
                    case 3:

                        if (griglia[posX - cella, posY].CompareTag("bombabile"))
                        {
                            Debug.Log(griglia[posX - cella, posY].name + " in [" + (posY - cella) + " " + posX + "]");
                            GridManager.instance.DistruzioneMuro(posX - cella, posY, griglia[posX - cella, posY].transform.parent.gameObject);
                            Destroy(griglia[posX - cella, posY]);
                        }
                        else if (griglia[posX - cella, posY] == GridManager.instance.Muro_Indistruttibile)
                        {
                            cella = range;
                        }
                        break;
                }
            }
        }

        griglia[posX, posY] = GridManager.instance.Pavimento;
        Destroy(gameObject);
    }
}
