

namespace EscalonadorDosMitos
{
    public class Simulation
    {
        private Cpu cpu;
        private int cpuTime;
        private int[] waitTime;
        private int[] timeInCpu;
        private int[] activations;
        private int[] lostDeadlines;

        public Simulation()
        {
            cpu = new Cpu();
        }

        private void SimulationContext(List<Task> tasks)
        {
            cpuTime = 0;
            waitTime = new int[tasks.Count()];
            timeInCpu = new int[tasks.Count()];
            activations = new int[tasks.Count()];
            lostDeadlines = new int[tasks.Count()];

            for (int i = 0; i < tasks.Count(); i++)
            {
                waitTime[i] = 0;
                timeInCpu[i] = 0;
                activations[i] = 0;
                lostDeadlines[i] = 0;
            }
        }






        private void PrintResults(List<int> index, string type, float time)
        {
            if (index.Count() == 1)
            {
                Console.WriteLine("A Tarefa " + (index.ElementAt(0) + 1) + " teve o " + type + " tempo de espera: " + time);
            }

            else
            {
                Console.WriteLine("As Tarefas ");
                for (int i = 0; i < index.Count(); i++)
                {
                    if (i > 0)
                    {
                        if (i == index.Count() - 1)
                        {
                            Console.WriteLine(" e ");
                        }

                        else
                        {
                            Console.WriteLine(", "); 
                        }
                    }
                    Console.WriteLine(index.ElementAt(0) + 1);
                }

                Console.WriteLine(" tiveram o " + type + " tempo de espera: " + time);
            }
        }






        private void InformationLog(SchedulerSpecs specs, List<Task> tasks)
        {
            Console.WriteLine(" ");

            float usage = 0;

            for (int i = 0; i < specs.TasksNumber; i++)
            {
                usage += (float)(timeInCpu[i]) / specs.SimulationTime;
            }

            usage *= 100;

            float productivity = (float)specs.TasksNumber / specs.SimulationTime;

            float averageWaitTime, watingTimeSum = 0;
            float averageTurnaroundTime, TurnaroundTimeSum = 0;

            List<int> longerIndexes = new List<int>();
            List<int> shorterIndexes = new List<int>();
            float longestWaitTime = float.MinValue, shortestWaitTime = float.MaxValue;

            for (int i = 0; i < specs.TasksNumber; i++)
            {
                watingTimeSum += (float)waitTime[i] / activations[i];
                TurnaroundTimeSum += (float)(timeInCpu[i] + waitTime[i]) / activations[i];

                Console.WriteLine("Ativacoes para Tarefa " + (i + 1) + ": " + activations[i]);
                Console.WriteLine("Tempo de Espera para Tarefa " + (i + 1) + ": " + (float)waitTime[i] / activations[i]);
                Console.WriteLine("Turnaround para Tarefa " + (i + 1) + ": " + (timeInCpu[i] + waitTime[i]) / activations[i]);

                if (timeInCpu[i] + waitTime[i] >= specs.SimulationTime)
                {
                    Console.WriteLine("Prazo Perdido para Tarefa " + (i + 1));
                }

                if (lostDeadlines[i] != 0)
                {
                    Console.WriteLine("Prazo Perdido para Tarefa " + (i + 1));
                    Console.WriteLine("Proporcao de Prazos Perdidos: " + ((float)lostDeadlines[i] / activations[i]));
                }
                Console.WriteLine("-------------------------------------");

                float currentWaitTime = (float)waitTime[i] / activations[i];

                if (currentWaitTime > longestWaitTime)
                {
                    longestWaitTime = currentWaitTime;
                    longerIndexes.Clear();
                    longerIndexes.Add(i);
                }
                else if (currentWaitTime == longestWaitTime)
                {
                    longerIndexes.Add(i);
                }

                if (currentWaitTime < shortestWaitTime)
                {
                    shortestWaitTime = currentWaitTime;
                    shorterIndexes.Clear();
                    shorterIndexes.Add(i);
                }
                else if (currentWaitTime == shortestWaitTime)
                {
                    shorterIndexes.Add(i);
                }
            }

            PrintResults(longerIndexes, "maior", longestWaitTime);
            PrintResults(shorterIndexes, "menor", shortestWaitTime);

            averageWaitTime = watingTimeSum / tasks.Count();
            averageTurnaroundTime = TurnaroundTimeSum / tasks.Count();

            Console.WriteLine("Utilizacao: " + usage);
            Console.WriteLine("Produtividade: " + productivity);
            Console.WriteLine("Tempo de Espera Medio: " + averageWaitTime);
            Console.WriteLine("Tempo de Turnaround Medio: " + averageTurnaroundTime);
        }







