using UnityEngine.Splines;
using UnityEngine;

public class RotateChariot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Quaternion rotationInitiale = collision.transform.rotation;
        Quaternion rotationFinale;

        switch (collision.name)
        {
            case "S_NE":
            case "S_SO":
                rotationFinale = Quaternion.Euler(0, 0, -45);
            break;

            case "S_horizontal":
                SplineContainer splineCont = collision.gameObject.GetComponent<SplineAnimate>().Container;
                if(splineCont.name=="SplineRight")
                    rotationFinale = Quaternion.Euler(0, 0, -90);
                else
                    rotationFinale = Quaternion.Euler(0, 0, 90);
            break;
                /*
            case "S_NO":
            case "S_SE":
                rotationFinale = Quaternion.Euler(0, 0, 45);
            break;
                */
            default:
            case "S_vertical":
                rotationFinale = Quaternion.Euler(0, 0, 0);
            break;
        }

        collision.transform.rotation = rotationFinale;  // Quaternion.Lerp(rotationInitiale, rotationFinale, Time.deltaTime);

    }
}
