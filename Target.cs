using UnityEngine;
using System.Collections;

public class Target : UsableEntity
{
    public bool hasLight;

    public static event System.Action OnUseStatic;

    private Light thisLight;

    void Awake()
    {
        if (hasLight)
        {
            thisLight = GetComponentInChildren<Light>();
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeUseHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        if (hasLight)
        {
            thisLight.enabled = !thisLight.enabled;
        }
        base.TakeUseHit(hitPoint, hitDirection);

        Destroy(gameObject);
    }

}
