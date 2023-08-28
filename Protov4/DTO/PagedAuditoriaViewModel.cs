namespace Protov4.DTO
{
    public class PagedAuditoriaViewModel
    {
        public List<AuditoriaDTO> AuditoriaList { get; set; }
        public string Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
