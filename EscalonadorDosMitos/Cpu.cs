

namespace EscalonadorDosMitos
{
    public class Cpu
    {
        private int computationTime = 0;
        private int quantum = 0;
        public Task? TaskInCpu {  get; set; }
        
        public bool Compute(Task task, bool hasQuantum, bool preemptive)
        {

            if (task != null)
            {
                if (computationTime <= 0 || quantum <= 0 || preemptive)
                {
                    task.ComputationTime = task.ComputationTime - 1;
                    computationTime = task.ComputationTime;
                    quantum = task.Quantum - 1;
                    TaskInCpu = task;
                }
                else
                {
                    task.ComputationTime = task.ComputationTime - 1;
                    computationTime -= 1;
                    quantum -= 1;
                }
            }

            if (!hasQuantum)
            {
                quantum = 2;
            }

            if (TaskInCpu != null)
            {
                Console.WriteLine("    Tarefa na CPU: " + (TaskInCpu.Index + 1));
            }

            return computationTime > 0 && quantum > 0;
        }

        
    }
}
