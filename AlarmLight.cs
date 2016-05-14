using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    public float highIntensity = 2.5f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;

    private float targetIntensity;
    private Light alarmLight;

    void Awake()
    {
        alarmLight = GetComponent<Light>();

        alarmLight.intensity = 0f;

        targetIntensity = highIntensity;
    }

    void FixedUpdate()
    {
        if (alarmOn)
        {
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);

            CheckTargetIntensity();
        }
        else
        {
            alarmLight.intensity = 0f;
        }
    }

    void CheckTargetIntensity()
    {
        if(Mathf.Abs(targetIntensity - alarmLight.intensity) < changeMargin)
        {
            if(targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }
}