using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public Vector3 Rotation;           // величина поворота

    // Update is called once per frame
    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(Rotation * Time.deltaTime);
        // тут ВАЖЕН порядок - необходимо довернуть дельта на текущий поворот, а не наоборот!
        transform.rotation = deltaRotation * transform.rotation;
    }
}
