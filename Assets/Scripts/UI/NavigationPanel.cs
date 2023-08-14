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

    Vector3 rightPos;
    Vector3 leftPos;
    Vector3 centerPos;

    private void Start()
    {
        currentOpenPanel = playPanel;
        centerPos = playPanel.transform.position;
        rightPos = centerPos;
        rightPos.x += 100;
        leftPos = centerPos;
        leftPos.x -= 100;
    }

    public void OpenPanel(Image panel)
    {
        if (panel.transform.localScale.z > currentOpenPanel.transform.localScale.z)
        {
            panel.rectTransform.position = rightPos;
            currentOpenPanel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
            panel.rectTransform.DOMove(centerPos, .3f);
            currentOpenPanel = panel;
        }

        else if (panel.transform.localScale.z < currentOpenPanel.transform.localScale.z)
        {
            panel.transform.position = leftPos;
            currentOpenPanel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
            panel.rectTransform.DOMove(centerPos, .3f);
            currentOpenPanel = panel;
        }
    }
}
