namespace JuliePro.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
