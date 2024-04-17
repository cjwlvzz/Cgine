using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.ECS
{
    [DataContract]
    public class Component : ViewModelBase
    {
        public Entity Owner { get; private set; }

        public Component(Entity entity)
        {
            Debug.Assert(entity != null);
            Owner = entity;
        }

    }
}
