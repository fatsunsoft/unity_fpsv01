using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    //where we hold the weapon, array of available weapons
    public Transform weaponHold;
    public Light m_flashlight;
    public Gun[] Guns;

    //reference for currently equipped gun
    Gun equippedGun;

    //player flashlight
    private Light flashlight;
    private bool toggleFlashlight;

    void Start()
    {
        //equip all the weapons in the Guns array and set the first one active
        for (int i = 0; i < Guns.Length; i++)
        {
            CreateGun(i);
        }
        equippedGun.gameObject.SetActive(true);

        flashlight = m_flashlight;
        ToggleFlashlight();
    }


    //create gun with the given index
    public void CreateGun(int weaponIndex)
    {
        CreateGun(Guns[weaponIndex]);
    }
    //instantiate and place the gun
    public void CreateGun(Gun gunToCreate)
    {
        //instantiate the gun at the weapon hold
        equippedGun = Instantiate(gunToCreate, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
        equippedGun.gameObject.SetActive(false);
    }

    //select weapons
    public void SelectGun(int weaponIndex)
    {

    }

    //Trigger - Holding
    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    //Trigger - Releasing
    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    //Reloading
    public void Reload()
    {
        if(equippedGun != null)
        {
            equippedGun.Reload();
        }
    }

    //Flashlight Toggle
    public void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.enabled = toggleFlashlight;
            toggleFlashlight = !toggleFlashlight;
        }
    }
}