        private void AddNonPreemptiveToList(List<Task> tasks, List<Task> readyQueue, int i)
        {
            foreach (Task task in tasks)
            {
                if (i == task.Offset || ((i - task.Offset) % task.PeriodTime) == 0)
                {
                    readyQueue.Add(task.CloneTask(task));
                    activations[task.Index]++;
                }
            }

            Console.WriteLine("Tempo: " + i);
        }

        private void AddPreemptiveToList(List<Task> readyQueue, int i, Task task)
        {
            if (i >= task.Offset && (i - task.Offset) % task.PeriodTime == 0)
            {
                if (readyQueue.Count == 0)
                {
                    readyQueue.Add(task.CloneTask(task));
                    activations[task.Index]++;
                }

                else 
                {
                    int InsertionIndex = 0;

                    for (int j = 0; j < readyQueue.Count(); j++)
                    {
                        if (task.PeriodTime < readyQueue.ElementAt(j).PeriodTime)
                        {
                            InsertionIndex = j;
                            break;
                        }
                        else 
                        {
                            InsertionIndex = j + 1;
                        }
                    }
                    readyQueue.Insert(InsertionIndex, task.CloneTask(task));
                    activations[task.Index]++;
                }
            }
        }









        private bool Check(List<Task> readyQueue, bool isComputing, bool hasQuantum, bool preemptive, bool hasDeadLine, int time)
        {
            if (readyQueue.Count != 0 || cpu.TaskInCpu != null)
            {
                if (!isComputing || preemptive)
                {
                    if (cpu.TaskInCpu != null && cpu.TaskInCpu.ComputationTime != 0)
                    {
                        readyQueue.Add(cpu.TaskInCpu);
                        cpu.TaskInCpu = null;
                    }

                    if (readyQueue.Count != 0)
                    {
                        isComputing = cpu.Compute(readyQueue.ElementAt(0), hasQuantum, preemptive);
                        readyQueue.RemoveAt(0);
                    }

                    else
                    {
                        isComputing = cpu.Compute(cpu.TaskInCpu, hasQuantum, preemptive);
                    }
                }
                else
                {
                    isComputing = cpu.Compute(cpu.TaskInCpu, hasQuantum, false);
                }
            }

            if (cpu.TaskInCpu != null)
            {
                timeInCpu[cpu.TaskInCpu.Index]++;

                cpu.TaskInCpu.RelativeDeadline = cpu.TaskInCpu.RelativeDeadline - 1;

                if (cpu.TaskInCpu.RelativeDeadline.Equals(0) && hasDeadLine)
                {
                    lostDeadlines[cpu.TaskInCpu.Index]++;
                    Console.WriteLine("Prazo perdido: " + (cpu.TaskInCpu.Index + 1));
                }

                if (cpu.TaskInCpu.ComputationTime == 0)
                {
                    cpu.TaskInCpu = null;
                }
            }

            else
            {
                Console.WriteLine("    Tarefa na CPU: null");
            }

            foreach (Task task in readyQueue)
            {
                waitTime[task.Index]++;

                task.RelativeDeadline = task.RelativeDeadline - 1;

                if (task.RelativeDeadline == 0 && hasDeadLine)
                {
                    lostDeadlines[task.Index]++;
                    Console.WriteLine("Prazo perdido: " + (task.Index + 1));
                }
            }

            return isComputing;
        }










