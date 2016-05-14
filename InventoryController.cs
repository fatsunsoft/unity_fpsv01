using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour
{
    //inventory keys
    private KeyCode[] InventoryKeys =
    {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    private int activeSlot;

    void Awake()
    {
        activeSlot = 1;
    }

    public void ActivateSlot(int slotNumber)
    {
        if (activeSlot != slotNumber)
        {
            activeSlot = slotNumber;
            Debug.Log("Activated Slot " + slotNumber);
        }
    }
}