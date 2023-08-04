using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NavigationPanel : MonoBehaviour
{
    [SerializeField] Image settingsPanel;
    [SerializeField] Image creditsPanel;
    [SerializeField] Image playPanel;
    [SerializeField] Image shopPanel;
    [SerializeField] Image collectionPanel;

    Image currentOpenPanel;


    private void Start()
    {
        currentOpenPanel = playPanel;
    }

    public void OpenPanel(Image panel)
    {
        currentOpenPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        currentOpenPanel = panel;
    }
}
