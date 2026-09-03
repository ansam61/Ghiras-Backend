namespace Ghiras.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalPlants { get; set; }
        public int TotalCategories { get; set; }
        public int TotalDiagnoses { get; set; }
        public int TotalUsers { get; set; }
        public List<PlantViewModel> RecentPlants { get; set; } = new();
        public List<AIDiagnosisViewModel> RecentDiagnoses { get; set; } = new();
    }
}
