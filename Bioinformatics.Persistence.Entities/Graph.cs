namespace Bioinformatics.Persistence.Entities
{
    public class Graph
    {
        public string Id { get; set; }
        public ProteinNode RootNode { get; set; }
    }
}