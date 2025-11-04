using UnityEngine;

public class DoubleDoorOpener : MonoBehaviour
{
    public Transform leftDoor;   // leftt
    public Transform rightDoor;  // right
    public float rotationAngle = 110f; // Rotation angle for each door
    public float rotationSpeed = 2f;   // Speed of rotation
    public float activationDistance = 3f; //distance

    private Quaternion leftDoorInitialRotation;
    private Quaternion rightDoorInitialRotation;
    private Quaternion leftDoorTargetRotation;
    private Quaternion rightDoorTargetRotation;
    private bool isOpening = false;

    void Start()
    {
        leftDoorInitialRotation = leftDoor.rotation;
        rightDoorInitialRotation = rightDoor.rotation;

        leftDoorTargetRotation = Quaternion.Euler(leftDoor.eulerAngles.x, leftDoor.eulerAngles.y - rotationAngle, leftDoor.eulerAngles.z);
        rightDoorTargetRotation = Quaternion.Euler(rightDoor.eulerAngles.x, rightDoor.eulerAngles.y + rotationAngle, rightDoor.eulerAngles.z);
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = GetClosestPlayer(players);

        if (closestPlayer)
        {
            float distance = Vector3.Distance(transform.position, closestPlayer.transform.position);
            isOpening = distance < activationDistance;
        }
        else
        {
            isOpening = false;
        }

        // Smoothly rotate both doors in opposite directions
        leftDoor.rotation = Quaternion.Lerp(leftDoor.rotation, isOpening ? leftDoorTargetRotation : leftDoorInitialRotation, Time.deltaTime * rotationSpeed);
        rightDoor.rotation = Quaternion.Lerp(rightDoor.rotation, isOpening ? rightDoorTargetRotation : rightDoorInitialRotation, Time.deltaTime * rotationSpeed);
    }

    GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        return closest;
    }
}
