using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10F;
    public static CinemachineCameraZoom2D Instance { get; private set; }

    [SerializeField] private CinemachineCamera _cinemachineCamera;
    private float _targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        float zoomSpeed = 2f;
        _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    public void SetOrthographicSize(float targetOrthographicSize)
    {
        this._targetOrthographicSize = targetOrthographicSize;
    }
    public void SetNormalOrthographicSize()
    {
        SetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
