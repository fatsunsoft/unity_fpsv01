using UnityEngine;
using System.Collections;

//base class for Player and enemies
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    //variable event which can be broadcast as OnDeath
    public event System.Action OnDeath;

    //assign some health
    protected virtual void Start()
    {
        health = startingHealth;
    }

    //take hit, give damage to damage function
    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    //damage on hit
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        //if health 0 or less, die
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    //destroy game object when it dies
    protected void Die()
    {
        dead = true;

        //invoke event OnDeath
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}