
using EscalonadorDosMitos;

string tipo;
JsonReader jsonReader = new JsonReader();
Simulation SimulationManager = new Simulation();

SchedulerSpecs fcfs = jsonReader.JsonFileReader("C:\\Users\\maria\\Source\\Repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_fcfs.json");
SchedulerSpecs rr = jsonReader.JsonFileReader("C:\\Users\\maria\\Source\\Repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_rr.json");
SchedulerSpecs rm = jsonReader.JsonFileReader("C:\\Users\\maria\\Source\\Repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_rm.json");
SchedulerSpecs edf = jsonReader.JsonFileReader("C:\\Users\\maria\\Source\\Repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_edf.json");

do
{
    Console.WriteLine("\n________________________________________ " +
                      "\n\nDeseja simular qual tipo de escalonador? \n" +
                      "First come first served = fcfs \n" +
                      "Round robin = rr \n" +
                      "Rate monotonic = rm \n" +
                      "Earliest deadline first = edf \n" +
                      "Sair do programa = sair ");

tipo = Console.ReadLine().ToLower();


    switch (tipo)
    {
        case "fcfs":
            SimulationManager.StartSimulation(fcfs);
            break;

        case "rr":
            SimulationManager.StartSimulation(rr);
            break;

        case "rm":
            SimulationManager.StartSimulation(rm);
            break;

        case "edf":
            SimulationManager.StartSimulation(edf);
            break;

        case "sair":
            break;

        default:
            Console.WriteLine("Escalonador Inválido");
            break;
    }
}
while (tipo != "sair");


    


