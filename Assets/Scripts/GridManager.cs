using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Impostazioni Mappa")]
    public GameObject Tile;

    [SerializeField] private int quantita_ostacoli_minima;
    [SerializeField] private int quantita_ostacoli_massima;

    private const int colonne = 21;
    private const int righe = 13;

    public Tile[,] GrigliaTile = new Tile[colonne, righe];
    public Vector3[,] GrigliaPosizioni = new Vector3[colonne, righe];

    [Header("Impostazioni Giocatore")]
    [SerializeField] private GameObject Giocatore;
    [Tooltip("La posizione della matrice in cui il giocatore spawnerà")] public Vector2Int PosizioneSpawn;

    ////////////////////////////////////////
    public static GridManager instance;

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    ////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        CreazioneMappa();

        SpawnGiocatore();
    }

    void SpawnGiocatore()
    {
        GameObject player = Instantiate(Giocatore, new Vector2(PosizioneSpawn.x, - PosizioneSpawn.y), Quaternion.identity);
        
        player.GetComponent<Player>().griglia = this;

        GrigliaTile[PosizioneSpawn.y, PosizioneSpawn.x].ChangeTile(global::Tile.TileType.Player);

        Debug.Log(GrigliaTile[PosizioneSpawn.y, PosizioneSpawn.x]);
    }

    void CreazioneMappa()
    {
        for (int i = 0; i < righe; i++)
        {
            int[] posEstratte = CreazionePosOstacoli();
            
            int indice = 0;

            GameObject riga = new GameObject();
            riga.name = "Riga " + i;
            riga.transform.SetParent(transform);

            for (int j = 0; j < colonne; j++)
            {
                if (i == 0 || j == 0 || i == righe - 1 || j == colonne - 1
                    || j % 2 == 0 && i % 2 == 0)
                {
                    SpawnTile(i, j, global::Tile.TileType.Muro_Indistruttibile, riga);
                }
                else
                {
                    //Se la posizione incontrata è uguale a una di quelle estratte allora spawniamo l'ostacolo.
                    if (posEstratte[indice] == j &&
                        i + j > 3 && //Eccetto l'angolo in alto a sinistra
                        j - i < (colonne - 1) - 3 && //Eccetto l'angolo in alto a destra
                        i - j < (righe - 1) - 3 && //Eccetto l'angolo in basso a sinistra
                        i + j < (colonne - 1) + (righe - 1) - 3) //Eccetto l'angolo in basso a destra
                    {
                        SpawnTile(i, j, global::Tile.TileType.Muro_Distruttibile, riga);
                    }
                    else
                    {
                        SpawnTile(i, j, global::Tile.TileType.Pavimento, riga);
                    }
                }

                if (posEstratte[indice] == j && indice < posEstratte.Length - 1)
                    indice++;
            }
        }
    }

    void SpawnTile(int i, int j, Tile.TileType tileType, GameObject riga)
    {
        GameObject tile = Instantiate(Tile, new Vector2(j, -i), Quaternion.identity);

        tile.GetComponent<Tile>().ChangeTile(tileType);

        GrigliaTile[j, i] = tile.GetComponent<Tile>();

        tile.name = i + " : " + j;

        tile.transform.SetParent(riga.transform);
    }

    int[] CreazionePosOstacoli()
    {
        int quantita_ostacoli = UnityEngine.Random.Range(quantita_ostacoli_minima, quantita_ostacoli_massima);

        int[] posEstratte = new int[quantita_ostacoli];

        do
        {
            int posEstratta = UnityEngine.Random.Range(1, colonne - 1);

            bool copia = false;

            foreach (int posizione in posEstratte)
            {
                if (posizione == posEstratta)
                {
                    copia = true;
                    break;
                }
            }

            if (!copia)
            {
                posEstratte[posEstratte.Length - quantita_ostacoli] = posEstratta;
                quantita_ostacoli--;
            }
        } while (quantita_ostacoli > 0);


        Array.Sort(posEstratte);

        return posEstratte;
    }

    //Funzione che verrà richiamata per cambiare il tipo di una tile.
    public void ChangeTile(int x, int y, Tile.TileType tileType)
    {
        GrigliaTile[x, y].ChangeTile(tileType);
    }
}
