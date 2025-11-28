using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEditor;

public class Player : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> pName = new NetworkVariable<FixedString32Bytes>();

    [SerializeField] private float speed;
    private Rigidbody rb;

    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material enemyMaterial;

    [SerializeField] private TextMeshPro nameText;

    [SerializeField] private GameObject bulletPrefab;

    private Vector3 shootDirection;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        nameText.text = pName.Value.ToString();
        pName.OnValueChanged += (oldName, newName) =>
        {
            nameText.text = newName.Value.ToString();
        };

    }

    public void SetName(string playerName)
    {
        if(IsOwner)
        {
            SendNameToServerRpc(playerName);
        }
    }

    [Rpc(SendTo.Server)]
    private void SendNameToServerRpc(string playerName)
    {
        pName.Value = playerName;
        SendNameToClientRpc(playerName);
    }


    [Rpc(SendTo.ClientsAndHost)]
    private void SendNameToClientRpc(string playerName)
    {
        nameText.text = playerName;
    }



    private void Start()
    {
        if(IsOwner)
        {
            meshRenderer.material= playerMaterial;
        }
        else
        {
            meshRenderer.material= enemyMaterial;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector3(h * speed, rb.linearVelocity.y, v * speed);

            if(h!=0||v!=0)
            {
                shootDirection = new Vector3(h, 0, v).normalized;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ShootRpc(shootDirection, transform.position);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void ShootRpc(Vector3 direction, Vector3 position)
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.transform.position = position;
        obj.GetComponent<Bullet>().Init(direction, OwnerClientId);
        obj.GetComponent<NetworkObject>().Spawn();
    }

}
