using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [Header("Statistiche")]
    [Tooltip("La velocità tra una casella è l'altra")] public float speed;
    public enum EntityType { Giocatore, Nemico }
    public EntityType tipoEntita;

    public Vector2Int posAttuale;

    public Movement entityMovement;

    public IMovement moveInput;
    public IPlaceBomb bombInput;

    [NonSerialized] public bool moving;

    [Header("Impostazioni bombe")]
    [SerializeField] private GameObject bombaPrefab;
    private GameObject referenceBomba;

    private void Awake()
    {
        //Inputs
        moveInput = tipoEntita == EntityType.Giocatore ? new PlayerInput() : new EnemyInput() as IMovement;
        bombInput = tipoEntita == EntityType.Giocatore ? new PlayerInput() : null;

        //Movimento
        entityMovement = new Movement(transform, this);

        moveInput.ReadInputMove();
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////

        //Se è il giocatore prende input ogni frame
        if (tipoEntita == EntityType.Giocatore)
            moveInput.ReadInputMove();
        //Se è il nemico, verrà assegnata una nuova direzione casuale
        //solo quando si scontrerà.
        else if (tipoEntita == EntityType.Nemico && !entityMovement.CanMoveThere(moveInput.movimento))
            moveInput.ReadInputMove();

        ////////////////////////////////////

        //Muove l'entità
        entityMovement.MoveEntity();

        ////////////////////////////////////

        if (bombInput != null //Se può piazzare le bombe... (Avevo pensato di farlo fare anche ad i nemici but nvm)
            && bombInput.ReadInputBomb() //Se vuole mettere una bomba...
                && referenceBomba == null //Se non ha piazzato altre bombe...
                    && !moving)//Se non si sta muovendo...
            SpawnBomb();

        ////////////////////////////////////
    }

    void SpawnBomb()
    {
        referenceBomba = Instantiate(bombaPrefab, transform.position, Quaternion.Euler(0, 0, 45));
        referenceBomba.GetComponent<bomba>().posX = posAttuale.x;
        referenceBomba.GetComponent<bomba>().posY = posAttuale.y;
        GridManager.instance.GrigliaTile[posAttuale.x, posAttuale.y].ChangeTile(Tile.TileType.PlayerSuBomba);
    }
}
