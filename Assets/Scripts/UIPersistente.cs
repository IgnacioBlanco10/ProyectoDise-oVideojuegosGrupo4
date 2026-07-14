using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPersistente : MonoBehaviour
{
    private void awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
