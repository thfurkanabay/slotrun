using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private float moveSpeed;
    private float destroyXPosition;

    public void Initialize(float speed, float destroyX)
    {
        moveSpeed = speed;
        destroyXPosition = destroyX;
    }

    private void Update()
    {
        // Hareket ettir
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Tüm alt nesnelerin yok edilme kontrolü
        if (AllChildrenOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    private bool AllChildrenOutOfBounds()
    {
        // Tüm alt nesneleri kontrol et
        foreach (Transform child in transform)
        {
            if (child.position.x > destroyXPosition)
            {
                return false; // Eğer bir alt nesne sınır içinde ise, yok etme
            }
        }
        return true; // Tüm alt nesneler sınır dışına çıktıysa, yok et
    }
}