        private void FCFS(SchedulerSpecs specs, int simulationTime, bool isComputing, List<Task> tasks, List<Task> readyQueue)
        {
            for (int i = 0; i < simulationTime; i++)
            {
                AddNonPreemptiveToList(tasks, readyQueue, i);
                isComputing = Check(readyQueue, isComputing, false, false, false, i);
            }

            InformationLog(specs, tasks);
        }

        private void RoundRobin(SchedulerSpecs specs, int simulationTime, bool isComputing, List<Task> tasks, List<Task> readyQueue)
        {
            for (int i = 0; i < simulationTime; i++)
            {
                AddNonPreemptiveToList(tasks, readyQueue, i);
                isComputing = Check(readyQueue, isComputing, true, false, false, i);
            }

            InformationLog(specs, tasks);
        }

        private void RateMonotonic(SchedulerSpecs specs, int simulationTime, bool isComputing, List<Task> tasks, List<Task> readyQueue)
        {
            bool preemptive = default;
            float systemUsage = 0;

            foreach (Task task in tasks)
            {
                systemUsage += ((float)task.ComputationTime / task.PeriodTime);
            }

            if (systemUsage > (specs.TasksNumber * (Math.Pow(2, ((double)1 / specs.TasksNumber) - 1))))
            {
                Console.WriteLine("Não programável");
                return;
            }

            for (int i = 0; i < simulationTime; i++)
            {
                foreach (Task tarefa in tasks)
                {
                    AddPreemptiveToList(readyQueue, i, tarefa);
                }

                if(readyQueue.Count != 0 && cpu.TaskInCpu != null && readyQueue.ElementAt(0).Deadline < cpu.TaskInCpu.Deadline) 
                {
                    preemptive = true;
                }
                

                Console.WriteLine("Tempo: " + i);
                isComputing = Check(readyQueue, isComputing, false, preemptive, true, i);
            }

            InformationLog(specs, tasks);
        }

        private void EDF(SchedulerSpecs specs, int simulationTime, bool isComputing, List<Task> tasks, List<Task> readyQueue)
        {
            float systemUsage = 0;
            bool preemptive = default;

            foreach (Task task in tasks)
            {
                systemUsage += ((float)task.ComputationTime / task.PeriodTime);
            }

            if (systemUsage > 1)
            {
                Console.WriteLine("Não programável");
                return;
            }

            else
            {
                for (int i = 0; i < simulationTime; i++)
                {
                    foreach (Task task in tasks)
                    {
                        AddPreemptiveToList(readyQueue, i, task);
                    }

                    if (readyQueue.Count != 0 && cpu.TaskInCpu != null && (readyQueue.ElementAt(0).RelativeDeadline < cpu.TaskInCpu.RelativeDeadline ||
                       (readyQueue.ElementAt(0).RelativeDeadline == cpu.TaskInCpu.RelativeDeadline && readyQueue.ElementAt(0).Deadline < cpu.TaskInCpu.Deadline))) 
                    { 
                        preemptive = true;
                    }

                    Console.WriteLine("Tempo: " + i);
                    isComputing = Check(readyQueue, isComputing, false, preemptive, true, i);
                }
            }
            InformationLog(specs, tasks);
        }














        public void StartSimulation(SchedulerSpecs specs)
        {

            int simulationTime = specs.SimulationTime;
            bool isComputing = false;

            List<Task> tasks = specs.Tasks;
            List<Task> readyQueue = new List<Task>();

            SimulationContext(tasks);

            switch (specs.SchedulerName.ToLower())
            {
                case "fcfs":
                    FCFS(specs, simulationTime, isComputing, tasks, readyQueue);
                    break;

                case "rr":
                    RoundRobin(specs, simulationTime, isComputing, tasks, readyQueue);
                    break;

                case "rm":
                    RateMonotonic(specs, simulationTime, isComputing, tasks, readyQueue);
                    break;

                case "edf":
                    EDF(specs, simulationTime, isComputing, tasks, readyQueue);
                    break;

                default:
                    Console.WriteLine("Escalonador Inválido");
                    break;
            }

        }

        

    }
}
