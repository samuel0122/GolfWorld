using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    public CinemachineVirtualCameraBase PlayerCamera;
    public Camera MainCamera;
    public TrackManager TrackManager;

    public float MaxDamage= 5f;
    public float MaxForce = 1.5f;
    public float ForceAcceleration = 1.5f;
    public Color MinForceColor = Color.green;
    public Color MaxForceColor = Color.red;

    private float _defaultLowestY = -10f;
    public float lowestY;

    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private float _currentForce;
    private float _pingPongTime;
    private bool _canShoot;
    private float _timeInHole;

    private float _maxSpeed = 9f;

    private Vector3 currentTrackCoord;
    private Vector3 currentFlagCoord;

    // To avoid entering the hold function when holding click on move
    private bool _validClick;

    // TODO: refactorizar un poco e implementar función de detener la bola con click derecho
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.enabled = false;

        lowestY = _defaultLowestY;

    }

    public void SpawnTo(Vector3 point)
    {
        point.y += 0.5f;
        currentTrackCoord = point;
        currentFlagCoord = TrackManager.getFlagPosition();
        _lineRenderer.enabled = false;

        _rigidbody.MovePosition(point);
    }

    private double getDistanceWithFlag()
    {
        double distanceX = Math.Abs(_rigidbody.position.x - currentFlagCoord.x);
        double distanceZ = Math.Abs(_rigidbody.position.z - currentFlagCoord.z);

        return (distanceX) + (distanceZ);
    }

    public float getSpeed() {
        return _rigidbody.velocity.magnitude; 
    }

    // Gets player's dmg hit bassed on its speed
    public float getHitDamage()
    {
        return (getSpeed() * 1.5f >= _maxSpeed) ? MaxDamage : (((getSpeed() * 1.5f) / _maxSpeed) * MaxDamage);
    }

    public void resetLowestY() { lowestY = _defaultLowestY; }

    // Constantly update the player
    private void Update()
    {

        // Looks if the player is out of the track (its going down)
        if (_rigidbody.position.y < lowestY)
        {
            SpawnTo(currentTrackCoord);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }

        // Calculate player's distance with flag
        float flagDistance = (float)getDistanceWithFlag();
        if (flagDistance < 1f)
        {
            //Move flag
            TrackManager.elevateFlagTo((1f - flagDistance) / 2);
        }

        // Check if player is moving
        _canShoot = _rigidbody.velocity.magnitude < 0.1f;

        if (!_canShoot)
            return;
        

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        ProcessOnMouseDown();
        ProcessOnMouseUp();
        ProcessOnMouseHold();
    }

    // Function when clicking down mouse
    private void ProcessOnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //PlayerCamera.gameObject.SetActive(false);
            _validClick = true;

            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.enabled = true;

            _currentForce = 0;
            _pingPongTime = 0;
        }
    }

    // Function when clicking off mouse
    private void ProcessOnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && _validClick)
        {
            //PlayerCamera.gameObject.SetActive(true);
            _validClick = false;
            _lineRenderer.enabled = false;

            var cameraForward = MainCamera.transform.forward;
            var forceDirection = new Vector3(cameraForward.x, 0, cameraForward.z) * _currentForce;

            _rigidbody.AddForce(forceDirection, ForceMode.Impulse);
            
        }
    }

    // Function while holding down mouse click
    private void ProcessOnMouseHold()
    {
        if (Input.GetMouseButton(0) && _validClick)
        {
            _pingPongTime += Time.deltaTime;

            _currentForce = Mathf.PingPong(ForceAcceleration * _pingPongTime, MaxForce);

            var cameraForward = MainCamera.transform.forward;
            var playerPosition = transform.position;
            var newPosition = playerPosition + new Vector3(cameraForward.x, 0, cameraForward.z) * _currentForce;

            _lineRenderer.SetPosition(1, newPosition);
            _lineRenderer.startColor = _lineRenderer.endColor = Color.Lerp(MinForceColor, MaxForceColor, _currentForce);
        }
    }



    //Calculate how long I'm in the hole
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            _timeInHole = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            _timeInHole += Time.deltaTime;

            if (_timeInHole > 1.5f)
            {
                TrackManager.NextTrack();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            _timeInHole = 0;
        }
    }
}
