namespace TranslationProjectManager.Data;

public class TranslationProject
{
    public long Id { get; set; }
    public ProjectStatusEnum Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
    public AccuracyEnum Accuracy { get; set; }
    public IndustryEnum Industry { get; set; }
    public UnitEnum Unit { get; set; }
    public long Amount { get; set; }
    public DateTime ExpectedStart { get; set; }
    public DateTime InternalDeadline { get; set; }
    public DateTime ExternalDeadline { get; set; }
    public decimal Budget { get; set; }
    public string Currency { get; set; }

    public string ManagerId { get; set; }
    public virtual User Manager { get; set; }
    public long ClientId { get; set; }
    public virtual Client Client { get; set; }

    public virtual IList<TranslationTask> TranslationTasks { get; set; }
    public virtual IList<TranslationProjectFile> TranslationProjectFiles { get; set; }
    public virtual IList<TranslationProjectComment> TranslationProjectComments { get; set; }
}
