using System;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public event Action OnDestroy;

    public void Destroy()
    {
        OnDestroy?.Invoke();
    }
}