using UnityEngine;

public class CameraStats : MonoBehaviour{

    [Header("General Stats")]
    [SerializeField] protected float _horizontalSensitivity;
    [SerializeField] protected float _verticalSensitivity;
    [SerializeField] protected bool _invertVertical;
    [SerializeField] protected bool _invertHorizontal;
    [SerializeField] protected Vector2 _viewRange;
    protected float _currentVerticalRotation;

    [Header("Dynamic Variables")]
    [SerializeField] protected float _currentHor;
    [SerializeField] protected float _currentVer;

    [Header("References")]
    [SerializeField] protected GameObject _cameraHolder;
}
