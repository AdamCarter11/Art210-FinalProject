using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] Camera mainCam;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, mainCam.transform.rotation.eulerAngles.y, 0f);
    }
}
 