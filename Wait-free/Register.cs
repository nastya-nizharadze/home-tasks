using System;
using System.Threading.Tasks;

namespace Scan
{

    public class Register
    {
        static Object lockobj = new Object();

        public int id;
        public int data;
        public bool[] bitmask;
        public bool toggle;
        public int[] view;

        public void AtomicUpdate(int newData, bool[] newBitmask, bool newToggle, int[] snapshot)
        {
            lock (lockobj)
            {
                this.bitmask = newBitmask;
                this.toggle = newToggle;
                this.view = snapshot;
                this.data = newData;
                Console.WriteLine("Task No {0} updated his register No {2} with data = {1}\n", Task.CurrentId, data, id);
            }
        }
    }
}
