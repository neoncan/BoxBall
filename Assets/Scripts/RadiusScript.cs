using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiusScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;
            transform.position = mouseWorldPosition;
        }
    }
}
