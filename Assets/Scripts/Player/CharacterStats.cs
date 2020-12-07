using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    [Header("General Stats")]
    [SerializeField] protected bool _canMove;
    [SerializeField] protected bool _canLook;
    [SerializeField] protected bool _canInteract;

    [Header("Dynamic Variables")]
    [SerializeField] protected float _movementSpeed;

    [Header("Interaction")]
    [SerializeField] protected float _interactionDistance;
    [SerializeField] protected float _interactionRadius;
    [SerializeField] protected LayerMask _interactionLayerMask;
    [SerializeField] protected GameObject _hoveringOnObject;

    [Header("Movement")]
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _runningSpeed;

    [Header("CameraOptions")]
    [SerializeField] protected CameraFunctions _camFunctions;

    [Header("References")]
    [SerializeField] protected Rigidbody _playerRigidbody;
    [SerializeField] protected GameObject _playerCamera;

    [Header("Inventory")]
    [SerializeField] protected List<Item> _inventory;
}
