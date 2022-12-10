using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    public Rigidbody _levitation_body;              // ����������� ������
    public int _motion_time = 2;                    // ����� � �������� � ����� �� ������
    public float _y_velocity = 0.5f;                // �������� ��� �������� �� ������������ ���
    public bool _use_debug = false;                 // ���� ��������� �������
    public bool _randomize_start = false;           // ���� ���������� ������ (�������� ����� ��� ����)

    private int _impulse_second = 0;                // ��������� ������� �������� ��������
    private bool _impulse_is_down = false;          // ������ ��� ������������ �������� (�� ����� ���� ������ ����)

    // Start is called before the first frame update
    void Start()
    {
        DirectionChecking();                        // �������� �������� ���������� ��������

        // ���� ������� ��������� �����
        if (_randomize_start)
        {
            // ������ ���������� ��� ���������� ����������� �����������
            System.Random rnd = new System.Random();
            
            if (rnd.Next(0,2) > 0)                    // �������� ������ ����? 
            {
                StartLevitation(_y_velocity);         // �������� � ������ �����������
            }
            else
            {
                StartLevitation(-(_y_velocity));      // �������� � �������� �����������
            }
        }
        else
        {
            StartLevitation(_y_velocity);             // �������� � ������ �����������
        }
    }

    private void Update()
    {
        // ����� ������� �������
        int _motion_second = ((int)Time.timeAsDouble % 60) % 60;

        // ���� ������� ������� ����� ���������
        if (_motion_second == _impulse_second)
        {
            // ������� � ������� ���������� ��� �������
            if (_use_debug)
            {
                Debug.Log("Current_second is - " + _impulse_second);
                Debug.Log("Motion_second is - " + _motion_second);
            }

            // ������ ���������� ��������
            UpdateMove();  // ������� �������� �������

            // ��������� ����� ���������� �������� ��������
            _impulse_second += _motion_time;
            // ������ ��� ����� ��������� �� ������� 60 - ������ �������� 60
            if (_impulse_second > 59) _impulse_second -= 60;
        }
    }

    // �������� ����������� ���������� ��������
    private void DirectionChecking()
    {
        // ���� ������ ������������� ��������, ������ ������� ������� ����
        if (_y_velocity < 0) _impulse_is_down = true;
    }

    // ��������� ��������
    private void StartLevitation(float y_velocity)
    {
        // ��� ������ ������ ���������� �����
        _levitation_body.AddForce(new Vector3(0, y_velocity, 0), ForceMode.Impulse);
        // ������� ������� �����
        _impulse_second += _motion_time;
    }

    // ������� ���������� ��������
    private void UpdateMove()
    {
        // ���� ��������� ������� ������������� � ������� ������������ �������� ������ ����
        if(_impulse_is_down && _levitation_body.velocity.y > 0 
            // ��� ��������� ������� �������������, � ������� ������������ �������� ���� ����
            || !_impulse_is_down && _levitation_body.velocity.y < 0)
        {
            // ����������� ����� ������ � ��������� ������������� ������������ ���������
            _levitation_body.AddForce(new Vector3(0, (_y_velocity * 2), 0), ForceMode.Impulse);
        }
        else
        {
            // ����������� ����� ������ � ��������� ������������� ������������ ���������
            _levitation_body.AddForce(new Vector3(0, -(_y_velocity * 2), 0), ForceMode.Impulse);
        }
    }
}