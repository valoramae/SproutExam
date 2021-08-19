using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Interface
{
    interface IBaseDA <Entity,KeyType>
    {
        Entity Insert(Entity e);
        bool Update(Entity e);
        bool Delete(Entity e);
        Entity Get(Entity e);
    }
}
