using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axix_Y_Rotate : MonoBehaviour
{

    public Rigidbody _rotation_body;              // ����������� ������
    public float _z_velocity = 0.1f;              // �������� ��� �������� ������ ������������ ���


    // Start is called before the first frame update
    void Start()
    {
        _rotation_body.AddRelativeTorque(0, _z_velocity, 0, ForceMode.VelocityChange);
    }
}
