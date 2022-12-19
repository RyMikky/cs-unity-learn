using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseTargetSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))        Debug.Log("Нажата ЛКМ");
    }

    void OnMouseOver()
    {
        //Debug.Log("Mouse is over GameObject.");
        //Debug.Log(this.name);
        //Transform target = this.gameObject.GetComponent<Transform>();
    }
}
