using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public Text ammoCount;
    public Text playerHealth;

    private Text currentAmmo;
    private Text currentHealth;

    void Awake()
    {
        ammoCount.text = ("12 / 12");
        playerHealth.text = ("100");
    }

}
