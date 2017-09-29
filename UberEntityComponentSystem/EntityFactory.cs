using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class EntityFactory : ObjectFactory<Entity>
    {
        public override Entity CleanForReuse(Entity obj)
        {
            obj.setPool(null);
            return obj;
        }

        public override Entity CreateNew()
        {
            return new Entity();
        }
    }
}
