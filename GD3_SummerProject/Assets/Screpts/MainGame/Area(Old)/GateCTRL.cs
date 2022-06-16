using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCTRL : MonoBehaviour
{
    [SerializeField]bool doClose;

    BoxCollider2D coll;
    MeshRenderer mr;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        coll.enabled = doClose;
        mr.enabled = doClose;
    }

    public void GateClose()
    {
        doClose = true;
    }

    public void GateOpen()
    {
        doClose= false;
    }
}
