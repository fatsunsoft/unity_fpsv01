using UnityEngine;
using System.Collections;

public class LaserDetection : MonoBehaviour
{
    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            lastPlayerSighting.position = other.transform.position;
        }
    }
}