using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemySight))]

public class EnemyController : LivingEntity
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    public static event System.Action OnDeathStatic;

    NavMeshAgent pathfinder;
    EnemySight sight;
    Transform target;
    LivingEntity targetEntity;
    Material enemyMaterial;
    Color originalColor;

    bool hasTarget;

    void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        sight = GetComponent<EnemySight>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
        }
        else
        {
            Debug.Log("No target, or no Player");
        }
    }

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

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update()
    {
        if (hasTarget && sight.entityInSight)
        {
            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }

        /*  //////we might implement attacking differently later//////
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position);
                    StartCoroutine(Attack());
                }

            }
        }
        */
    }
    IEnumerator UpdatePath()
    {
       Debug.Log("Updating Path");
       float refreshRate = .5f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget /* (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2)*/;
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }

    }

}