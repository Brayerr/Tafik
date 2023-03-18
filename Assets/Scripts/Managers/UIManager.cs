using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Player player;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Image stick;
    private Vector2 mousePositionHolder;

    private bool firstTouch = false;


    private void Start()
    {
        CloseGameOverMenu();
        GameManager.GameOver += OpenGameOverMenu;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(firstTouch) RepositionStick();

            StickLogic();
        }
        else DisableStick();
        livesText.text = ($"LIVES - {player.HP}");
    }


    public void OpenGameOverMenu() => MainMenuButton.gameObject.SetActive(true);
    public void CloseGameOverMenu() => MainMenuButton.gameObject.SetActive(false);

    public void StickLogic()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            EnableStick();
        }
        else DisableStick();
    }

    public void RepositionStick()
    {
#if UNITY_EDITOR
        stick.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y -60);
#endif

#if !UNITY_EDITOR
        stick.transform.position = Input.GetTouch(0).position;
#endif
        firstTouch = false;
    }

    public void EnableStick()
    {
        stick.gameObject.SetActive(true);
    }

    public void DisableStick()
    {
        firstTouch = true;
        stick.gameObject.SetActive(false);
    }


}
