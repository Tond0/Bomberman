using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
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

    public Tile.TileType[,] GrigliaTile = new Tile.TileType[colonne, righe];
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

        StampaMappa();

        SpawnGiocatore();
    }


    void StampaMappa()
    {
        for(int i = 0; i < righe; i++)
        {
            for(int j = 0; j < colonne; j++)
            {
                GameObject tile = Instantiate(Tile, new Vector2(j, -i), Quaternion.identity);
                tile.GetComponent<Tile>().tileType = GrigliaTile[j, i];
            }
        }
    }

    void SpawnGiocatore()
    {
        GameObject player = Instantiate(Giocatore, new Vector2(PosizioneSpawn.x, - PosizioneSpawn.y), Quaternion.identity);
        
        player.GetComponent<Player>().griglia = this;

        GrigliaTile[PosizioneSpawn.x, PosizioneSpawn.y] = global::Tile.TileType.Player;
    }

    void CreazioneMappa()
    {
        for (int i = 0; i < righe; i++)
        {
            int[] posEstratte = CreazionePosOstacoli();
            
            int indice = 0;

            GameObject raccoglitore = new GameObject();
            raccoglitore.name = "Riga " + i;
            raccoglitore.transform.SetParent(transform);

            for (int j = 0; j < colonne; j++)
            {
                if (i == 0 || j == 0 || i == righe - 1 || j == colonne - 1
                    || j % 2 == 0 && i % 2 == 0)
                {
                    GrigliaTile[j, i] = global::Tile.TileType.Muro_Indistruttibile;
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
                        GrigliaTile[j, i] = global::Tile.TileType.Muro_Distruttibile;
                    }
                    else
                    {
                        GrigliaTile[j, i] = global::Tile.TileType.Pavimento;
                    }
                }

                if (posEstratte[indice] == j && indice < posEstratte.Length - 1)
                    indice++;
            }
        }
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

    public void ResetPavimento(int x, int y)
    {
        //GrigliaTile[x, y].SetActive(false);
    }
}
