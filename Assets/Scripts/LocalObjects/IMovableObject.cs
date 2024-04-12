using UnityEngine;

public interface IMovableObject
{

    //private Vector3 _oldRayPosition;
    //private Camera _camera;

    public void Take();
    public void Move(Vector3 delta);
    public void Put(Cell cell, Vector3 poition);

 //   private void Start()
 //   {
 //       _camera = Camera.main;
 //   }

 //   private void OnMouseDown()
 //   {
 //       _oldRayPosition = GetRayPosition();
 //       Get();
 //   }

 //   private void OnMouseDrag()
 //   {
 //       Vector3 rayPosition = GetRayPosition();
 //       Vector3 direction = rayPosition - _oldRayPosition;
 //       _oldRayPosition = rayPosition;

 //       Move(direction);
 //   }

 //   private void OnMouseUp()
 //   {
 //       Put();
	//}

 //   private Vector3 GetRayPosition()
 //   {
 //       Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
 //       float distance = 0;
 //       Plane hPlane = new Plane(Vector3.up, Vector3.zero);
 //       if (hPlane.Raycast(ray, out distance))
 //       {
 //           return ray.GetPoint(distance);
 //       }
 //       return Vector3.zero;
 //   }

}
