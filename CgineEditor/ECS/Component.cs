using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.ECS
{
    interface IMSComponent { }
    [DataContract]
    abstract class ComponentBase : ViewModelBase
    {
        public Entity Owner { get; private set; }

        public ComponentBase(Entity entity)
        {
            Debug.Assert(entity != null);
            Owner = entity;
        }

    }

    abstract class MSComponentBase<T> : ViewModelBase, IMSComponent where T : ComponentBase { }

}
