using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
}