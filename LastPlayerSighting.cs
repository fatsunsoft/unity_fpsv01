using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour
{
    public Vector3 position = new Vector3(0f, -10f, 0f);
    public Vector3 resetPosition = new Vector3(0f, -10f, 0f);

    private AlarmLight alarm;

    private float onDuration = 5f;
    private float timer = 0f;

    void Awake()
    {
        //refernece alarm lights
        alarm = GameObject.FindGameObjectWithTag("Alarm").GetComponent<AlarmLight>();
    }

    void FixedUpdate()
    {
        SwitchAlarms();

        if (alarm.alarmOn)
        {
            timer += Time.deltaTime;

            if(timer >= onDuration)
            {
                position = resetPosition;

                timer = 0f;
            }
        }
    }

    void SwitchAlarms()
    {
        //if last global sighting is not default position, alarm on
        alarm.alarmOn = position != resetPosition;
    }
}
