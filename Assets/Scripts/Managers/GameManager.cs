using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action GameOver;
    public static event Action OnOpenedPauseMenu;
    public static event Action OnClosedPauseMenu;
    public static event Action OnFakeRestart;
    public static GameManager instance;
    [SerializeField] UIManager UI;
    bool victoryIsListener;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    private void Start()
    {
        GameOver += TimeFreeze;
        //Player.PlayerDead += GameOver;
    }

    private void Update()
    {
        if (!victoryIsListener)
        {
            TileBoardLogic.OnConvert += Victory;
            victoryIsListener = true;
        }
    }

    public void Victory()
    {
        if (TileBoardLogic.DugPrecentage >= 70)
            UI.Victory();
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

    public void RestartButtonLogic()
    {
        //SceneManager.LoadScene(0);
        OnFakeRestart.Invoke();
        ClosePauseMenu();
    }

    public void OpenPauseMenu()
    {
        OnOpenedPauseMenu.Invoke();
        Time.timeScale = 0;

    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        OnClosedPauseMenu.Invoke();
    }
}
