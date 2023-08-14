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
        UIManager.MenuOpened += TimeFreeze;
        UIManager.MenuClosed += TimeUnfreeze;
        //Player.PlayerDead += GameOver;
    }

    private void OnDisable()
    {
        UIManager.MenuOpened -= TimeFreeze;
        UIManager.MenuClosed -= TimeUnfreeze;
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
    public void TimeUnfreeze() => Time.timeScale = 1;

    public void MainMenuButtonLogic()
    {
        TimeUnfreeze();
        SceneManager.LoadScene(0);
    }

    public void PlayButtonLogic()
    {
        TimeUnfreeze();
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
        //TimeFreeze();

    }

    public void ClosePauseMenu()
    {
        //TimeUnfreeze();
        OnClosedPauseMenu.Invoke();
    }

    private void OnDestroy()
    {
        GameOver = null;
        OnOpenedPauseMenu = null;
        OnClosedPauseMenu = null;
        OnFakeRestart = null;
        EnemyManager.ClearList();
    }
}
