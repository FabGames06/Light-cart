using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs_Actions playerInputs;
    private Vector2 moveInput;
    void Awake()
    {
        playerInputs = new PlayerInputs_Actions();
    }

    void OnEnable()
    {
        playerInputs.Player.Enable();
        playerInputs.Player.Move.performed += Debuglogdir;  // ctx => Debuglogdir(ctx);
        playerInputs.Player.Move.canceled += OnMoveCanceled;  //ctx => moveInput = Vector2.zero;
    }

    public void Debuglogdir(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        if(moveInput.x<0)
        {
            Debug.Log("GAUCHE");

            for (int i = 0; i < Chariot.leSplineContainerTab.Length; i++)
                Debug.Log("leSplineContainerTab[" + i + "] = " + Chariot.leSplineContainerTab[i].name);

            if (Chariot.leSplineContainerTab[1].name == "SplineLeft")
                Chariot.deraille = true;
        }

        if (moveInput.x>0)
        {
            Debug.Log("DROITE");
            Debug.Log("currentSplineAnimate=" + Chariot.leSplineContainerTab);
            if (Chariot.leSplineContainerTab[1].name == "SplineRight")
                Chariot.deraille = true;
        }
    }

    public void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        playerInputs.Player.Disable();
        playerInputs.Player.Move.performed -= Debuglogdir;
        playerInputs.Player.Move.canceled -= OnMoveCanceled;  //ctx => moveInput = Vector2.zero;
    }

    void Update()
    {
        //Debug.Log($"Mouvement : {moveInput}");
    }
}
