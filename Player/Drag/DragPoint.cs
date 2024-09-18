using UnityEngine;

public class DragPoint : MonoBehaviour
{
    private void Start()
    {
        Vector3 vector3 = Vector3.zero;
        vector3.z = Player.InteractRange;

        transform.localPosition = vector3;
    }
}
