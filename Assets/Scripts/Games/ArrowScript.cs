using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Soldier":
                {
                    other.GetComponent<EnemyScript>().hit(0);
                    break;
                }
        }
    }
}
