using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action GameOver;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    private void Start()
    {
        GameOver += TimeFreeze;
        Player.PlayerDead += GameOver;
    }

    public void TimeFreeze() => Time.timeScale = 0;

    public void MainMenuButtonLogic()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void PlayButtonLogic()
    {
        SceneManager.LoadScene(1);
    }

}
