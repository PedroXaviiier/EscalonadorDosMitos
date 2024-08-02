
using EscalonadorDosMitos;

LeitorJson leitorJson = new LeitorJson();
Simulacao gerenciadorSimulacao = new Simulacao();

SpecsEscalonador fcfs = leitorJson.lerArquivoJson("C:\\Users\\phxav\\source\\repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_fcfs.json");
SpecsEscalonador rr = leitorJson.lerArquivoJson("C:\\Users\\phxav\\source\\repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_rr.json");
SpecsEscalonador rm = leitorJson.lerArquivoJson("C:\\Users\\phxav\\source\\repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_rm.json");
SpecsEscalonador edf = leitorJson.lerArquivoJson("C:\\Users\\phxav\\source\\repos\\EscalonadorDosMitos\\EscalonadorDosMitos\\sched_edf.json");

Console.WriteLine("Deseja simular qual tipo de escalonador? \n" +
                  "first come first served = fcfs \n" +
                  "round robin = rr \n" +
                  "rate monotonic = rm \n" +
                  "earliest deadline first = edf");

string tipo = Console.ReadLine().ToLower();

switch (tipo)
{
    case "fcfs":
        gerenciadorSimulacao.iniciarSimulacao(fcfs);
        break;

    case "rr":
        gerenciadorSimulacao.iniciarSimulacao(rr);
        break;

    case "rm":
        gerenciadorSimulacao.iniciarSimulacao(rm);
        break;

    case "edf":
        gerenciadorSimulacao.iniciarSimulacao(edf);
        break;

    default:
        Console.WriteLine("Escalonador Inválido");
        break;
}
    


