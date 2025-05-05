using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs_Actions playerInputs;
    private Vector2 moveInput;
    public Chariot chariot;  // Référence au chariot

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

        if (chariot != null && moveInput.x < 0)
        {
            Debug.Log("GAUCHE");

            // Changer de rail si une spline "SplineLeft" existe
            if (chariot.leSplineContainerTab.Length > 1 && chariot.leSplineContainerTab[1].name == "SplineLeft")
            {
                chariot.currentSpline = chariot.leSplineContainerTab[1].Spline;
                chariot.t = 0f;  // Réinitialisation de la progression sur la nouvelle spline
                chariot.etatActuel = Chariot.Etat.Toutdroit;
            }
        }

        if (chariot != null && moveInput.x > 0)
        {
            Debug.Log("DROITE");

            // Changer de rail si une spline "SplineRight" existe
            if (chariot.leSplineContainerTab.Length > 1 && chariot.leSplineContainerTab[1].name == "SplineRight")
            {
                chariot.currentSpline = chariot.leSplineContainerTab[1].Spline;
                chariot.t = 0f;
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
