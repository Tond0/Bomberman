using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{

    private readonly Transform transformToMove;
    private readonly Entity entity;

    public Movement(Transform transformToMove, Entity entity)
    {
        this.transformToMove = transformToMove;
        this.entity = entity;
    }

    public void MoveEntity()
    {

        if (entity.moveInput.movimento != Vector2.zero //Se si vuole muovere
            && entity.moveInput.movimento.x * entity.moveInput.movimento.y == 0 //Non in diagonale
                && !entity.moving //Non si sta già muovendo
                    && CanMoveThere(entity.moveInput.movimento)) //Può muoversi in quella direzione
        {
            //Variabile d'appoggio così che non dipenda da una variabile in continuo cambiamento a causa dell'Update.
            Vector2Int direzione = entity.moveInput.movimento;

            //L'entità si sta muovendo
            entity.moving = true;

            //Inizio animazione
            transformToMove.DOMove(new Vector2(transformToMove.position.x + direzione.x, transformToMove.position.y + direzione.y), entity.speed / 60).OnComplete(() => SetNewPosition(direzione));
        }
    }

    bool CanMoveThere(Vector2Int direzione)
    {
        var OggettoNextPosition = GridManager.instance.GrigliaTile[entity.posAttuale.x + direzione.x, entity.posAttuale.y - direzione.y].tileType;

        if (OggettoNextPosition == Tile.TileType.Pavimento)
            return true;
        else
        {
            entity.moving = false;
            return false;
        }
    }

    void SetNewPosition(Vector2Int direzione)
    {
        GridManager.instance.GrigliaTile[entity.posAttuale.x + direzione.x, entity.posAttuale.y - direzione.y].ChangeTile(Tile.TileType.Player);


        if (GridManager.instance.GrigliaTile[entity.posAttuale.x, entity.posAttuale.y].tileType != Tile.TileType.PlayerSuBomba)
        {
            GridManager.instance.GrigliaTile[entity.posAttuale.x, entity.posAttuale.y].ChangeTile(Tile.TileType.Pavimento);
        }
        else
        {
            GridManager.instance.GrigliaTile[entity.posAttuale.x, entity.posAttuale.y].ChangeTile(Tile.TileType.Bomba);
        }


        entity.posAttuale.x += direzione.x;
        entity.posAttuale.y -= direzione.y;

        entity.moving = false;
    }

}
