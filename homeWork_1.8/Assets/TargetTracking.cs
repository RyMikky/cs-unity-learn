using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TargetTracking : MonoBehaviour
{
    public bool _Enable = false;
    public bool _DebugDistance = false;
    public bool _ShowManualDisable = false;

    public Transform _LockAt_Target;                // объект вдоль которого будет осуществлён поворот
    public Transform _Tower_Axix;                   // объект - ось вращение башни танка
    public Transform _Gun_Axix;                     // объект - ось привязки ствола пушки

    public float _TowerRotationSpeed = 1;           // базовая скорость вращения башни
    public float _GunRotationSpeed = 0.1f;          // базовая скорость возвышения ствола
    public Camera _TankCamera;                      // основная камера которая будет "стрелять" рейкастами

    public GameObject _GunEdge;                     // крайняя точка ствола танка откуда будет происходить выстрел
    public GameObject _TankBullet;                  // танковый снаряд, которым будет происходить выстрел
    public float _GunPower = 10f;                   // базовая мощность пушки
    public float _BulletLifeTime = 2f;              // время существования снаряда

    private Vector3 _TowerRotation = new Vector3(0, 0, 0);                 // закрытое поле поворота башни
    private Vector3 _PastTowerRotation = new Vector3(0, 0, 0);             // закрытое поле поворота башни
    private Vector3 _GunRotation = new Vector3(0, 0, 0);                   // закрытое поле возвышения ствола
    private float _Distance = 0;                                           // расстояние до цели
    private Transform _onMouseOver;                                        // поле положения указателя мыши

    void Update()
    {
        MouseRayCast();                      // порверяем нет ли выбора новой цели

        if (_Enable)
        {
            DistanceUpdate();                // тогда считаем дистанцию до цели
            TowerAtTargetRotation();         // доворачиваем башню до цели
            Attack();                        // производим выстрел, если нажата кнопка пробела
        }
    }

    private void TowerAtTargetRotation()
    {
        // Берем разницу векторов позиций башни и цели
        Vector3 towerTargetDirection = (_LockAt_Target.position - _Tower_Axix.position).normalized;
        // Пересчитываем скорось относительно времени кадра
        float towerSingleStep = _TowerRotationSpeed * Time.deltaTime;
        // Расчитываем доворот башни по встроенному методу
        _TowerRotation = Vector3.RotateTowards(_Tower_Axix.forward, towerTargetDirection, towerSingleStep, 0.0f);
        Debug.Log("_TowerRotation - " + _TowerRotation);
        Debug.Log("_PastTowerRotation - " + _PastTowerRotation);
        _TowerRotation.y = 0; // Блокируем ось y, чтобы башня не крутилась чёрти как
        // Производим вращение башни
        _Tower_Axix.rotation = Quaternion.LookRotation(_TowerRotation);

        if (RotationsIsEqual(_PastTowerRotation, _TowerRotation))
        {
            // Берем разницу векторов позиций ствола и цели
            Vector3 gunTargetDirection = _LockAt_Target.position - _Gun_Axix.position;
            // Пересчитываем скорось относительно времени кадра
            float gunSingleStep = _GunRotationSpeed * Time.deltaTime;
            // Расчитываем возвышение ствола башни по встроенному методу
            _GunRotation = Vector3.RotateTowards(_Gun_Axix.forward, gunTargetDirection, gunSingleStep, 0.0f);

            if (_GunRotation.y > 0.2) _GunRotation.y = 0.2f;
            if (_GunRotation.y < -0.02) _GunRotation.y = -0.02f;

            Debug.DrawRay(_Gun_Axix.position, gunTargetDirection, Color.red);
            // Возвышаем ствол орудия
            _Gun_Axix.rotation = Quaternion.LookRotation(_GunRotation);
        }
        else
        {
            _PastTowerRotation.x = _TowerRotation.x;
            _PastTowerRotation.z = _TowerRotation.z;
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
            }
            // обновляем флаг отслеживания цели
            if (!_Enable) _Enable = true;
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

            Debug.Log("Bullet direct - " + (_LockAt_Target.position - _GunEdge.transform.position).normalized);
            // сила воздействующая на снаряд считается от дистанции до объекта
            r_bullet.AddForce((_LockAt_Target.position - _GunEdge.transform.position).normalized * _GunPower/* * _Distance*/, ForceMode.Impulse);

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

    private bool RotationsIsEqual(Vector3 a, Vector3 b)
    {
        if (a == b) return true;
        
        else if (a.x == b.x && a.z == b.z) return true;

        else if ((a.x - b.x <= 0.01/* || a.x - b.x >= -0.01*/)
            && (a.z - b.z <= 0.01 /*|| a.z - b.z >= -0.01*/)) return true;

        else
        {
            return false;
        }
    }
}