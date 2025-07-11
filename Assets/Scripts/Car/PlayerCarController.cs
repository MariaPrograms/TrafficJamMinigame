using UnityEngine;

public class PlayerCarController : BaseCarController
{
    Camera mainCam;

    public override void Initialize(CarManager manager)
    {
        mainCam = GameManager.Instance.GameplayCamera;
    }

    public override void CarUpdate(CarManager manager)
    {
        Vector3 mousePos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(mousePos); //Raycasting the mouse to the game to see where the user is clicking
            Vector3 endPos = manager.Rigidbody.position; //Setting as the current position in case something errors out

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: Mathf.Infinity, ~6))
            {
                endPos = new Vector3(hitInfo.point.x, endPos.y, hitInfo.point.z); //Updating the x and z position to where the mouse is 
            }

            Vector3 forward = endPos - manager.transform.position;
            manager.transform.rotation = Quaternion.LookRotation(forward, Vector3.up); //Setting the rotation to the mouse position
            manager.Steer(Movement.Forward); //Move the character forward
        }
        else
        {
            manager.SlowDown(); //Smoothing the stop 
        }
    }
}
