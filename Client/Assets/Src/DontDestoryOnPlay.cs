using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnPlay : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}