

namespace EscalonadorDosMitos
{
    public class Cpu
    {
        private int tempoDeComputacao = 0;
        private int quantum = 0;
        public Task? tarefaNaCpu {  get; set; }
        
        public bool Computar(Task task, bool possuiQuantum, bool preemptivo)
        {

            if (task != null)
            {
                if (tempoDeComputacao <= 0 || quantum <= 0 || preemptivo)
                {
                    task.ComputationTime = task.ComputationTime - 1;
                    tempoDeComputacao = task.ComputationTime;
                    quantum = task.Quantum - 1;
                    tarefaNaCpu = task;
                }
                else
                {
                    task.ComputationTime = task.ComputationTime - 1;
                    tempoDeComputacao -= 1;
                    quantum -= 1;
                }
            }

            if (!possuiQuantum)
            {
                quantum = 2;
            }

            if (tarefaNaCpu != null)
            {
                Console.WriteLine("    Tarefa na CPU: " + (tarefaNaCpu.Index + 1));
            }

            return tempoDeComputacao > 0 && quantum > 0;
        }

        
    }
}
