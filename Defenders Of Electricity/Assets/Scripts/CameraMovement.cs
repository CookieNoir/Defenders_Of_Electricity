using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panningSpeed = 5f;
    public float scrollSpeed = 2f;
    public float sensetivity = 1000f;

    public float paddingFromScreenBorders = 30f;
    public float bottomClamp = 10f;
    public float topClamp = 80f;

    public float xAxisAmplitude;
    public float zAxisAmplitude;

    void Update()
    {
        doPanning();
        doZoom();
        ClampPosition();
    }

    void doPanning()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * panningSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * panningSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * panningSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * panningSpeed * Time.deltaTime, Space.World);
    }

    void doZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 position = transform.position;

        position.y -= scroll * sensetivity * scrollSpeed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, bottomClamp, topClamp);

        transform.position = position;
    }

    private void ClampPosition()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xAxisAmplitude, xAxisAmplitude), transform.position.y, Mathf.Clamp(transform.position.z, -zAxisAmplitude, zAxisAmplitude));
    }
}
