

namespace EscalonadorDosMitos
{
    public class Simulacao
    {
        private Cpu cpu;
        private int tempoCpu;
        private int[] tempoEspera;
        private int[] tempoNaCpu;
        private int[] quantidadeAtivacao;
        private int[] quantidadePrazoPerdido;

        public Simulacao()
        {
            cpu = new Cpu();
        }

        private void contextoSimulacao(List<Task> tarefas)
        {
            tempoCpu = 0;
            tempoEspera = new int[tarefas.Count()];
            tempoNaCpu = new int[tarefas.Count()];
            quantidadeAtivacao = new int[tarefas.Count()];
            quantidadePrazoPerdido = new int[tarefas.Count()];

            for (int i = 0; i < tarefas.Count(); i++)
            {
                tempoEspera[i] = 0;
                tempoNaCpu[i] = 0;
                quantidadeAtivacao[i] = 0;
                quantidadePrazoPerdido[i] = 0;
            }
        }






        private void imprimirResultados(List<int> indices, string tipo, float tempo)
        {
            if (indices.Count() == 1)
            {
                Console.WriteLine("A Tarefa " + (indices.ElementAt(0) + 1) + " teve o " + tipo + " tempo de espera: " + tempo);
            }

            else
            {
                Console.WriteLine("As Tarefas ");
                for (int i = 0; i < indices.Count(); i++)
                {
                    if (i > 0)
                    {
                        if (i == indices.Count() - 1)
                        {
                            Console.WriteLine(" e ");
                        }

                        else
                        {
                            Console.WriteLine(", "); 
                        }
                    }
                    Console.WriteLine(indices.ElementAt(0) + 1);
                }

                Console.WriteLine(" tiveram o " + tipo + " tempo de espera: " + tempo);
            }
        }






        private void informacoesLog(SpecsEscalonador specs, List<Task> tarefas)
        {
            Console.WriteLine(" ");

            float utilizacao = 0;

            for (int i = 0; i < specs.tasksNumber; i++)
            {
                utilizacao += (float)(tempoNaCpu[i]) / specs.simulationTime;
            }

            utilizacao *= 100;

            float produtividade = (float)specs.tasksNumber / specs.simulationTime;

            float tempoEsperaMedio, somaTempoEspera = 0;
            float tempoTurnaroundMedio, somaTempoTurnaround = 0;

            List<int> indicesMaisLongos = new List<int>();
            List<int> indicesMaisCurtos = new List<int>();
            float tempoEsperaMaisLongo = float.MinValue, tempoEsperaMaisCurto = float.MaxValue;

            for (int i = 0; i < specs.tasksNumber; i++)
            {
                somaTempoEspera += (float)tempoEspera[i] / quantidadeAtivacao[i];
                somaTempoTurnaround += (float)(tempoNaCpu[i] + tempoEspera[i]) / quantidadeAtivacao[i];

                Console.WriteLine("Ativacoes para Tarefa " + (i + 1) + ": " + quantidadeAtivacao[i]);
                Console.WriteLine("Tempo de Espera para Tarefa " + (i + 1) + ": " + (float)tempoEspera[i] / quantidadeAtivacao[i]);
                Console.WriteLine("Turnaround para Tarefa " + (i + 1) + ": " + (tempoNaCpu[i] + tempoEspera[i]) / quantidadeAtivacao[i]);

                if (tempoNaCpu[i] + tempoEspera[i] >= specs.simulationTime)
                {
                    Console.WriteLine("Prazo Perdido para Tarefa " + (i + 1));
                }

                if (quantidadePrazoPerdido[i] != 0)
                {
                    Console.WriteLine("Prazo Perdido para Tarefa " + (i + 1));
                    Console.WriteLine("Proporcao de Prazos Perdidos: " + ((float)quantidadePrazoPerdido[i] / quantidadeAtivacao[i]));
                }
                Console.WriteLine("-------------------------------------");

                float tempoAtualEspera = (float)tempoEspera[i] / quantidadeAtivacao[i];

                if (tempoAtualEspera > tempoEsperaMaisLongo)
                {
                    tempoEsperaMaisLongo = tempoAtualEspera;
                    indicesMaisLongos.Clear();
                    indicesMaisLongos.Add(i);
                }
                else if (tempoAtualEspera == tempoEsperaMaisLongo)
                {
                    indicesMaisLongos.Add(i);
                }

                if (tempoAtualEspera < tempoEsperaMaisCurto)
                {
                    tempoEsperaMaisCurto = tempoAtualEspera;
                    indicesMaisCurtos.Clear();
                    indicesMaisCurtos.Add(i);
                }
                else if (tempoAtualEspera == tempoEsperaMaisCurto)
                {
                    indicesMaisCurtos.Add(i);
                }
            }

            imprimirResultados(indicesMaisLongos, "maior", tempoEsperaMaisLongo);
            imprimirResultados(indicesMaisCurtos, "menor", tempoEsperaMaisCurto);

            tempoEsperaMedio = somaTempoEspera / tarefas.Count();
            tempoTurnaroundMedio = somaTempoTurnaround / tarefas.Count();

            Console.WriteLine("Utilizacao: " + utilizacao);
            Console.WriteLine("Produtividade: " + produtividade);
            Console.WriteLine("Tempo de Espera Medio: " + tempoEsperaMedio);
            Console.WriteLine("Tempo de Turnaround Medio: " + tempoTurnaroundMedio);
        }







