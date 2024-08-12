

using Newtonsoft.Json;

namespace EscalonadorDosMitos
{
    public class Task
    {
        
        public int Offset { get; set; }

        
        public int ComputationTime { get; set; }

        
        public int PeriodTime { get; set; }

        
        public int Quantum { get; set; }

        
        public int Deadline { get; set; }

        
        public int Index { get; set; }

        
        public int RelativeDeadline { get; set; }

        public Task(int offset, int computationTime, int periodTime, int quantum, int deadline, int index)
        {
            Offset = offset;
            ComputationTime = computationTime;
            PeriodTime = periodTime;
            Quantum = quantum;
            Deadline = deadline;
            Index = index;
            RelativeDeadline = deadline;
        }
        

        public Task CloneTask(Task taskToBeCloned)
        {
            Task task = new Task(taskToBeCloned.Offset, taskToBeCloned.ComputationTime, taskToBeCloned.PeriodTime, taskToBeCloned.Quantum, taskToBeCloned.Deadline, taskToBeCloned.Index);
            return  task;
        }

        public override string ToString()
        {
            return "Tarefa\n" +
                    "Offset = " + Offset + "\n" +
                    "ComputationTime = " + ComputationTime + "\n" +
                    "PeriodTime = " + PeriodTime + "\n" +
                    "Quantum = " + Quantum + "\n" +
                    "Deadline = " + Deadline;
        }
    }
}
