namespace Scan
{
    class Scanning
    {
        private Register[] registers;
        private readonly bool[,] q_handshakes;

        public Scanning(Register[] regs)
        {
            this.registers = regs;
            this.q_handshakes = new bool[registers.Length, registers.Length]; // init matrix of bytes
        }

        public int[] Scan(int ind = 0)  //BEGIN SCAN
        {
            var moved = new int[registers.Length]; 
            for (int mov = 0; mov < registers.Length; mov++) moved[mov] = 0; // init moves
            while (true)
            {
                for (var j = 0; j < registers.Length; j++) q_handshakes[ind, j] = registers[j].bitmask[ind];
                var a = Collect(); // collecting data, bitmask, toggle, view
                var b = Collect(); // collecting data, bitmask, toggle, view
                var result = true;
                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k].bitmask[ind] == b[k].bitmask[ind] && b[k].bitmask[ind] == q_handshakes[ind, k] && a[k].toggle == b[k].toggle) continue; // Nobody moved 
                    if (moved[k] == 1) return b[k].view; //k moved
                    moved[k] = 1;
                    result = false;
                    break;
                }

                if (!result) continue;

                var view = new int[registers.Length];
                for (var i = 0; i < registers.Length; i++)
                    view[i] = a[i].data;
                return view;
            }
        }

        public void Update(int i, int value)
        {
            var newBitmask = new bool[registers.Length];
            for (var j = 0; j < registers.Length; j++)
                newBitmask[j] = !q_handshakes[j, i];
            var view = Scan(i);

            registers[i].AtomicUpdate(value,
                newBitmask, !registers[i].toggle, view);
        }

        private Register[] Collect()
        {
            return (Register[])registers.Clone();
        }
    }
}
