using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс управляющий работой башни танка
/// Включет фуекционал выбора цели танка, доворота башни и ствола до выбраной цели, выстрела снарядом
/// </summary>

public class TowerEngine : MonoBehaviour
{
    public bool _Enable = false;                    // включение работы класса, отслеживание цели
    public bool _DebugDistance = false;             // печатает в консоль дистанцию до цели
    public bool _ShowManualDisable = false;         // печатает в консоль дистанцию до цели посчитанную нативно

    // --------------------------------- основные объекты трансформации ------------------------------------------

    public Transform _LockAt_Target;                // объект вдоль которого будет осуществлён поворот
    public Transform _Tower_Axix;                   // объект - ось вращение башни танка
    public Transform _Gun_Axix;                     // объект - ось привязки ствола пушки
    public Transform _TankFront;                    // объект - точка на носу корпуса танка

    public float _TowerRotationSpeed = 1f;          // базовая скорость вращения башни
    public float _GunLevelingSpeed = 1f;            // базовая скорость возвышения ствола
    public float _GunUpAngleLimmit = 12f;           // максимальный угол возвышеня ствола
    public float _GunDownAngleLimmit = 4f;          // максимальный угол опускания ствола
    public Camera _TankCamera;                      // основная камера которая будет "стрелять" рейкастами

    // --------------------------------- поля настройки стрельбы -------------------------------------------------

    public GameObject _GunEdge;                     // крайняя точка ствола танка откуда будет происходить выстрел
    public GameObject _TankBullet;                  // танковый снаряд, которым будет происходить выстрел
    public float _GunPower = 10f;                   // базовая мощность пушки
    public float _BulletLifeTime = 2f;              // время существования снаряда

    // --------------------------------- закрытые внутренние поля ------------------------------------------------

    private Vector3 _GunRotation = new Vector3(0, 0, 0);                   // закрытое поле возвышения ствола
    private float _Distance = 0;                                           // расстояние до цели
    private Transform _onMouseOver;                                        // поле положения указателя мыши

    void Update()
    {
        MouseRayCast();                      // порверяем нет ли выбора новой цели

        if (_Enable)
        {
            DistanceUpdate();                // тогда считаем дистанцию до цели
            TowerAtTargetRotation(_LockAt_Target);         // доворачиваем башню до цели
        }
        else
        {
            TowerAtTargetRotation(_TankFront);             // доворачиваем башню и ставим её по центру
        }

        GunLeveling();                       // возвышаем ствол на нужную величину
        Attack();                            // производим выстрел, если нажата кнопка пробела
    }

    private void TowerAtTargetRotation(Transform target)
    {
        // Берем разницу векторов позиций башни и цели
        Vector3 towerTargetDirection = (target.position - _Tower_Axix.position).normalized;
        // Пересчитываем скорось относительно времени кадра
        float towerSingleStep = _TowerRotationSpeed * Time.deltaTime;
        // Расчитываем доворот башни по встроенному методу
        Vector3 towerAxixRotation = 
            Vector3.RotateTowards(_Tower_Axix.forward, towerTargetDirection, towerSingleStep, 0.0f);
        // Блокируем ось y, чтобы башня не крутилась чёрти как
        towerAxixRotation.y = 0;
        // Производим вращение башни
        _Tower_Axix.rotation = Quaternion.LookRotation(towerAxixRotation);
    }

    // метод смены возвышения дула ствола
    private void GunLeveling()
    {
        // проверяем есть ли назначенная цель
        if (HaveATarget())
        {
            // производим расчёт угла требуемого возвышения ствола орудия
            LevelingAngle(out float angle);
            // задаем параметры врашения вектору поворота ствола
            GunTargeting(angle);
        }
        else
        {
            GunTargeting(-2f);
        }

        // применяем обычный поворот эейлера по полученному вектору
        Quaternion gunRotation = Quaternion.Euler(_GunRotation * Time.deltaTime);
        // производим изменение угла возвышения орудия
        _Gun_Axix.localRotation = gunRotation * _Gun_Axix.localRotation;
    }

    // метод определения требуемого угла возвышения башни
    private void LevelingAngle(out float angle)
    {
        // для определения угла возвышения воспользуемся соотношением углов в прямоугльном треугольнике АВС,
        // где А - точка нахождения танка, В - точка нахождения цели, С - точка перпендикулярная к А и В, обладающая углом в 90 градусов
        // sin a = a / c, где синус альфа - угол возвышения башни танка, а - катет между ВС, с - гиппотенуза АВ.

        bool TragetUpper = _Gun_Axix.position.y < _LockAt_Target.position.y;                  // флаг относительного положения цели относительно ствола

        float hypotAB = Vector3.Distance(_LockAt_Target.position, _Gun_Axix.position);   // гиппотенуза по факту является дистанцией до цели
        Vector3 C_point = _LockAt_Target.position; C_point.y = _Gun_Axix.position.y;     // точка С по горизоналям назодится под целью, но на высоте башни танка
        float katetBC = Vector3.Distance(_LockAt_Target.position, C_point);              // катет расчитывается также как гиппотенуза

        Debug.DrawRay(_Gun_Axix.position, _LockAt_Target.position - _Gun_Axix.position, Color.red);
        Debug.DrawRay(C_point, _LockAt_Target.position - C_point, Color.red);
        Debug.DrawRay(_Gun_Axix.position, C_point - _Gun_Axix.position, Color.red);

        angle = katetBC / hypotAB;                                                       // производим деление катета на гиппотенузу
        angle = Mathf.Asin(angle);                                                       // переврдим значение в арксинус и радианы
        angle = angle * 180 / Mathf.PI;                                                  // преобразуем из радиан в градусы

        if (TragetUpper) angle = -angle;                                                 // обращаем знак угла, при возвышении цели
    } 

