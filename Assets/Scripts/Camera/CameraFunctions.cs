using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunctions : CameraStats {

    //Initialization
    private void Awake(){
        HorizontalSensitivity = HorizontalSensitivity;
        VerticalSensitivity = VerticalSensitivity;
        InvertVertical = InvertVertical;
        InvertHorizontal = InvertHorizontal;
    }

    //General Stats
    public float HorizontalSensitivity {
        get => _horizontalSensitivity;
        set {
            _horizontalSensitivity = value;
            InvertHorizontal = InvertHorizontal;
        }
    }
    public float VerticalSensitivity {
        get => _verticalSensitivity;
        set {
            _verticalSensitivity = value;
            InvertVertical = InvertVertical;
        }
    }
    public bool InvertVertical {
        get => _invertVertical;
        set {
            CurrentVerSens = value ? Mathf.Abs(VerticalSensitivity) : Mathf.Abs(VerticalSensitivity) * -1;
            _invertVertical = value;
        }
    }
    public bool InvertHorizontal {
        get => _invertHorizontal;
        set {
            CurrentHorSens = value ? Mathf.Abs(HorizontalSensitivity) * -1 : Mathf.Abs(HorizontalSensitivity);
            _invertHorizontal = value;
        }
    }
    public Vector2 ViewRange {
        get => _viewRange;
        set => _viewRange = value;
    }
    public float CurrentVerticalRotation {
        get => _currentVerticalRotation;
        set => _currentVerticalRotation = value;
    }

    //Dynamic Stats
    public float CurrentHorSens {
        get => _currentHor;
        set => _currentHor = value;
    }
    public float CurrentVerSens {
        get => _currentVer;
        set => _currentVer = value;
    }

    //References
    public GameObject CameraHolder {
        get => _cameraHolder != null ? _cameraHolder : _cameraHolder = transform.Find("Movement").gameObject;
    }

    //Functions
    public void MoveCamera(float ver, float hor = 0) {

        //Tilts the Camera up and down
        if (ver != 0) TiltCamera(ver);

        //Rotates the Camera
        if (hor != 0) RotateCamera(hor);
        
    }

    //Tilts the Camera up and down and sets the limits
    public void TiltCamera(float ver) {
        //Does a Rotation
        CurrentVerticalRotation += ver * CurrentVerSens;

        //Exits the rotation if it is out of bounds
        if (CurrentVerticalRotation >= ViewRange.y){
            CurrentVerticalRotation = ViewRange.y;
            return;
        }else if (CurrentVerticalRotation <= ViewRange.x) {
            CurrentVerticalRotation = ViewRange.x;
            return;
        }

        //Sets the rotation if it is in bounds
        CameraHolder.transform.Rotate(Vector3.right * ver * CurrentVerSens);
    }

    //Rotates the Camera if the player is in a third person mode
    public void RotateCamera(float hor) { 
    
    }
}
