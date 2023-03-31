using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] Vector2 stickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick Joystick;
    [SerializeField] PlayerLogic player;
    [SerializeField] Button abilityButton;
    public Vector2 scaledMovement;

    private Finger MovementFinger;
    public Vector2 MovementAmount;


    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = stickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;

            if (MovementAmount.x * MovementAmount.x > MovementAmount.y * MovementAmount.y)
            {
                MovementAmount.y = 0;
                MovementAmount.Normalize();
            }
            else if (MovementAmount.y * MovementAmount.y > MovementAmount.x * MovementAmount.x)
            {
                MovementAmount.x = 0;
                MovementAmount.Normalize();
            }

        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null /*&& TouchedFinger.screenPosition.x < Screen.width / 1.3f && TouchedFinger.screenPosition.y < Screen.height / 1.2f*/ //1st method
            && TouchedFinger.screenPosition.x < Screen.width * 0.8f //2nd method
            || TouchedFinger.screenPosition.y > Screen.height * 0.11f) //2nd method
        {           
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = stickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);            
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < stickSize.x / 2)
        {
            StartPosition.x = stickSize.x / 2;
        }

        if (StartPosition.y < stickSize.y / 2)
        {
            StartPosition.y = stickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - stickSize.y / 2)
        {
            StartPosition.y = Screen.height - stickSize.y / 2;
        }

        return StartPosition;
    }

    //public bool IsPointerOverUI(int fingerId)
    //{
    //    EventSystem eventSystem = EventSystem.current;
    //    return eventSystem.IsPointerOverGameObject(fingerId) && eventSystem.currentSelectedGameObject != null;
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    ;
    //}
}
