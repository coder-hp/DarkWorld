using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public static CameraTrack s_instance = null;

    public Transform target = null;

    Vector3 pianyi_pos;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {

    }

    public void setTarget(Transform _target)
    {
        target = _target;
        pianyi_pos = _target.position - transform.position;
    }

    void Update()
    {
        if(target)
        {
            transform.position = target.position - pianyi_pos;

            //Vector3 newPos = target.position - pianyi_pos;
            //transform.position = new Vector3(transform.position.x, newPos.y, newPos.z);
        }
    }
}
