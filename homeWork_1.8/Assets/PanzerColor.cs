using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzerColor : MonoBehaviour
{
    public bool _Use_Default = true;
    public bool _Green_Color = false;
    public bool _Red_Color = false;

    public List<Component> _Objects;
    public List<Material> _PanzerColors;

    // Start is called before the first frame update
    void Start()
    {
        if (!_Use_Default)
        {
            foreach (var obj in _Objects)
            {
                if (_Green_Color) obj.GetComponent<MeshRenderer>().material = _PanzerColors[0];
                if (_Red_Color) obj.GetComponent<MeshRenderer>().material = _PanzerColors[1];
            }
        }
    }
}