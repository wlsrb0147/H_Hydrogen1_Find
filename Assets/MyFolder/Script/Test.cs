using System;
using UnityEngine;

public class Test : MonoBehaviour
{

    public void Turn()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
    }
}
