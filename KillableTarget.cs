using UnityEngine;
using System.Collections;

public class KillableTarget : LivingEntity
{
    public static event System.Action OnDeathStatic;

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= health && !dead)
        {
            if (OnDeathStatic != null)
            {
                OnDeathStatic();
            }
            //Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }


}
