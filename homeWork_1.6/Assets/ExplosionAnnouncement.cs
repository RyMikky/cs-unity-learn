using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionAnnouncement : MonoBehaviour
{
    public List<Rigidbody> _rigidbodies;     // массив для тел которые надо будет запульнуть
    public int _random_lhs;                  // нижняя граница рандомайзера
    public int _random_rhs;                  // верхняя граница рандомайзера
    public float _forces_multiply;           // мультипликатор силы

    public bool _x_comp;                     // компонента ветора силы
    public bool _y_comp;                     // компонента ветора силы
    public bool _z_comp;                     // компонента ветора силы
    
    public List<Vector3> _forces;           // закрытое поле заполняется в случае наличия тел в массиве

    private void Start()
    {
        // на старте если есть тела на запуск в полет, то заполняем лист векторов импульсов силы
        if (_rigidbodies.Count > 0)
        {
            for (int i = 0; i != _rigidbodies.Count; ++i)
            {
                _forces.Add(GetRandomForceVector(_x_comp, _y_comp, _z_comp));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // если на триггер приезжает BigBall
        if (other.gameObject.tag == "BigBall")
        {
            // если есть тела для отправления в полет и есть список векторов сил
            if(_rigidbodies.Count > 0 && _forces.Count > 0)
            {
                Debug.Log("Скдыщ =^_^=");  // выводим сообщение
                System.Random rnd = new System.Random();
                
                // добавляем большому шару случайным импульс
                other.GetComponent<Rigidbody>().AddForce(
                    new Vector3(
                        rnd.Next(_random_lhs, _random_rhs) / other.GetComponent<Rigidbody>().mass, 
                        rnd.Next(0, _random_rhs) / (other.GetComponent<Rigidbody>().mass / 2), 
                        rnd.Next(_random_lhs, _random_rhs)) / other.GetComponent<Rigidbody>().mass, 
                    ForceMode.Impulse);

                // разбрасываем прочие шарики
                BallExplosive();
            }
        }
    }

    private Vector3 GetRandomForceVector(bool _x_comp, bool _y_comp, bool _z_comp)
    {
        float x = 0f; float y = 0f; float z = 0f;
        System.Random rnd = new System.Random();

        if (_x_comp)
        {
            x = (float) rnd.Next(_random_lhs, _random_rhs) * _forces_multiply;
        }

        if (_y_comp)
        {
            y = (float) rnd.Next(0, _random_rhs) * _forces_multiply;
        }

        if (_z_comp)
        {
            z = (float) rnd.Next(_random_lhs, _random_rhs) * _forces_multiply;
        }

        return new Vector3(x, y, z);
    }

    private void BallExplosive()
    {
        for (int i = 0; i != _rigidbodies.Count; ++i)
        {
            _rigidbodies[i].AddForce(_forces[i], ForceMode.Impulse);
        }
    }
}