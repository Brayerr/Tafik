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
    public static event Action<float> OnAbilityFillUpdated;

    //[SerializeField] private Button MainMenuButton;
    [SerializeField] private TextMeshProUGUI digPrecentageText;
    [SerializeField] private TextMeshProUGUI VictoryText;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Button abilityButtonFill;
    [SerializeField] private Canvas pauseMenu;



    [SerializeField] private float abilityFill = 0;
    [SerializeField] private float maxAbilityFill = 200;
    bool updateScoreIsListener = false;


    private void Start()
    {
        //CloseGameOverMenu();
        //GameManager.GameOver += OpenGameOverMenu;
        digPrecentageText.text = "";
        TileBoardLogic.OnConverted += UpdateAbilityFill;
        GrappleHook2.onActivatedAbility += ResetAbilityFill;
        GameManager.OnOpenedPauseMenu += ActivatePauseMenu;
        GameManager.OnClosedPauseMenu += DeactivatePauseMenu;
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


    //public void SetButtonImage()
    //{
    //    if (abilityFill <= 100) abilityButton.image.sprite = ability1;
    //    else if (abilityFill > 100 && abilityFill < 200) abilityButton.image.sprite = ability2;
    //    else if (abilityFill == 200) abilityButton.image.sprite = ability3;
    //}

    public void UpdateAbilityFill(int tilesDug)
    {
        abilityFill += tilesDug;
        if (abilityFill >= maxAbilityFill) abilityFill = maxAbilityFill;
        abilityButtonFill.image.fillAmount = abilityFill / 200;
        OnAbilityFillUpdated.Invoke(abilityFill);
        //SetButtonImage();
    }

    public void ResetAbilityFill()
    {
        abilityFill = 0;
        //SetButtonImage();
    }

    public void ActivatePauseMenu() => pauseMenu.gameObject.SetActive(true);
    public void DeactivatePauseMenu() => pauseMenu.gameObject.SetActive(false);

}