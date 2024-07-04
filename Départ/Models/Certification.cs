namespace JuliePro.Models
{
    public class Certification
    {
        public int Id { get; set; }
        public string? CertificationCenter { get; set; }


        public virtual Trainer? Trainer { get; set; }

    }
}
