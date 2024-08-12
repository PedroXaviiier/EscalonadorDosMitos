
using Newtonsoft.Json;

namespace EscalonadorDosMitos
{
    public class SchedulerSpecs
    {
        
        public int SimulationTime { get; set; }
        
        public String SchedulerName { get; set; }
        
        public int TasksNumber { get; set; }
        
        public List<Task> Tasks { get; set; }
    }
}
