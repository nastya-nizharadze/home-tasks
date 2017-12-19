namespace Scan
{
    class RegisterManager
    {
        private Register[] registers;
        private readonly bool[,] q_handshakes;

        public RegisterManager(Register[] regs)
        {
            this.registers = regs;
            this.q_handshakes = new bool[registers.Length, registers.Length];
        }

        public int[] Scan(int ind = 0)
        {
            var moved = new int[registers.Length];
            for (int mov = 0; mov < registers.Length; mov++) moved[mov] = 0;
            while (true)
            {
                for (var j = 0; j < registers.Length; j++) q_handshakes[ind, j] = registers[j].GetBitmask()[ind];
                
                var a = Collect();
                var b = Collect();

                var result = true;
                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k].GetBitmask()[ind] == b[k].GetBitmask()[ind] && b[k].GetBitmask()[ind] == q_handshakes[ind, k] && a[k].GetToggle() == b[k].GetToggle()) continue;
                    if (moved[k] == 1) return b[k].GetView();
                    moved[k] = 1;
                    result = false;
                    break;
                }

                if (!result) continue;

                var view = new int[registers.Length];
                for (var i = 0; i < registers.Length; i++)
                    view[i] = a[i].GetData();
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
                newBitmask, !registers[i].GetToggle(), view);
        }

        private Register[] Collect()
        {
            return (Register[])registers.Clone();
        }

    }



}