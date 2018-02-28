using UnityEngine;

public class CameraController : MonoBehaviour
{
    // This script is attached to the camera object!

    public GenerateGridManager game;

    public float cameraSpeed = 10.0f;
    public float zoomSpeed = 8.0f;

    float clampRow0;
    float clampCol0;

    Vector3 newPos;

    void Start()
    {
        newPos = transform.position;
    }

    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        clampRow0 = game.row * game.padding;
        clampCol0 = game.column * game.padding;

        newPos.x += xMove * cameraSpeed * Time.deltaTime;
        newPos.y += yMove * cameraSpeed * Time.deltaTime;

        // Camera clamp to prevent over-offset.
        newPos.x = Mathf.Clamp(newPos.x, 0.0f, (float)(clampRow0 - 1.0f));
        newPos.y = Mathf.Clamp(newPos.y, 0.0f, (float)(clampCol0 - 1.0f));

        transform.position = newPos;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            CameraZoom(zoomSpeed);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            CameraZoom(-zoomSpeed);
        }
    }

    void CameraZoom(float zMove)
    {
        newPos.z += zMove * Time.deltaTime;

        newPos.z = Mathf.Clamp(newPos.z, (float)(-clampRow0 - clampCol0), (float)((-clampRow0 - clampCol0) / 10.0f));

        transform.position = newPos;
    }
}