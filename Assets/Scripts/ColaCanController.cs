using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class ColaCanController : MonoBehaviour
{
    private Vector3 _startMousePosition;
    private Vector3 _currentMousePosition;
    private Rigidbody2D _rigidbody2D;
    private bool _isDragging = false;
    private bool _isLaunching = false; // tracks if the player is in a 'slam state'
    private float _currentFizziness; //current fizziness value

    //private Vector3 _originalScale;

    [SerializeField] private float launchForceMultiplier = 5f; //adjust the strenght of lauch
    [SerializeField] private float spinForceMultiplier = 200f; //controls how fast the can can spin
    [SerializeField] private float maxAngularVelocity = 100f; //maximum rotational spin
    [SerializeField] private float maxFizziness = 100f; //max value of fizziness (health)
    [SerializeField] private Slider fizzinessSlider; //referencing the UI slider

    [SerializeField] private Vector3 enlargedScale = new Vector3(2f, 2f, 1f); //scale of the player during the slam
    [SerializeField] private float slamRadius = 2f; //Radius of slam effect
    [SerializeField] private LayerMask enemyLayer; //Layer for enemies to detect during slam
    [SerializeField] private float slamDamage = 10f; //damage dealt by slam
    [SerializeField] private float slamSpeedThreshold = 0.1f; //speed needed to exit slam
    [SerializeField] private float sizeChangeSpeed = 8f;
    [SerializeField] private float slamForce = 5f;

    [SerializeField] private ParticleSystem slamParticles;

    [SerializeField] private float damageForce = 5f;

    [SerializeField] private float minLaunchSpeed = 1f;

    // if an enemies damage would put you below this amount of health, you stay on it
    // if you are alreay at or below the amount, however, they enemy can kill you
    [SerializeField] private float oneShotProtection;

    [SerializeField] private LineRenderer aimIndicator;
    [SerializeField] private Color lineLaunchColor;
    [SerializeField] private Color lineDisabledColor;
    [SerializeField] private float lineScale;

    private bool _isDead = false;

    public RectTransform deathScreen;
    public GameObject pauseScreen;

    public Timer timer;

    [SerializeField] private ParticleSystem launchParticles;

    public ScreenShake.Amount damageScreenShake;
    public ScreenShake.Amount slamScreenShake;
    public ScreenShake screenShake;

    public RandomAudioSource damageAudio;
    public RandomAudioSource gameOverAudio;
    public RandomAudioSource launchAudio;
    public RandomAudioSource slamAudio;
    public RandomAudioSource enemyHitAudio;
    public RandomAudioSource slurpAudio;

    private void Awake()
    {
        SetTimeScale(1f);

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
        if (_isDead) {
            deathScreen.anchoredPosition = new Vector2(0, Mathf.MoveTowards(deathScreen.anchoredPosition.y, 0, deathScreen.anchoredPosition.y*8*Time.unscaledDeltaTime));
            SetTimeScale(Time.timeScale - Time.timeScale*Time.unscaledDeltaTime);
        } else {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
                TogglePause();
            }
        }

        HandleMouseInput();
        UpdateFizzinessUI(); //update the UI slider with the current fizziness

        _rigidbody2D.angularVelocity = Mathf.Clamp(_rigidbody2D.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
        UpdateSlam();
    }

    private void HandleMouseInput()
    {
        if (_isDead) {
            aimIndicator.gameObject.SetActive(false);
            _isDragging = false;
            return;
        }

        if (Time.timeScale < 1f) {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //takes the location of where the mouse clicked before launch/pull
            _startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startMousePosition.z = 0; //sets z to 0 since we're in 2D
            _isDragging = true;
            aimIndicator.gameObject.SetActive(true);
        }

        if (_isDragging) {
            _currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentMousePosition.z = 0;

            Vector3 offset = Quaternion.Inverse(transform.rotation)*(_currentMousePosition-_startMousePosition)*lineScale;
            Vector3 padding = offset.normalized*0.25f;
            aimIndicator.SetPosition(0, padding+offset);
            aimIndicator.SetPosition(1, padding);

        
            Color color;
            if (_currentFizziness <= 0 || Vector2.Distance(_startMousePosition, _currentMousePosition) * launchForceMultiplier <= minLaunchSpeed) {
                color = lineDisabledColor;
            } else {
                color = lineLaunchColor;
            }
            aimIndicator.endColor = color;
            color.a = 0;
            aimIndicator.startColor = color;
        }

        if (_isDragging && !Input.GetMouseButton(0))
        {
            aimIndicator.gameObject.SetActive(false);
            LaunchCan();
            _isDragging = false;
        }
    }

    private void LaunchCan()
    {
        if (_isLaunching || _currentFizziness <= 0) {
            return;
        }

        //calculate the direction and force of the launch
        Vector2 launchDirection = (_startMousePosition - _currentMousePosition).normalized;
        float launchForce = Vector2.Distance(_startMousePosition, _currentMousePosition) * launchForceMultiplier;

        //apply the force to the rigidbody on the cola can
        _rigidbody2D.velocity = launchDirection * launchForce;

        //apply spin (angular velocity)
        float randomSpin = Random.Range(-1f, 1f); // Random spin direction
        _rigidbody2D.angularVelocity = randomSpin * spinForceMultiplier; // Calculate angular velocity

        //decrease fizziness based on launch force
        DecreaseFizziness(launchForce);

        if (launchForce > minLaunchSpeed) {
            _isLaunching = true;
            gameObject.layer = 6;
            launchParticles.Play();
        }

        launchAudio.Play();
    }

    private void UpdateSlam() {
        Transform image = transform.GetChild(0);
        Vector3 targetScale = _isLaunching ? enlargedScale : Vector3.one;

        image.localScale = Vector3.MoveTowards(image.localScale, targetScale, Vector3.Distance(image.localScale, targetScale)*Time.deltaTime*sizeChangeSpeed);

        if (!_isLaunching) {
            if (image.localScale.x < 1.1f) {
                gameObject.layer = 0;
                launchParticles.Stop();
            }
            return;
        }

        if (_rigidbody2D.velocity.magnitude < slamSpeedThreshold) {
            SlamDamage();
            slamParticles.transform.position = transform.position;
            slamParticles.Play();
            screenShake.ShakeScreen(slamScreenShake);
            slamAudio.Play();
            _isLaunching = false;
        }
    }

    private void SlamDamage() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, slamRadius, Vector2.zero, 0, enemyLayer);
        foreach (RaycastHit2D hit in hits) {
            Vector2 force = (hit.transform.position-transform.position).normalized * slamForce;

            hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(slamDamage);
        }

        if (hits.Length > 0) {
            enemyHitAudio.Play();
        }
    }

    private void DecreaseFizziness(float launchForce)
    {
        //the more force applied, the more fizz the cola can loses
        float fizzLoss = Mathf.Clamp(launchForce * 0.5f, 0, maxFizziness); // adjust the multiplier for desired fizz loss rate
        _currentFizziness -= fizzLoss;

        //clamp the fizziness to make sure it doesn't go below 0
        _currentFizziness = Mathf.Clamp(_currentFizziness, 0, maxFizziness);
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
        if (_isDead) {
            return;
        }

        //increase fizziness by x amount
        _currentFizziness += amount;

        //clamps the fizziness so it doesnt go over the maximum value
        _currentFizziness = Mathf.Clamp(_currentFizziness, 0, maxFizziness);

        //updates the UI with the new value
        UpdateFizzinessUI();

        slurpAudio.Play();
    }

    public void DamagePlayer(float amount, Transform source) {
        //if the fizziness reaches 0, trigger game over or other effects
        if (_currentFizziness <= oneShotProtection) {
            _currentFizziness = 0;
            _isDead = true;
            timer.StopTimer();
            deathScreen.gameObject.SetActive(true);
            gameOverAudio.Play();
        } else {
            damageAudio.Play();
            _currentFizziness -= amount;

            //clamp the fizziness to make sure it doesn't go below 0
            _currentFizziness = Mathf.Clamp(_currentFizziness, oneShotProtection, maxFizziness);
        }

        Vector2 force = (transform.position - source.position).normalized * damageForce;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        source.GetComponent<Rigidbody2D>().AddForce(-force, ForceMode2D.Impulse);
        screenShake.ShakeScreen(damageScreenShake);
        
    }

    public bool IsLaunching() {
        return _isLaunching;
    }

    public void TogglePause() {
        pauseScreen.SetActive(!pauseScreen.activeSelf);

        _isDragging = false;
        aimIndicator.gameObject.SetActive(false);

        SetTimeScale(pauseScreen.activeSelf ? 0f: 1f);
    }

    public void Restart() {
        SetTimeScale(1f);
        SceneManager.LoadScene("MainGame");
    }

    public void Quit() {
        SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }

    public void SetTimeScale(float timeScale) {
        if (timeScale < 0.01f) {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0.02f;
        } else {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02f*timeScale;
        }
    }
}
 