using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    ////////////////////////////////////

    public static GameManager instance;

    ////////////////////////////////////

    public static Action OnGameWon;
    public static Action OnGameLost;

    void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        OnGameWon += NextLevel;
        OnGameLost += GameOver;
    }

    void OnDisable()
    {
        OnGameWon -= NextLevel;
        OnGameLost -= GameOver;   
    }

    public void CheckIfWon()
    {
        //Ricordiamoci che un'entity rimarrà sempre (il player)
        if (FindObjectsOfType<Entity>().Length <= 2)
            GameWon();
    }

    void GameWon()
    {
        Debug.Log("dada");
        OnGameWon.Invoke();
    }

    public void GameLost()
    {
        OnGameLost.Invoke();
    }

    //Facciamo finta che dovrebbero succede cose diverse a seconda di se vince o meno
    //(Per colpa del lavoro non ho avuto tempo)
    void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
