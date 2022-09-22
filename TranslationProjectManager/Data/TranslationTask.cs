namespace TranslationProjectManager.Data;

public class TranslationTask
{
    public long Id { get; set; }
    public string Title { get; set; }
    public TaskStatusEnum Status { get; set; }
    public string Description { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
    public AccuracyEnum Accuracy { get; set; }
    public IndustryEnum Industry { get; set; }
    public UnitEnum Unit { get; set; }
    public long Amount { get; set; }
    public DateTime ExpectedStart { get; set; }
    public DateTime Deadline { get; set; }
    public decimal Budget { get; set; }
    public string Currency { get; set; }

    public long TranslationProjectId { get; set; }
    public virtual TranslationProject TranslationProject { get; set; }
    public virtual IList<TranslatorAssignment> TranslatorAssignments { get; set; }
    public virtual IList<TranslationProjectFile> VisibleFiles { get; set; }
    public virtual IList<TranslationTaskComment> TranslationTaskComments { get; set; }
}
