using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLayer : MonoBehaviour
{
    public static MatchLayer s_instance = null;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        
    }

    public void close()
    {
        Destroy(gameObject);
    }
}
