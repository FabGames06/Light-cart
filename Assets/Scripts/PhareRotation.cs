using UnityEngine;

public class PhareRotation : MonoBehaviour
{

    private float rotate;
    void Start()
    {
        rotate = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotate);
        rotate += 100f * Time.deltaTime;

        if(rotate > 360f)
            rotate = 0f;
    }
}
