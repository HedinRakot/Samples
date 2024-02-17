using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApi.Domain
{
    public class VirtualsInClass
    {
        public virtual int Speed()
        {
            return 0;
        }
    }


    public class Car : VirtualsInClass
    {
        public override int Speed()
        {
            return 1;
        }
    }

    public class Bike : VirtualsInClass
    {
        public override int Speed()
        {
            return 2;
        }
    }
}
