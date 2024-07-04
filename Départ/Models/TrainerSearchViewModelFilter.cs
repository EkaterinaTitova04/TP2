using X.PagedList;

namespace JuliePro.Models
{
    public class TrainerSearchViewModelFilter
    {
        
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public IPagedList<Trainer>? Items { get; set; }
        public string? SearchString { get; set; }



    }
}
