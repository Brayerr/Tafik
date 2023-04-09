using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    

    //[SerializeField] private Button MainMenuButton;
    [SerializeField] private TextMeshProUGUI digPrecentageText;
    [SerializeField] private TextMeshProUGUI VictoryText;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Button abilityButtonFill;
    [SerializeField] private Canvas pauseMenu;

    [SerializeField] Image[] hearts = new Image[5];
    [SerializeField] int currentHearts;

    [SerializeField] private float abilityFill = 0;

    bool updateScoreIsListener = false;


    private void Start()
    {
        //CloseGameOverMenu();
        //GameManager.GameOver += OpenGameOverMenu;
        digPrecentageText.text = "";
        PlayerLogic.OnAbilityFillUpdated += UpdateAbilityFill;
        GameManager.OnOpenedPauseMenu += ActivatePauseMenu;
        GameManager.OnClosedPauseMenu += DeactivatePauseMenu;
        PlayerLogic.OnHPChanged += UpdateHeartsAmount;
        currentHearts = 3;

    }

    void Update()
    {
        if (!updateScoreIsListener)
        {
            TileBoardLogic.OnConvert += UpdateScore;
            updateScoreIsListener = true;
        }

    }

    public void UpdateScore()
    {
        digPrecentageText.text = $"{TileBoardLogic.DugPrecentage}%";
    }

    public void Victory()
    {
        VictoryText.gameObject.SetActive(true);
    }


    //public void OpenGameOverMenu() => MainMenuButton.gameObject.SetActive(true);
    //public void CloseGameOverMenu() => MainMenuButton.gameObject.SetActive(false);

    void UpdateAbilityFill(float newAbilityFill)
    {
        abilityFill = newAbilityFill;
        abilityButtonFill.image.fillAmount = abilityFill / 200;
    }

    void UpdateHeartsAmount(bool value)
    {
        if (value) currentHearts++;
        else currentHearts--;

        switch(currentHearts)
        {
            case 0:
                {
                    for (int i = 0; i < hearts.Length; i++)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case 1:
                {
                    hearts[0].gameObject.SetActive(true);
                    for (int i = 1; i < hearts.Length; i++)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case 2:
                {
                    hearts[0].gameObject.SetActive(true);
                    hearts[1].gameObject.SetActive(true);
                    for (int i = 2; i < hearts.Length; i++)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case 3:
                {
                    hearts[0].gameObject.SetActive(true);
                    hearts[1].gameObject.SetActive(true);
                    hearts[2].gameObject.SetActive(true);
                    for (int i = 3; i < hearts.Length; i++)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case 4:
                {
                    hearts[0].gameObject.SetActive(true);
                    hearts[1].gameObject.SetActive(true);
                    hearts[2].gameObject.SetActive(true);
                    hearts[3].gameObject.SetActive(true);
                    for (int i = 4; i < hearts.Length; i++)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case 5:
                {
                    hearts[0].gameObject.SetActive(true);
                    hearts[1].gameObject.SetActive(true);
                    hearts[2].gameObject.SetActive(true);
                    hearts[3].gameObject.SetActive(true);
                    hearts[4].gameObject.SetActive(true);                   
                    break;
                }

        }
    }
    

    public void ActivatePauseMenu() => pauseMenu.gameObject.SetActive(true);
    public void DeactivatePauseMenu() => pauseMenu.gameObject.SetActive(false);


}