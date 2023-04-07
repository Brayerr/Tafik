using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static List<Pickup> pickupList = new List<Pickup>();
    [SerializeField] List<Pickup> serilizedList = new List<Pickup>();

    private void Start()
    {
        UpdateStaticList();
    }

    void UpdateStaticList()
    {
        pickupList.AddRange(serilizedList);
    }
}
