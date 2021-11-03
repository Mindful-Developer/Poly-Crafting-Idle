using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFly : MonoBehaviour
{
    public float speed = 1.0f;
    public float sensitivity = 0.5f;
    Vector3 lastMouse = new Vector3(255, 255, 255);
    float totalRun = 2.0f;

    void Update()
    {
        // get mouse input
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * sensitivity, lastMouse.x * sensitivity, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;
        // get keyboard input
        Vector3 p = GetBaseInput();
        p = p * Time.deltaTime;
        p = p * speed;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        // vertical movement when left control or space is pressed
        if (Input.GetKey(KeyCode.LeftControl))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        // fast movement when shift is held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += (Time.deltaTime * speed);
            p_Velocity = p_Velocity * totalRun;
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p_Velocity = p_Velocity * (totalRun * 0.5f);
        }
        return p_Velocity;
    }
}
