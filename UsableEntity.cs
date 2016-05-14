using UnityEngine;
using System.Collections;

//base class for usable, but non damagable objects
public class UsableEntity : MonoBehaviour, IUsable
{
    private bool used;
    private bool reUsable;

    //variable event which can be broadcast as OnUse
    public event System.Action OnUse;

    protected virtual void Start()
    {
    }

    public virtual void TakeUseHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        Debug.Log("Took use hit at " + hitPoint + "point, from " + hitDirection);

        ChangeState();
    }

    public virtual void ChangeState()
    {
        used = !used;

        if (OnUse != null)
        {
            OnUse();
        }
    }
}
