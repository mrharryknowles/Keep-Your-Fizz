using UnityEngine;
using UnityEngine.UI; 

public class ColaCanController : MonoBehaviour
{
    private Vector3 _startMousePosition;
    private Vector3 _currentMousePosition;
    private Rigidbody2D _rigidbody2D;
    private bool _isDragging = false;
    private bool _isLaunching = false; // tracks if the player is in a 'slam state'
    private float _currentFizziness; //current fizziness value

    private Vector3 _originalScale;

    [SerializeField] private float launchForceMultiplier = 5f; //adjust the strenght of lauch
    [SerializeField] private float spinForceMultiplier = 200f; //controls how fast the can can spin
    [SerializeField] private float maxAngularVelocity = 100f; //maximum rotational spin
    [SerializeField] private float maxFizziness = 100f; //max value of fizziness (health)
    [SerializeField] private Slider fizzinessSlider; //referencing the UI slider

    [SerializeField] private Vector3 enlargedScale = new Vector3(2f, 2f, 1f); //scale of the player during the slam
    [SerializeField] private float slamRadius = 2f; //Radius of slam effect
    [SerializeField] private LayerMask enemyLayer; //Layer for enemies to detect during slam
    [SerializeField] private float slamDamage = 20f; //damage dealt by slam

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentFizziness = maxFizziness; // start with full (100) fizziness
        if (fizzinessSlider != null)
        {
            fizzinessSlider.maxValue = maxFizziness;  //set the slider's max value
            fizzinessSlider.value = _currentFizziness;  //set the initial value of the slider
        }
    }

    private void Update()
    {
        HandleMouseInput();
        UpdateFizzinessUI(); //update the UI slider with the current fizziness
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //takes the location of where the mouse clicked before launch/pull
            _startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startMousePosition.z = 0; //sets z to 0 since we're in 2D
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            //records the mouse position when let go
            _currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentMousePosition.z = 0;

            LaunchCan();
            _isDragging = false;
        }
    }

    private void LaunchCan()
    {
        //calculate the direction and force of the launch
        Vector2 launchDirection = (_startMousePosition - _currentMousePosition).normalized;
        float launchForce = Vector2.Distance(_startMousePosition, _currentMousePosition) * launchForceMultiplier;

        //apply the force to the rigidbody on the cola can
        _rigidbody2D.velocity = launchDirection * launchForce;

        //apply spin (angular velocity)
        float randomSpin = Random.Range(-1f, 1f); // Random spin direction
        float angularVelocity = randomSpin * spinForceMultiplier; // Calculate angular velocity

        //clamps angular velocity to the max value
        _rigidbody2D.angularVelocity = Mathf.Clamp(angularVelocity, -maxAngularVelocity, maxAngularVelocity); // Apply angular velocity with clamp

        //decrease fizziness based on launch force
        DecreaseFizziness(launchForce);
    }



    private void DecreaseFizziness(float launchForce)
    {
        //the more force applied, the more fizz the cola can loses
        float fizzLoss = Mathf.Clamp(launchForce * 0.1f, 0, maxFizziness); // Adjust the multiplier for desired fizz loss rate
        _currentFizziness -= fizzLoss;

        //clamp the fizziness to make sure it doesn't go below 0
        _currentFizziness = Mathf.Clamp(_currentFizziness, 0, maxFizziness);

        //if the fizziness reaches 0, trigger game over or other effects
        if (_currentFizziness <= 0)
        {
            Debug.Log("The cola can has gone flat! Game Over!");
            // TODO: Add text/UI for player "Death"
        }
    }

    //update the UI Slider with the current fizziness 
    private void UpdateFizzinessUI()
    {
        if (fizzinessSlider != null)
        {
            fizzinessSlider.value = _currentFizziness; //update the slider value
        }
    }

    public void IncreaseFizziness(float amount)
    {
        //increase fizziness by x amount
        _currentFizziness += amount;

        //clamps the fizziness so it doesnt go over the maximum value
        _currentFizziness = Mathf.Clamp(_currentFizziness, 0, maxFizziness);

        //updates the UI with the new value
        UpdateFizzinessUI();
    }
}
 