using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DropPoint : MonoBehaviour
{
    [ShowInInspector]
    public LightController currentLight;
    [ShowInInspector]
    public Collider2D dropCollider;
    [SerializeField]
    DropPoint[] connectedPoints;
    [ShowInInspector]
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        SyncDPInit();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = connectedPoints.Length + 1;
        lr.SetPosition(0, transform.position);
        for(int i = 0; i < connectedPoints.Length; i++)
        {
            lr.SetPosition(i+1, connectedPoints[i].transform.position);
        }
        
        if(dropCollider == null)
        {
            Debug.Log("ERROR: No drop collider set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        dropCollider.enabled = (currentLight == null);
    }

    public void SyncDP()
    {
        foreach (DropPoint dp in connectedPoints)
        {
            dp.TurnOnOrOff(currentLight != null && currentLight.isOn);
        }
    }

    public void SyncDPInit()
    {
        bool shouldBeOn = (currentLight != null && currentLight.isOn);
        foreach (DropPoint dp in connectedPoints)
        {
            shouldBeOn &= (dp.currentLight != null && dp.currentLight.isOn);
        }
        TurnOnOrOff(shouldBeOn);
    }

    public void TurnOnOrOff(bool shouldBeOn)
    {
        if (currentLight != null)
        {
            currentLight.isOn = shouldBeOn;
        }
    }
}
