using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public float smoothSpeed = 100;
    public Vector3 offset;
    public Vector3 closeUp;
    public static bool isCloseUp = false;

    void Update () {
            if (!isCloseUp) {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;

            } else {
                Vector3 desiredPosition = target.position + closeUp;
                Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition,smoothSpeed);
                transform.position = smoothedPosition;
            }
            transform.eulerAngles = new Vector3(0, target.transform.eulerAngles.y, 0);
    }

}