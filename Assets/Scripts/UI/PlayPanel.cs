using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PlayPanel : MonoBehaviour
{
    [SerializeField] Image[] islands = new Image[2];
    int currentIslandIndex;

    [SerializeField] Button playButton;


    private void Start()
    {
        currentIslandIndex = 0;
    }

    private void Update()
    {
        if (currentIslandIndex > 0) playButton.interactable = false;
        else playButton.interactable = true;
    }

    public void NextIsland()
    {
        if (currentIslandIndex + 1 < islands.Length)
        {
            islands[currentIslandIndex].gameObject.SetActive(false);
            currentIslandIndex++;
            islands[currentIslandIndex].gameObject.transform.localScale = Vector3.zero;
            islands[currentIslandIndex].gameObject.SetActive(true);
            islands[currentIslandIndex].gameObject.transform.DOScale(4.5f, .2f);

        }
    }

    public void PreviousIsland()
    {
        if(currentIslandIndex - 1 > -1)
        {
            islands[currentIslandIndex].gameObject.SetActive(false);
            currentIslandIndex--;
            islands[currentIslandIndex].gameObject.transform.localScale = Vector3.zero;
            islands[currentIslandIndex].gameObject.SetActive(true);
            islands[currentIslandIndex].gameObject.transform.DOScale(4.5f, .2f);
        }    
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(currentIslandIndex + 1);
    }

}
