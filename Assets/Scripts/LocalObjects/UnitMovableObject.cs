//using UnityEngine;

//public class UnitMovableObject : MovableObject
//{

//    public LayerMask mask;
//    private Vector3 direction;

//    private void Update()
//    {
//        Vector3 eulerAngles = new Vector3(-direction.z, 0, direction.x) * 10f;
//        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -70f, 70f);
//        eulerAngles.y = Mathf.Clamp(eulerAngles.y, -70f, 70f);

//        transform.eulerAngles = eulerAngles;

//        direction = Vector3.Lerp(direction, Vector3.zero, Mathf.Clamp(direction.magnitude / 0.5f, 1f, float.MaxValue) * Time.deltaTime);
//    }

//    public override void Get()
//    {
//        transform.position = GetRayPosition() + Vector3.up * 0.5f;
//    }

//    public override void Move(Vector3 movement)
//    {
//        transform.position += movement;
//        direction += movement;

//        transform.position = GetRayPosition() + Vector3.up * 0.5f;
//    }

//    public override void Put()
//    {
//        transform.position = GetRayPosition();
//    }

//    private Vector3 GetRayPosition()
//    {
//        if (Physics.Raycast(transform.position + Vector3.up * 100f, Vector3.down, out RaycastHit hit, 1000f, LayerMask.GetMask("Cell")))
//        {
//            return hit.point;
//        }
//        return transform.position;
//    }

//}
