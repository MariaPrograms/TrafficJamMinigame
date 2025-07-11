using System.Linq;
using UnityEngine;

public class CpuCarController : BaseCarController
{
    public Vector2 idleTime = new Vector2(1, 3);
    public Vector2 moveTime = new Vector2(5, 10);

    public enum States
    {
        Idle,
        Moving
    }

    public override void CarUpdate(CarManager manager)
    {
        if (MoneyManager.moneyInPlay.Count == 0)
        {
            //if there's no money current spawned in just having them spin in place 
            Vector3 forward = manager.transform.position + new Vector3(45, 0, 0);
            manager.transform.rotation = Quaternion.Lerp(manager.transform.rotation, Quaternion.LookRotation(forward, Vector3.up), Time.deltaTime);
            manager.Steer(Movement.Stop);
            return;
        }

        //Otherwise getting the closest one
        Vector3 endPos = MoneyManager.moneyInPlay.OrderBy(go => Vector3.Distance(go.transform.position, manager.transform.position)).ElementAt(0).transform.position;
        Collider[] colliders = Physics.OverlapSphere(manager.transform.position, 10f); //Area for Checking for traffic

        //Check if in the area bubble there are any NPC cars near by
        if (colliders.Any(a => a.tag == "NPC"))
        {
            manager.Steer(Movement.Backward); //Going backwards if so to get out of the way 
        }
        else
        {
            //Otherwise rotating to the closes money and moving forward
            Vector3 forward = endPos - manager.transform.position;
            manager.transform.rotation = Quaternion.Lerp(manager.transform.rotation, Quaternion.LookRotation(forward, Vector3.up), .1f);

            float distance = Vector3.Distance(manager.Rigidbody.position, endPos);
            manager.Steer(Movement.Forward);
        }
    }
}
