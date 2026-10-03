using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseData
{
    private static Camera _mainCamera = Camera.main;

    private static Vector3 CursorWorldPosOnNCP
    {
        get
        {
            return _mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                            Input.mousePosition.y,
                            _mainCamera.nearClipPlane));
        }
    }
    public static Vector3 FromCameraToCursor
    {
        get
        {
            return CursorWorldPosOnNCP - _mainCamera.transform.position;
        }
    }

    /// <summary>
    /// Get the 3D mouse position. The fallbackdistance is when the ray is not hitting anything, it will consider a point x studs in front to be the target mouse position.
    /// </summary>
    public static Vector3 MousePosition3D(int fallbackDistance = 100)
    {
        Vector3 mousePosition;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePosition = hit.point;
        }
        else
        {
            mousePosition = ray.GetPoint(fallbackDistance);
        }

        return mousePosition;
    }

    /// <summary>
    /// Get the GameObject where the mouse is currently on. Returns null when no GameObject is found at the mouse's position.
    /// </summary>
    /// <returns>The GameObject hit with the mousecursor.</returns>
    public static GameObject HitGameObjectWithMouse
    {
        get {
            Ray ray = new Ray(_mainCamera.transform.position, FromCameraToCursor);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