        private void adicionarNaoPreemptivoALista(List<Task> tarefas, List<Task> filaPronta, int i)
        {
            foreach (Task tarefa in tarefas)
            {
                if (i == tarefa.Offset || ((i - tarefa.Offset) % tarefa.PeriodTime) == 0)
                {
                    filaPronta.Add(tarefa.CloneTask());
                    quantidadeAtivacao[tarefa.Index]++;
                }
            }

            Console.WriteLine("Tempo: " + i);
        }

        private void adicionarPreemptivoALista(List<Task> filaPronta, int i, Task tarefa)
        {
            if (i >= tarefa.Offset && (i - tarefa.Offset) % tarefa.PeriodTime == 0)
            {
                if (filaPronta == null)
                {
                    filaPronta.Add(tarefa.CloneTask());
                    quantidadeAtivacao[tarefa.Index]++;
                }

                else
                {
                    int indexInsercao = 0;
                    for (int j = 0; j < filaPronta.Count(); j++)
                    {
                        if (tarefa.PeriodTime < filaPronta.ElementAt(j).PeriodTime)
                        {
                            indexInsercao = j;
                            break;
                        }
                        else
                        {
                            indexInsercao = j + 1;
                        }
                    }
                    filaPronta[indexInsercao] = tarefa.CloneTask();
                    quantidadeAtivacao[tarefa.Index]++;
                }
            }
        }









        private bool passo(List<Task> filaPronta, bool isComputing, bool possuiQuantum, bool preemptive, bool possuiPrazo, int tempo)
        {
            if (filaPronta == null || cpu.tarefaNaCpu != null)
            {
                if (!isComputing || preemptive)
                {
                    if (cpu.tarefaNaCpu != null && cpu.tarefaNaCpu.ComputationTime != 0)
                    {
                        filaPronta.Add(cpu.tarefaNaCpu);
                        cpu.tarefaNaCpu = null;
                    }

                    if (filaPronta == null)
                    {
                        isComputing = cpu.Computar(filaPronta.ElementAt(0), possuiQuantum, preemptive);
                        filaPronta.RemoveAt(0);
                    }

                    else
                    {
                        isComputing = cpu.Computar(cpu.tarefaNaCpu, possuiQuantum, preemptive);
                    }
                }
                else
                {
                    isComputing = cpu.Computar(cpu.tarefaNaCpu, possuiQuantum, false);
                }
            }

            if (cpu.tarefaNaCpu != null)
            {
                tempoNaCpu[cpu.tarefaNaCpu.Index]++;

                cpu.tarefaNaCpu.RelativeDeadline = cpu.tarefaNaCpu.RelativeDeadline - 1;

                if (cpu.tarefaNaCpu.RelativeDeadline.Equals(0) && possuiPrazo)
                {
                    quantidadePrazoPerdido[cpu.tarefaNaCpu.Index]++;
                    Console.WriteLine("Prazo perdido: " + (cpu.tarefaNaCpu.Index + 1));
                }

                if (cpu.tarefaNaCpu.ComputationTime == 0)
                {
                    cpu.tarefaNaCpu = null;
                }
            }

            else
            {
                Console.WriteLine("    Tarefa na CPU: null");
            }

            foreach (Task tarefa in filaPronta)
            {
                tempoEspera[tarefa.Index]++;

                tarefa.RelativeDeadline = tarefa.RelativeDeadline - 1;

                if (tarefa.RelativeDeadline == 0 && possuiPrazo)
                {
                    quantidadePrazoPerdido[tarefa.Index]++;
                    Console.WriteLine("Prazo perdido: " + (tarefa.Index + 1));
                }
            }

            return isComputing;
        }










