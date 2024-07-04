using Microsoft.AspNetCore.Mvc.Rendering;
using JuliePro.Models;


namespace JuliePro.Models
{
    public class RecordViewModel
    {
        public Record Record { get; set; }
        public SelectList TrainerList { get; set; }
        public SelectList CustomerList { get; set; } 


    }
}
