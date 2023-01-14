using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]

public class Player : MonoBehaviour
{
    // Public variables
    public Camera mainCamera;
    public LevelManager levelManager;

    public float maxDamage= 5f;

    public Color MinForceColor = Color.green;
    public Color MaxForceColor = Color.red;


    // Player's objects
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    
    // Player's variables
    private float _currentForce;
    private float _pingPongTime;
    
    private float _maxForce = 1.5f;
    private float _forceAcceleration = 1.5f;

    private float _maxSpeed = 9f;
    private float _minSpeed = 0.1f;

    // Level's variables
    private float _timeInHole;
    private float _lowestY;

    private Vector3 currentLevelCoord;
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
    }

    public void SpawnTo(Vector3 point)
    {
        point.y += 0.5f;
        currentLevelCoord = point;
        currentFlagCoord = levelManager.getFlagPosition();
        _lineRenderer.enabled = false;

        _rigidbody.MovePosition(point);

        // Sets the lowest Y the player can go
        _lowestY = levelManager.getMaxDrop();
    }

    private float getDistanceWithFlag()
    {
        float distanceX = Math.Abs(_rigidbody.position.x - currentFlagCoord.x);
        float distanceZ = Math.Abs(_rigidbody.position.z - currentFlagCoord.z);

        return new Vector3(distanceX, 0, distanceZ).magnitude;
    }

    public float getSpeed() { return _rigidbody.velocity.magnitude; }

    // Gets player's dmg hit bassed on its speed
    public float getHitDamage()
    {
        return (getSpeed() * 1.5f >= _maxSpeed) ? maxDamage : (((getSpeed() * 1.5f) / _maxSpeed) * maxDamage);
    }

    // Constantly update the player
    private void Update()
    {

        // Looks if the player is out of the track (its going down)
        if (_rigidbody.position.y < _lowestY)
        {
            SpawnTo(currentLevelCoord);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }

        // Calculate player's distance with flag
        float flagDistance = getDistanceWithFlag();
        if (flagDistance < 1f)
        {
            //Move flag
            levelManager.elevateFlagTo((1f - flagDistance) / 2);
        }

        // Check if player is moving
        if (getSpeed() >= _minSpeed)
        {
            ProcessRightClick();
            return;
        }
        
        // If speed is below min, turn it Z and press mouse
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        ProcessLeftClickDown();
        ProcessLeftClickOff();
        ProcessLeftClickHold();
    }

    private void ProcessRightClick()
    {
        if (Input.GetMouseButtonDown(1) && !levelManager.getCurrentLevel().areEnemiesDead())
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    // Function when clicking down mouse
    private void ProcessLeftClickDown()
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
    private void ProcessLeftClickOff()
    {
        if (Input.GetMouseButtonUp(0) && _validClick)
        {
            //PlayerCamera.gameObject.SetActive(true);
            _validClick = false;
            _lineRenderer.enabled = false;

            var cameraForward = mainCamera.transform.forward;
            var forceDirection = new Vector3(cameraForward.x, 0, cameraForward.z) * _currentForce;

            _rigidbody.AddForce(forceDirection, ForceMode.Impulse);
            
        }
    }

    // Function while holding down mouse click
    private void ProcessLeftClickHold()
    {
        if (Input.GetMouseButton(0) && _validClick)
        {
            _pingPongTime += Time.deltaTime;

            _currentForce = Mathf.PingPong(_forceAcceleration * _pingPongTime, _maxForce);

            var cameraForward = mainCamera.transform.forward;
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
                levelManager.NextLevel();
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
