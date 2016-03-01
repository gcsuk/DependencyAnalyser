namespace DependencyAnalyser.Models
{
    public class Args
    {
        public string ProjectName { get; set; }
        public string BuildId { get; set; }
        public string BuildRoot { get; set; }
        public ConsolidationLevel ConsolidationLevel { get; set; }
        public bool ConsolidationEnforced { get; set; }
        public bool UploadResults { get; set; }
        public override string ToString()
        {
            return $"{ProjectName} {BuildId} {BuildRoot} {ConsolidationLevel} {ConsolidationEnforced} {UploadResults}";
        }
    }
}
