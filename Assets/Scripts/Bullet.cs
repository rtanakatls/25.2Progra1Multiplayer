using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Vector3 direction;
    private Rigidbody rb;

    [SerializeField] private float speed;

    private ulong ownerId;

    public void Init(Vector3 direction, ulong ownerId)
    {
        this.direction= direction.normalized;
        this.ownerId = ownerId;
    }

    private void Start()
    {
        if (!HasAuthority)
        {
            return;
        }


        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (!HasAuthority)
        {
            return;
        }

        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasAuthority)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Player>().OwnerClientId != ownerId)
        {
            NetworkObject.Despawn();
        }

    }
}