    // расчёт вектора поворота орудия башни
    private void GunTargeting(float angle)
    {
        // смотрим текущий угол относительного возвышения ствола
        float currentAngle = _Gun_Axix.localRotation.eulerAngles.x;

        // если получили угол меньше нуля - значит надо поднимать ствол - работаем в верхней полусфере
        if (angle < 0)
        {
            // есди текущий угол меньше десятка - скорее всего он до этого смотрел "вниз"
            if (currentAngle < 30)
            {
                _GunRotation.x = -_GunLevelingSpeed * 10;      // подымаем орудие
            }
            else
            {
                // если текущее возвышение равно нулю или текущий относительный угол минус требуемый больше 0,01
                if (currentAngle - (360 + angle) > 0.02 && (360 - currentAngle) <= _GunUpAngleLimmit)
                {
                    _GunRotation.x = -_GunLevelingSpeed * 10;   // подымаем орудие
                }
                // требуемое возвышение больше текущего - значит надо опустить орудие - цель ниже текущего направления
                else if ((360 + angle) - currentAngle > 0.02 && currentAngle != 0)
                {
                    _GunRotation.x = _GunLevelingSpeed * 10;    // опускаем орудие
                }
                else
                {
                    _GunRotation.x = 0;                         // останавливаем вращение
                }
            }
        }
        // если получен угол больше нуля - значит цель ниже и ствол надо опускать - работаем в нижней полусфере
        else if (angle > 0)
        {
            // если предыдущая цель была выше оси танка
            if (currentAngle > 330)
            {
                // опускаем орудие, чтобы перейти в нижнюю полусферу обзора
                _GunRotation.x = _GunLevelingSpeed * 10;
            }
            else
            {
                // если требуемый угол минус текущий больше 0,01
                if (angle - currentAngle > 0.02 && currentAngle <= _GunDownAngleLimmit)
                {
                    _GunRotation.x = _GunLevelingSpeed * 10;    // опускаем орудие
                }
                // если текущий угол больше требуемого - значит цель чуть выше но всё еще в нижней полусфере
                else if (currentAngle - angle > 0.02 && currentAngle != 0)
                {
                    _GunRotation.x = -_GunLevelingSpeed * 10;   // подымаем орудие
                }
                else
                {
                    _GunRotation.x = 0;                         // останавливаем вращение
                }
            }
        }
        else
        {
            _GunRotation.x = 0;                                 // останавливаем вращение
        }
    }

    private void DistanceUpdate()
    {
        _Distance = Vector3.Distance(_LockAt_Target.position, _Tower_Axix.position);

        if (_DebugDistance)
        {
            Debug.Log("Distance to target - " + _Distance);
            if (_ShowManualDisable) ManualDistanceCalculate();
        }
    }

    private void ManualDistanceCalculate()
    {
        float x = _LockAt_Target.position.x - _Tower_Axix.position.x;
        float y = _LockAt_Target.position.y - _Tower_Axix.position.y;
        float z = _LockAt_Target.position.z - _Tower_Axix.position.z;

        float distance = Mathf.Sqrt((x * x) + (y * y) + (z * z));
        Debug.Log("ManualDistance to target - " + distance);
    }

    private void MouseRayCast()
    {
        // при нажатии на ЛКМ
        if (Input.GetMouseButtonUp(0))
        {
            // пускаем рейкаст из камеры по положению мышки
            Ray ray = _TankCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // если рейкаст во что-то попал
            if (Physics.Raycast(ray, out hit))
            {
                // даём информацию о том с чем столкнулись
                Debug.Log("Hit object type - " + hit.GetType());
                // задаём новую цель
                _LockAt_Target = hit.transform;

                // обновляем флаг отслеживания цели
                if (!_Enable) _Enable = true;
            }
            else
            {
                // если рейкаст был в пустоту, то сбрасывавем цель и заканчиваем отслеживание
                if (_Enable) _Enable = false;
                _LockAt_Target = null;
            }
        }
    }

    private void Attack()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // берем позицию крайней точки ствола
            Vector3 start_point = _GunEdge.transform.position;
            // берем величину поворота крайней точки ствола
            Quaternion start_rotation = _GunEdge.transform.rotation;

            // создаём снаряд по полученным координатам и углу поворота
            GameObject bullet = Instantiate(_TankBullet, start_point, start_rotation) as GameObject;
            // берем физическое тело пули
            Rigidbody r_bullet = bullet.GetComponent<Rigidbody>();


            Debug.Log("Bullet direct - " + (_GunEdge.transform.position - _Gun_Axix.position).normalized);
            // сила воздействующая на снаряд считается от дистанции до объекта
            r_bullet.AddForce((_GunEdge.transform.position - _Gun_Axix.position).normalized * _GunPower/* * _Distance*/, ForceMode.Impulse);

            Rigidbody tank = GetComponentInChildren<Rigidbody>();
            tank.AddForce((_Gun_Axix.position - _GunEdge.transform.position).normalized * _GunPower/* * _Distance*/, ForceMode.Impulse);

            Destroy(bullet, _BulletLifeTime);
        }
    }

    private float DistanceScalar()
    {
        float result = 0f;
        if (_Enable)
        {
            result = ((_Distance / 100) + 1) * 5;
        }
        Debug.Log("DistanceScalar - " + result);
        return result;
    }

    private bool HaveATarget()
    {
        return _LockAt_Target != null;
    }
}