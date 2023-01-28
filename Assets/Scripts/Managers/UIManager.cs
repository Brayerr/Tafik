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


    private void Start()
    {
        CloseGameOverMenu();
        GameManager.GameOver += OpenGameOverMenu;
    }

    void Update()
    {
        livesText.text = ($"LIVES - {player.HP}");
    }


    public void OpenGameOverMenu() => MainMenuButton.gameObject.SetActive(true);
    public void CloseGameOverMenu() => MainMenuButton.gameObject.SetActive(false);

    

}
