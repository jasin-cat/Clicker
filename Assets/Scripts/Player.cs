using UnityEngine;

public class Player : PooledObject
{
    [SerializeField]
    private float _speed;
    private Transform _transform;
    private Collision _collision;
    private void Awake()
    {
        _collision = new Collision();
        _collision.AddCollisionObject(this.gameObject);
    }
    private void OnEnable()
    {
        _transform = this.transform;
        _collision.EnableCollisionObject(this.gameObject);
    }

    private void OnDisable()
    {
        _collision.DisableCollisionObject(this.gameObject);
    }

    private void Update()
    {
        if(_collision.IsHit.Value) return;

        _collision.Check();

        _transform.position = _transform.position + Vector3.right * _speed * Time.deltaTime;
    }
}
