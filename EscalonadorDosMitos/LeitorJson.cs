

using Newtonsoft.Json;

namespace EscalonadorDosMitos
{
    public class LeitorJson
    {
        public SpecsEscalonador lerArquivoJson(string filePath)
        {

            SpecsEscalonador? specs = JsonConvert.DeserializeObject<SpecsEscalonador>(filePath);

            for (int i = 0; i < specs.tasksNumber; i++)
            {
                specs.tasks.ElementAt(i).Index = i;
            }

            return specs;

        }
    }
}
            
                
            
    

            
    

