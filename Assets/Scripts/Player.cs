using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEditor;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("La velocità tra una casella è l'altra")] private float speed;
    private Vector2 movimento;
    private Vector2 destinazione;

    [HideInInspector] public GridManager griglia;

    private int x;
    private int y;

    private bool moving;

    [Header("Impostazioni bombe")]
    [SerializeField] private GameObject bombaPrefab;
    // Start is called before the first frame update
    void Start()
    {
        x = (int)griglia.PosizioneSpawn.x;
        y = (int)griglia.PosizioneSpawn.y;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        var GrigliaNextPos = griglia.GrigliaPosizioni[x + (int)movimento.x, y - (int)movimento.y];
        if (movimento != Vector2.zero && !moving && CanMoveThere())
        {
            moving = true;
            destinazione = movimento;
            transform.DOMove(GrigliaNextPos, speed / 60).onComplete = SetNewPosition;
        }
    }

    bool CanMoveThere()
    {
        var OggettoNextPosition = griglia.GrigliaOggetti[x + (int)movimento.x, y - (int)movimento.y];

        if (OggettoNextPosition == griglia.Ostacolo || OggettoNextPosition == griglia.Muro_Indistruttibile || OggettoNextPosition.name == bombaPrefab.name)
            return false;
        else
            return true;
    }

    //Aggiorniamo la griglia su dove si trova il giocatore.
    void SetNewPosition()
    {
        //Nuova posizione del giocatore nella griglia
        griglia.GrigliaOggetti[x + (int)destinazione.x, y - (int)destinazione.y] = gameObject;
        //Rimettiamo il pavimento al suo posto
        if (griglia.GrigliaOggetti[x, y].name != bombaPrefab.name)
            griglia.GrigliaOggetti[x, y] = griglia.Pavimento;
        //Informiamo il player della sua nuova posizione
        x += (int)destinazione.x;
        y -= (int)destinazione.y;

        moving = false;
    }

    public void Raccolta_Input_Movimento(InputAction.CallbackContext ctx)
    {
        movimento = ctx.ReadValue<Vector2>();
    }

    public void Raccolta_Input_PlaceBomb(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject bomba = Instantiate(bombaPrefab, transform.position, Quaternion.Euler(0, 0, 45));
            bomba.name = bombaPrefab.name;
            bomba.GetComponent<bomba>().posX = x;
            bomba.GetComponent<bomba>().posY = y;
            griglia.GrigliaOggetti[x, y] = bomba;
        }
    }
}
