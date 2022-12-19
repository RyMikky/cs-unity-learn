using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD_Moving : MonoBehaviour
{

    // --------------------- Блок назначения колайдеров колес --------------------------------------

    public WheelCollider _left_front_f_col;        // коллайдер левого переднего колеса
    public WheelCollider _left_front_c_col;        // коллайдер левого передне-центрального колеса
    public WheelCollider _left_rear_c_col;         // коллайдер левого задне-центрального колеса
    public WheelCollider _left_rear_r_col;         // коллайдер левого заднего колеса

    public WheelCollider _right_rear_r_col;        // коллайдер правого заднего колеса
    public WheelCollider _right_rear_c_col;        // коллайдер правого задне-центрального колеса
    public WheelCollider _right_front_c_col;       // коллайдер правого передне-центрального колеса
    public WheelCollider _right_front_f_col;       // коллайдер правого переднего колеса

    // --------------------- Блок назначения трансформов колес -------------------------------------

    public Transform _left_front_f_trans;          // трансформ левого переднего колеса
    public Transform _left_front_c_trans;          // трансформ левого передне-центрального колеса
    public Transform _left_rear_c_trans;           // трансформ левого задне-центрального колеса
    public Transform _left_rear_r_trans;           // трансформ левого заднего колеса

    public Transform _right_rear_r_trans;          // трансформ правого заднего колеса
    public Transform _right_rear_c_trans;          // трансформ правого задне-центрального колеса
    public Transform _right_front_c_trans;         // трансформ правого передне-центрального колеса
    public Transform _right_front_f_trans;         // трансформ правого переднего колеса

    // --------------------- Блок назначения общих параметров движения------------------------------

    public bool _Enable = false;                   // включение скрипта

    public float _steer_Angle = 25.0f;             // угол поворота колёс
    public float _engineForce = 18000f;            // мощность двигателя
    public float _brakeForce = 12000f;             // сила торможения
    
    public Rigidbody _Panzer;                      // объект который поедет

    private float _steer;                          // значения поворота
    private float _h, _v;                          // значения со стрелок

    void FixedUpdate()
    {
        if (_Enable)
        {
            Inputs();
            Drive();
            //Stop();
            Steering();

            UpdateWheelPos(_left_front_f_col, _left_front_f_trans);
            UpdateWheelPos(_left_front_c_col, _left_front_c_trans);
            UpdateWheelPos(_left_rear_c_col, _left_rear_c_trans);
            UpdateWheelPos(_left_rear_r_col, _left_rear_r_trans);

            UpdateWheelPos(_right_front_f_col, _right_front_f_trans);
            UpdateWheelPos(_right_front_c_col, _right_front_c_trans);
            UpdateWheelPos(_right_rear_c_col, _right_rear_c_trans);
            UpdateWheelPos(_right_rear_r_col, _right_rear_r_trans);
        }
    }

    // получение данных по горизонтали и вертикали со стрелок
    void Inputs()
    {
        _h = Input.GetAxis("Horizontal"); _v = Input.GetAxis("Vertical");
    } 

    // базовая функция движения
    void Drive()
    {
        // так как у нас ведущие все колеса, то делим момент на четыре оси
        _left_front_f_col.motorTorque = (_v * _engineForce) / 4;
        _left_front_c_col.motorTorque = (_v * _engineForce) / 4;
        _left_rear_c_col.motorTorque = (_v * _engineForce) / 4;
        _left_rear_r_col.motorTorque = (_v * _engineForce) / 4;

        _right_rear_r_col.motorTorque = (_v * _engineForce) / 4;
        _right_rear_c_col.motorTorque = (_v * _engineForce) / 4;
        _right_front_c_col.motorTorque = (_v * _engineForce) / 4;
        _right_front_f_col.motorTorque = (_v * _engineForce) / 4;
    }

    //void Stop()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("Breaking");
    //        _left_front_f_col.brakeTorque = _brakeForce;
    //        _left_front_c_col.brakeTorque = _brakeForce;
    //        _left_rear_c_col.brakeTorque = _brakeForce;
    //        _left_rear_r_col.brakeTorque = _brakeForce;

    //        _right_rear_r_col.brakeTorque = _brakeForce;
    //        _right_rear_c_col.brakeTorque = _brakeForce;
    //        _right_front_c_col.brakeTorque = _brakeForce;
    //        _right_front_f_col.brakeTorque = _brakeForce;
    //    }
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        Debug.Log("Breaking");
    //        _left_front_f_col.brakeTorque = 0;
    //        _left_front_c_col.brakeTorque = 0;
    //        _left_rear_c_col.brakeTorque = 0;
    //        _left_rear_r_col.brakeTorque = 0;

    //        _right_rear_r_col.brakeTorque = 0;
    //        _right_rear_c_col.brakeTorque = 0;
    //        _right_front_c_col.brakeTorque = 0;
    //        _right_front_f_col.brakeTorque = 0;
    //    }
    //}

    // величина поворота
    void Steering()
    {
        _steer = _h * _steer_Angle;
        // поворот первых двух левых колес
        _left_front_f_col.steerAngle = _steer;
        _left_front_c_col.steerAngle = _steer;
        // поворот первых двух правых колес
        _right_front_c_col.steerAngle = _steer;
        _right_front_f_col.steerAngle = _steer;
    }

    void UpdateWheelPos(WheelCollider colider, Transform transform)
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        colider.GetWorldPose(out position, out rotation);
        transform.position = position;
        transform.rotation = rotation;
    }
}