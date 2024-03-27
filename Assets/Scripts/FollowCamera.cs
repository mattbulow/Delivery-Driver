using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // this objects position will be the same as the player cars position

    [SerializeField] private GameObject _car;
    [SerializeField] private Collider2D gameBounds;
    private Camera _camera;

    [SerializeField] private float boundsCameraOffset = 2f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        // if car position + camera size > bounds, then set camera to bounds - camera size

        float orthoSize = _camera.orthographicSize;
        float aspect = _camera.aspect;

        // for x direction we need to take the ortho size and use aspect to get a real x vlaue
        float x_cameraHalfSize = orthoSize * aspect;

        

        float x_pos;
        if (_car.transform.position.x + x_cameraHalfSize >= gameBounds.bounds.max.x+ boundsCameraOffset)
        {
            x_pos = gameBounds.bounds.max.x - x_cameraHalfSize+ boundsCameraOffset;
        } 
        else if (_car.transform.position.x - x_cameraHalfSize <= gameBounds.bounds.min.x- boundsCameraOffset)
        {
            x_pos = gameBounds.bounds.min.x + x_cameraHalfSize- boundsCameraOffset;
        } else
        {
            x_pos = _car.transform.position.x;
        }

        float y_pos;
        if (_car.transform.position.y + orthoSize >= gameBounds.bounds.max.y+ boundsCameraOffset)
        {
            y_pos = gameBounds.bounds.max.y - orthoSize+ boundsCameraOffset;
        }
        else if (_car.transform.position.y - orthoSize <= gameBounds.bounds.min.y- boundsCameraOffset)
        {
            y_pos = gameBounds.bounds.min.y + orthoSize- boundsCameraOffset;
        }
        else
        {
            y_pos = _car.transform.position.y;
        }

        // want to follow player car unless camera is at bounds.
        this.transform.position = new Vector3(x_pos, y_pos, -10);
    }
}
