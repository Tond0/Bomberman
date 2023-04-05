using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEditor;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("La velocità tra una casella è l'altra")] private float speed;
    public Vector2Int movimento;
    private Vector2Int destinazione;

    [HideInInspector] public GridManager griglia;

    [SerializeField] private int x;
    [SerializeField] private int y;

    private bool moving;

    [Header("Impostazioni bombe")]
    [SerializeField] private GameObject bombaPrefab;
    // Start is called before the first frame update
    void Start()
    {
        x = griglia.PosizioneSpawn.x;
        y = griglia.PosizioneSpawn.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Non può muoversi in diagonale
        if(movimento.x + movimento.y <= 1 && movimento.x + movimento.y >= -1)
            Movement();
    }

    void Movement()
    {
        var GrigliaNextPos = griglia.GrigliaTile[x + movimento.x, y - movimento.y];
        if (movimento != Vector2.zero && !moving && CanMoveThere())
        {
            Debug.Log(y + movimento.y + " " + (x + movimento.x));
            moving = true;
            destinazione = movimento;
            transform.DOMove(new Vector2(transform.position.x + movimento.x, transform.position.y + movimento.y), speed / 60).onComplete = SetNewPosition;
        }
    }

    bool CanMoveThere()
    {
        var OggettoNextPosition = griglia.GrigliaTile[y - movimento.y, x + movimento.x];

        if (OggettoNextPosition == Tile.TileType.Pavimento)
            return true;
        else
            return false;
    }

    //Aggiorniamo la griglia su dove si trova il giocatore.
    void SetNewPosition()
    {
        griglia.GrigliaTile[y, x] = Tile.TileType.Pavimento;
        griglia.GrigliaTile[x + movimento.x, y - movimento.y] = Tile.TileType.Player;

        x += destinazione.x;
        y -= destinazione.y;

        moving = false;
    }

    public void Raccolta_Input_Movimento(InputAction.CallbackContext ctx)
    {
        movimento = Vector2Int.RoundToInt(ctx.ReadValue<Vector2>());
    }

    public void Raccolta_Input_PlaceBomb(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject bomba = Instantiate(bombaPrefab, transform.position, Quaternion.Euler(0, 0, 45));
            bomba.name = bombaPrefab.name;
            bomba.GetComponent<bomba>().posX = x;
            bomba.GetComponent<bomba>().posY = y;
            griglia.GrigliaTile[x, y] = Tile.TileType.Bomba;
        }
    }
}
