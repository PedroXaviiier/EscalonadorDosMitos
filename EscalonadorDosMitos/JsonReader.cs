

using Newtonsoft.Json;

namespace EscalonadorDosMitos
{
    public class JsonReader
    {
        public SchedulerSpecs JsonFileReader(string filePath)
        {
            var jsonFile = File.ReadAllText(filePath);

            SchedulerSpecs specs = new SchedulerSpecs();

             specs = JsonConvert.DeserializeObject<SchedulerSpecs>(jsonFile);



            for (int i = 0; i < specs.TasksNumber; i++)
            {
                specs.Tasks.ElementAt(i).Index = i;
            }

            return specs;

        }
    }
}
            
                
            
    

            
    

