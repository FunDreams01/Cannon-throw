using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float Speed;
    public Vector3 offsetStart;
    public Vector3 offetCloseup;

    public Vector3 offset;
    public GameObject target;
    public string state;
    private static CameraManager _instance;
    public static CameraManager Instance {

        get {
            if (_instance == null) {
                _instance = FindObjectOfType<CameraManager> ();
                if (_instance == null) {
                    GameObject go = new GameObject ();
                    go.name = typeof (CameraManager).Name;
                    _instance = go.AddComponent<CameraManager> ();
                }
            }
            return _instance;
        }
    }

    private void Awake () {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy (gameObject);
        }
    }

    private void Start () {
        offset = offsetStart;
        state = "far";
    }
    void Update () {
        transform.position = target.transform.position + offset;
        if (state == "farToClose") {
            offset = Vector3.Lerp (offset, offetCloseup, 0.1f);
            if(offset==offetCloseup){
                state="close";
            }
        } else if (state == "closeToFar") {
             offset = Vector3.Lerp (offset, offsetStart, 0.1f);
              if(offset==offsetStart){
                state="far";
            }
        }
    }

    public void changeCam(string s){
        state=s;
    }

    public string GetCamState(){
        return state;
    }
}