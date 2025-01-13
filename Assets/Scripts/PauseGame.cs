using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    [Header("Pausing")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] bool isPaused;

    PlayerMovment playerMovement;
    PlayerControls playerControls;

    #region Cutscene
    public bool SetCutscene(bool inCutscene)
    {
        if (inCutscene)
        {
            playerControls.Pause.Disable();
        }
        else
        {
            playerControls.Pause.Enable();
        }

        return inCutscene;
    }
    #endregion

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovment>();
        playerControls = new PlayerControls();

        playerControls.Pause.Enable();

        playerControls.Pause.Pausing.performed += Pausing;

        isPaused = false;
        pauseUI.SetActive(isPaused);
    }

    private void Pausing(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isPaused = !isPaused;
            pauseUI.SetActive(isPaused);
            if(isPaused)
            {
                Time.timeScale -= 1.0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale += 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            playerMovement.SetCutscene(isPaused);

            Debug.Log("IsPaused: " + isPaused);
        }
    }

    public void PauseButton()
    {
        isPaused = false;
        pauseUI.SetActive(isPaused);
        Time.timeScale += 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.SetCutscene(isPaused);
    }
}