        private void fcfs(SpecsEscalonador specs, int tempoDeSimulacao, bool estaComputando, List<Task> tarefas, List<Task> filaPronta)
        {
            for (int i = 0; i < tempoDeSimulacao; i++)
            {
                adicionarNaoPreemptivoALista(tarefas, filaPronta, i);
                estaComputando = passo(filaPronta, estaComputando, false, false, false, i);
            }

            informacoesLog(specs, tarefas);
        }

        private void roundRobin(SpecsEscalonador specs, int tempoDeSimulacao, bool estaComputando, List<Task> tarefas, List<Task> filaPronta)
        {
            for (int i = 0; i < tempoDeSimulacao; i++)
            {
                adicionarNaoPreemptivoALista(tarefas, filaPronta, i);
                estaComputando = passo(filaPronta, estaComputando, true, false, false, i);
            }

            informacoesLog(specs, tarefas);
        }

        private void rateMonotonic(SpecsEscalonador specs, int tempoDeSimulacao, bool isComputing, List<Task> tarefas, List<Task> filaPronta)
        {
            bool preemptive = default;
            float utilizacaoSistema = 0;

            foreach (Task tarefa in tarefas)
            {
                utilizacaoSistema += ((float)tarefa.ComputationTime / tarefa.PeriodTime);
            }

            if (utilizacaoSistema > (specs.tasksNumber * (Math.Pow(2, ((double)1 / specs.tasksNumber) - 1))))
            {
                Console.WriteLine("Não programável");
                return;
            }

            for (int i = 0; i < tempoDeSimulacao; i++)
            {
                foreach (Task tarefa in tarefas)
                {
                    adicionarPreemptivoALista(filaPronta, i, tarefa);
                }

                if(filaPronta == null && cpu.tarefaNaCpu != null && filaPronta.ElementAt(0).Deadline < cpu.tarefaNaCpu.Deadline) 
                {
                    preemptive = true;
                }
                

                Console.WriteLine("Tempo: " + i);
                isComputing = passo(filaPronta, isComputing, false, preemptive, true, i);
            }

            informacoesLog(specs, tarefas);
        }

        private void earliestDeadlineFirst(SpecsEscalonador specs, int tempoDeSimulacao, bool estaComputando, List<Task> tarefas, List<Task> filaPronta)
        {
            float utilizacaoSistema = 0;
            bool preemptive = default;

            foreach (Task tarefa in tarefas)
            {
                utilizacaoSistema += ((float)tarefa.ComputationTime / tarefa.PeriodTime);
            }

            if (utilizacaoSistema > 1)
            {
                Console.WriteLine("Não programável");
                return;
            }

            else
            {
                for (int i = 0; i < tempoDeSimulacao; i++)
                {
                    foreach (Task tarefa in tarefas)
                    {
                        adicionarPreemptivoALista(filaPronta, i, tarefa);
                    }

                    if (filaPronta == null && cpu.tarefaNaCpu != null && (filaPronta.ElementAt(0).RelativeDeadline < cpu.tarefaNaCpu.RelativeDeadline ||
                       (filaPronta.ElementAt(0).RelativeDeadline == cpu.tarefaNaCpu.RelativeDeadline && filaPronta.ElementAt(0).Deadline < cpu.tarefaNaCpu.Deadline))) 
                    { 
                        preemptive = true;
                    }

                    Console.WriteLine("Tempo: " + i);
                    estaComputando = passo(filaPronta, estaComputando, false, preemptive, true, i);
                }
            }
            informacoesLog(specs, tarefas);
        }














        public void iniciarSimulacao(SpecsEscalonador specs)
        {

            int tempoDeSimulacao = specs.simulationTime;
            bool isComputing = false;

            List<Task> tarefas = specs.tasks;
            List<Task> filaPronta = new List<Task>();

            contextoSimulacao(tarefas);

            switch (specs.schedulerName.ToLower())
            {
                case "fcfs":
                    fcfs(specs, tempoDeSimulacao, isComputing, tarefas, filaPronta);
                    break;

                case "rr":
                    roundRobin(specs, tempoDeSimulacao, isComputing, tarefas, filaPronta);
                    break;

                case "rm":
                    rateMonotonic(specs, tempoDeSimulacao, isComputing, tarefas, filaPronta);
                    break;

                case "edf":
                    earliestDeadlineFirst(specs, tempoDeSimulacao, isComputing, tarefas, filaPronta);
                    break;

                default:
                    Console.WriteLine("Escalonador Inválido");
                    break;
            }

        }

        

    }
}
