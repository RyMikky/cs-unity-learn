using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    public float _z_reverce_impulse;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BigBall")
        {
            Debug.Log($"Отправляем шарик {{{other.gameObject.tag}}} назад =^_^=");

            // беру компоненту вектора скорости шара на момент попадания в триггер
            float z = other.GetComponent<Rigidbody>().velocity.z;

            // придаю обратный импульс для возврата шара назад
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, (z * -_z_reverce_impulse)), ForceMode.Impulse);
        }
    }
}