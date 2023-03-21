using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Impostazioni mappa")]
    [SerializeField] private GameObject Pavimento;
    [SerializeField] private GameObject Muro_Indistruttibile;
    [SerializeField] private GameObject Ostacolo;

    [SerializeField] private int quantita_ostacoli_minima;
    [SerializeField] private int quantita_ostacoli_massima;

    private const int righe = 21;
    private const int colonne = 13;

    GameObject[,] Griglia = new GameObject[righe, colonne];
   
    // Start is called before the first frame update
    void Start()
    {
        CreazioneMappa();

        StampaMatrice();
    }

    //Gira la matrice scorrendo prima le righe
    void StampaMatrice()
    {
        for(int i = 0; i < colonne; i++)
        {
            GameObject raccoglitore = new GameObject();
            raccoglitore.name = "Riga " + i;
            raccoglitore.transform.SetParent(transform);

            for(int j = 0; j < righe; j++)
            {
                GameObject tassello = Instantiate(Griglia[j,i], new Vector2(j, -i), Quaternion.identity);
                tassello.transform.SetParent(raccoglitore.transform);
                tassello.name = i + " : " + j;
            }
        }
    }

    void CreazioneMappa()
    {
        for (int i = 0; i < colonne; i++)
        {

            int[] posEstratte = CreazionePosOstacoli();

            int indice = 0;

            for (int p = 0; p < posEstratte.Length; p++)
                Debug.Log(posEstratte[p]);

            Debug.Log("             ");

            for (int j = 0; j < righe; j++)
            {
                if (i == 0 || j == 0 || i == colonne - 1 || j == righe - 1
                    || j % 2 == 0 && i % 2 == 0)
                {
                    Griglia[j, i] = Muro_Indistruttibile;
                }
                else
                {
                    if (indice < posEstratte.Length && posEstratte[indice] == j)
                    {
                        Debug.Log("entrato");
                        Griglia[j, i] = Ostacolo;
                        indice++;
                    }
                    else
                    {
                        Griglia[j, i] = Pavimento;
                    }
                }
            }
        }
    }

    int[] CreazionePosOstacoli()
    {
        int quantita_ostacoli = UnityEngine.Random.Range(quantita_ostacoli_minima, quantita_ostacoli_massima);

        int[] posEstratte = new int[quantita_ostacoli];

        do
        {
            int posEstratta = UnityEngine.Random.Range(1, righe - 1);

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
                Debug.Log("ESTRATTO IL NUMERO: " + posEstratta);
                posEstratte[posEstratte.Length - quantita_ostacoli] = posEstratta;
                quantita_ostacoli--;
            }

                Debug.Log("QUANTITA OSTACOLI RIMANENTI: " + quantita_ostacoli);
        } while (quantita_ostacoli > 0);


        Array.Sort(posEstratte);

        return posEstratte;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
