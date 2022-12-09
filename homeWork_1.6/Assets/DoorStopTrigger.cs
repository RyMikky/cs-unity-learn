using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            // беру компоненту вектора скорости двери на момент попадания в триггер
            float x = other.GetComponent<Rigidbody>().velocity.x;

            // придаю обратный импульс для постепенной остановки двери
            other.GetComponent<Rigidbody>().AddForce(new Vector3(-(x + (x * 0.9f)), 0, 0), ForceMode.Impulse);
        }
    }
}