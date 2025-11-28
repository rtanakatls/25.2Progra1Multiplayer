using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(-Vector3.forward * 360 * Time.deltaTime);
    }
}
