using UnityEngine;
using System.Collections;

public class CCTVCamera : MonoBehaviour
{
    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    private bool withinRange;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();

        withinRange = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Within Camera FOV");
        
        if (other.gameObject == player)
        {
            withinRange = true;

            Vector3 playerPosition = player.transform.position - transform.position;

            //raycast toward the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerPosition, out hit))
            {
                //if the raycast hits the player, update last player sighting
                if (hit.collider.gameObject == player)
                {
                    lastPlayerSighting.position = player.transform.position;
                    Debug.Log("Camera updated player position, alarm should sound now");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Camera FOV");
    }
}