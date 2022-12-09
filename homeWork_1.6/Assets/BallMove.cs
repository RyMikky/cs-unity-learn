using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{

    public Vector3 _Force;                  // создаем поле для вектора силы импульса
    public Rigidbody _Body;                 // создаем поле для захвата тела

    private void Awake()
    {
        _Body = GetComponent<Rigidbody>();          // захватываем тело
        _Body.AddForce(_Force, ForceMode.Impulse);  // придаём импульс на старте
    }

}
