using System.Collections.Generic;

public class BaseCarController
{
    public virtual void Initialize(CarManager manager) { }

    //Called on update in the manager
    public virtual void CarUpdate(CarManager manager) { }

}
