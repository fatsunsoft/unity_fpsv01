using UnityEngine;
using System.Collections;

//Switches the attached gameobject between two states
public class Switch : UsableEntity
{
    public GameObject switchObject;
    public bool startUsed;

    public static event System.Action OnUseStatic;

    private GameObject m_switchObject;
    private bool m_switched = false;

    void Awake()
    {
        if(switchObject != null)
        {
            m_switchObject = switchObject;
        }

        if (startUsed)
        {
            m_switchObject.SetActive(m_switched);
        }
        m_switched = startUsed;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeUseHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        m_switchObject.SetActive(m_switched);

        m_switched = !m_switched;

        base.TakeUseHit(hitPoint, hitDirection);
    }

}
