using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Impostazioni Mappa")]
    public GameObject Pavimento;
    public GameObject Muro_Indistruttibile;
    public GameObject Ostacolo;

    [SerializeField] private int quantita_ostacoli_minima;
    [SerializeField] private int quantita_ostacoli_massima;

    private const int colonne = 21;
    private const int righe = 13;

    public GameObject[,] GrigliaOggetti = new GameObject[colonne, righe];
    public Vector3[,] GrigliaPosizioni = new Vector3[colonne, righe];

    [Header("Impostazioni Giocatore")]
    [SerializeField] private GameObject Giocatore;
    [Tooltip("La posizione della matrice in cui il giocatore spawner�")] public Vector2 PosizioneSpawn;

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

        StampaMatrice();

        SpawnGiocatore();
    }

    void SpawnGiocatore()
    {
        GameObject player = Instantiate(Giocatore, GrigliaPosizioni[(int)PosizioneSpawn.x, (int)PosizioneSpawn.y], Quaternion.identity);
        player.GetComponent<Player>().griglia = this;

        GrigliaOggetti[(int)PosizioneSpawn.x, (int)PosizioneSpawn.y] = player;
    }

    //Gira la matrice scorrendo prima le righe
    void StampaMatrice()
    {
        for(int i = 0; i < righe; i++)
        {
            GameObject raccoglitore = new GameObject();
            raccoglitore.name = "Riga " + i;
            raccoglitore.transform.SetParent(transform);

            for(int j = 0; j < colonne; j++)
            {
                GameObject tassello = Instantiate(GrigliaOggetti[j,i], new Vector2(j, -i), Quaternion.identity);
                tassello.transform.SetParent(raccoglitore.transform);
                tassello.name = i + " : " + j;

                GrigliaPosizioni[j, i] = new Vector2(j, -i);
            }
        }
    }

    void CreazioneMappa()
    {
        for (int i = 0; i < righe; i++)
        {
            int[] posEstratte = CreazionePosOstacoli();
            
            int indice = 0;

            for (int j = 0; j < colonne; j++)
            {
                if (i == 0 || j == 0 || i == righe - 1 || j == colonne - 1
                    || j % 2 == 0 && i % 2 == 0)
                {
                    GrigliaOggetti[j, i] = Muro_Indistruttibile;
                }
                else
                {
                    //Se la posizione incontrata � uguale a una di quelle estratte allora spawniamo l'ostacolo.
                    if (posEstratte[indice] == j &&
                        i + j > 3 && //Eccetto l'angolo in alto a sinistra
                        j - i < (colonne - 1) - 3 && //Eccetto l'angolo in alto a destra
                        i - j < (righe - 1) - 3 && //Eccetto l'angolo in basso a sinistra
                        i + j < (colonne - 1) + (righe - 1) - 3) //Eccetto l'angolo in basso a destra
                    {
                        GrigliaOggetti[j, i] = Ostacolo;
                    }
                    else
                    {
                        GrigliaOggetti[j, i] = Pavimento;
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

    public void DistruzioneMuro(int x, int y, GameObject raccoglitore)
    {
        Destroy(GrigliaOggetti[x, y]);
        GameObject tassello = Instantiate(GrigliaOggetti[x,y], GrigliaPosizioni[x,y], Quaternion.identity);
        tassello.transform.SetParent(raccoglitore.transform);
        tassello.name = x + " : " + y;

    }
}
