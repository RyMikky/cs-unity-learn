using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAtTarget : MonoBehaviour
{
    public bool Enable = false;

    public Transform LockAt_Target;                 // объект вдоль которого будет осуществлён поворот
    public Transform Tower_Axix;                    // объект - ось вращение башни танка

    void Update()
    {
        if (Enable)
        {
            // берем нормаль от разницы векторов позиций
            Vector3 forward = (LockAt_Target.position - Tower_Axix.position).normalized;

            forward.y = 0; // обнуляем ось y, чтобы башня всегда крутилась только вокруг неё

            // поворачиваем башню за объектом преследования
            Tower_Axix.rotation = Quaternion.LookRotation(forward);
        }
    }
}