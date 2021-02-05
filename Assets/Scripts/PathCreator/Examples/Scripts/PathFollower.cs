using PathCreation;
using UnityEngine;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class PathFollower : MonoBehaviour {
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    public float addedLaunchSpeed;
    public float decAmountLaunch;
    void Start () {
        if (pathCreator != null) {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            addedLaunchSpeed = GameManager.Instance.addedLaunchSpeed;
            decAmountLaunch = GameManager.Instance.decAmountLaunch;
            speed = GameManager.Instance.GetSpeed ();
        }
    }

    void Update () {

        if (pathCreator != null) {
            if (CharacterController.Instance.boom) {
                if (addedLaunchSpeed > 0) {
                    distanceTravelled += (speed+addedLaunchSpeed) * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance (distanceTravelled, endOfPathInstruction);
                    transform.rotation = pathCreator.path.GetRotationAtDistance (distanceTravelled, endOfPathInstruction);
                    addedLaunchSpeed = addedLaunchSpeed - decAmountLaunch;
                } else {
                    CharacterController.Instance.boom = false;
                }
            } else {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance (distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance (distanceTravelled, endOfPathInstruction);
            }

        }

    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged () {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath (transform.position);
    }
}