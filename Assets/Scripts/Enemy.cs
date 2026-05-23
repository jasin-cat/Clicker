using UnityEngine;

public class Enemy : PooledObject
{
    private Collision _collision;
    private void Awake()
    {
        _collision = new Collision();
        _collision.AddCollisionObject(this.gameObject);
    }

    private void OnEnable()
    {
        _collision.EnableCollisionObject(this.gameObject);
    }

    private void OnDisable()
    {
        _collision.DisableCollisionObject(this.gameObject);
    }
}
