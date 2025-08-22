using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private Transform _landerStartPositionTransform;
    [SerializeField] private Transform _cameraStartTargetTransform;
    [SerializeField] private float _zoomOutOrthographicSize;

    //function to expose the above data

    public int GetLevelNumber()
    {
        return _levelNumber;
    }
    public Vector3 GetLevelStartPosition()
    {
        return _landerStartPositionTransform.position;
    }

    public Transform GetCameraStartTargetTransform()
    {
        return _cameraStartTargetTransform;
    }

    public float GetZoomOutOrthographicSize()
    {
        return _zoomOutOrthographicSize;
    }
}
