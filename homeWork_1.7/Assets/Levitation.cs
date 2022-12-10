using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    public Rigidbody _levitation_body;              // настраиваем объект
    public int _motion_time = 2;                    // время в движении в любую из сторон
    public float _y_velocity = 0.5f;                // величина для импульса по вертикальной оси
    public bool _use_debug = false;                 // флаг включения деббага
    public bool _randomize_start = false;           // флаг рандомного старта (случайно вверх или вниз)

    private int _impulse_second = 0;                // ожидаемая секунда придания импульса
    private bool _impulse_is_down = false;          // чекаем тип изначального импулься (он может быть меньше нуля)

    // Start is called before the first frame update
    void Start()
    {
        DirectionChecking();                        // вызываем проверку начального движения

        // если включён рандомный старт
        if (_randomize_start)
        {
            // создаём переменную для рандомного определения направления
            System.Random rnd = new System.Random();
            
            if (rnd.Next(0,2) > 0)                    // значение больше нуля? 
            {
                StartLevitation(_y_velocity);         // стуртуем в прямом направлении
            }
            else
            {
                StartLevitation(-(_y_velocity));      // стуртуем в обратном направлении
            }
        }
        else
        {
            StartLevitation(_y_velocity);             // стуртуем в прямом направлении
        }
    }

    private void Update()
    {
        // берем текущую секунду
        int _motion_second = ((int)Time.timeAsDouble % 60) % 60;

        // если текущая секунда равна ожидаемой
        if (_motion_second == _impulse_second)
        {
            // выводим в консоль информацию для деббага
            if (_use_debug)
            {
                Debug.Log("Current_second is - " + _impulse_second);
                Debug.Log("Motion_second is - " + _motion_second);
            }

            // делаем обновление движения
            UpdateMove();  // придаем обратный импульс

            // обновляем время следующего придания импульса
            _impulse_second += _motion_time;
            // каждый раз когда переходим за границу 60 - просто вычитаем 60
            if (_impulse_second > 59) _impulse_second -= 60;
        }
    }

    // проверка направления начального движения
    private void DirectionChecking()
    {
        // если задают отрицательное значение, значит импульс толкает вниз
        if (_y_velocity < 0) _impulse_is_down = true;
    }

    // стартовое движение
    private void StartLevitation(float y_velocity)
    {
        // при старте просто отправляем вверх
        _levitation_body.AddForce(new Vector3(0, y_velocity, 0), ForceMode.Impulse);
        // плюсуем текущее время
        _impulse_second += _motion_time;
    }

    // обычное обновление движения
    private void UpdateMove()
    {
        // если стартовый импульс отрицательный и текущая вертикальная скорость больше нуля
        if(_impulse_is_down && _levitation_body.velocity.y > 0 
            // или стартовый импульс положительный, а текущая вертикальная скорость ниже нуля
            || !_impulse_is_down && _levitation_body.velocity.y < 0)
        {
            // присваиваем новый вектор с удвоенной положительной вертикальной скоростью
            _levitation_body.AddForce(new Vector3(0, (_y_velocity * 2), 0), ForceMode.Impulse);
        }
        else
        {
            // присваиваем новый вектор с удвоенной отрицательной вертикальной скоростью
            _levitation_body.AddForce(new Vector3(0, -(_y_velocity * 2), 0), ForceMode.Impulse);
        }
    }
}