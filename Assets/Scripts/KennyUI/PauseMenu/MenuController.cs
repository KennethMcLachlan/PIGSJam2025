using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MenuController : MonoBehaviour
{
    //Player Positioning
    [SerializeField] private Transform _player;
    [SerializeField] private DynamicMoveProvider _playerMovement;
    private Vector3 _activePlayPosition;
    private Quaternion _activePlayRotation;
    [SerializeField] private Transform _pauseSpawnLocation;

    //Sanp and Smooth Turn Providers
    public SnapTurnProvider snapTurn;
    public ContinuousTurnProvider smoothTurn;

    public InputAction snapTurnAction;
    public InputAction smoothTurnAction;

    //Pause / Menu Button
    public InputActionReference pauseMenuAction;
    private bool _pauseMenuIsActive;

    //Fade In/Out Variables
    [SerializeField] private Image _fadeImage;
    private float _fadeDuration = 1f;

    private void Awake()
    {
        pauseMenuAction.action.Enable();
        pauseMenuAction.action.performed += ToggleMenu;

        StartCoroutine(GameStartFadeIn());
    }

    //Turns the Menu on/off
    private void ToggleMenu(InputAction.CallbackContext context)
    {
        _pauseMenuIsActive = !_pauseMenuIsActive;

        if (_pauseMenuIsActive == true)
        {
            //Enable Pause menu
            _activePlayPosition = _player.position;
            _activePlayRotation = _player.rotation;
            StartCoroutine(BeginPauseSequenceRoutine());

        }

        if (_pauseMenuIsActive == false)
        {
            //Disable Pause Menu and return player to gameplay
            StartCoroutine(EndPauseSequenceRoutine());
        }
    }

    private IEnumerator BeginPauseSequenceRoutine()
    {
        _pauseMenuIsActive = true;
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(1f);
        _player.position = _pauseSpawnLocation.position;
        _player.rotation = _pauseSpawnLocation.rotation;
        _playerMovement.moveSpeed = 0f; //Prevents player movement while in menu
        StartCoroutine(FadeOut());
    }

    private IEnumerator EndPauseSequenceRoutine()
    {
        _pauseMenuIsActive = false;
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(1f);
        _player.position = _activePlayPosition;
        _player.rotation = _activePlayRotation;
        _playerMovement.moveSpeed = 5f; //Enables player movement when game is resumed
        StartCoroutine(FadeOut());
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void ResumeGameFromButton()
    {
        StartCoroutine(EndPauseSequenceRoutine());
    }

    public void EnableSnapTurn()
    {
        //if (snapTurn != null)
        //{
        //snapTurn.enabled = true;
        //Debug.Log("Snap Turn has been enabled");
        //snapTurn.transformationPriority = 1;
        //}

        //if (smoothTurn != null)
        //{
        //    //smoothTurn.enabled = false;
        //    smoothTurn.transformationPriority = 0;
        //}

        smoothTurn.enabled = false;
        smoothTurnAction.Disable();

        snapTurn.enabled = true;
        snapTurnAction.Enable();

    }

    public void EnableSmoothTurn()
    {
        //if (smoothTurn != null)
        //{
        //    //smoothTurn.enabled = true;
        //    //Debug.Log("Smooth Turn has been enabled");
        //   // smoothTurn.transformationPriority = 1;
        //}

        //if (snapTurn != null)
        //{
        //    //snapTurn.enabled = false;
        //    snapTurn.transformationPriority = 0;
        //}

        snapTurn.enabled = false;
        snapTurnAction.Disable();


        smoothTurn.enabled = true;
        smoothTurnAction.Enable();

        
    }

    //Fade in at start of game
    private IEnumerator GameStartFadeIn()
    {
        _fadeImage.color = new Color(0, 0, 0, 1);
        yield return FadeOut();
    }

    // Fade from transparent to black
    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = _fadeImage.color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }
    }

    // Fade from black to transparent
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = _fadeImage.color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / _fadeDuration));
            _fadeImage.color = color;
            yield return null;
        }
    }
}
