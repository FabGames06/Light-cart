using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs_Actions playerInputs;
    private Vector2 moveInput;
    public Chariot chariot;  // Référence au chariot
    //public GameObject effetTouch;  // Effet visuel au toucher

    void Awake()
    {
        playerInputs = new PlayerInputs_Actions();
    }

    void OnEnable()
    {
        playerInputs.Player.Enable();
        playerInputs.Player.Move.performed += Deraille;
        playerInputs.Player.Move.canceled += OnMoveCanceled;
    }

    public void Deraille(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        /*
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Instantiate(effetTouch, Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10f)), Quaternion.identity);
        }
        */

        // Gestion du mouvement gauche/droite
        if (chariot != null && moveInput.x < -0.5f) // Précision pour éviter les détections accidentelles
        {
            Debug.Log("GAUCHE");
            if (chariot.leSplineContainerTab.Length > 1 && chariot.leSplineContainerTab[1].name == "SplineLeft")
            {
                chariot.currentSpline = chariot.leSplineContainerTab[1].Spline;
                chariot.t = 0.2f;
                chariot.etatActuel = Chariot.Etat.Toutdroit;
            }
        }

        if (chariot != null && moveInput.x > 0.5f)
        {
            Debug.Log("DROITE");
            if (chariot.leSplineContainerTab.Length > 1 && chariot.leSplineContainerTab[1].name == "SplineRight")
            {
                chariot.currentSpline = chariot.leSplineContainerTab[1].Spline;
                chariot.t = 0.2f;
                chariot.etatActuel = Chariot.Etat.Toutdroit;
            }
        }
    }

    public void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        playerInputs.Player.Disable();
        playerInputs.Player.Move.performed -= Deraille;
        playerInputs.Player.Move.canceled -= OnMoveCanceled;
    }
}
