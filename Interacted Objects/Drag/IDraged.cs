using UnityEngine;

public interface IDraged
{
    public void Drag(Transform transform);
    public void Drop();

    public void Throw(Vector3 vector3);
}
