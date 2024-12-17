using UnityEngine;

public class Slot : MonoBehaviour
{
    public float rotationSpeed = 45f;

    void Update()
    {
        // Z ekseninde döndürme işlemi
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}