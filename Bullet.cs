using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Bullet : MonoBehaviour, IPoolable {
    public int damage;
    public float speed;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Enemy"))
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);

        rb.velocity = new Vector3(0, 0, 0);

        ObjectPool.Instance.ReturnItemToInactivePool(gameObject);
    }
}
