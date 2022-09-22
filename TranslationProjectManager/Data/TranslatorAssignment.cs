namespace TranslationProjectManager.Data;

public class TranslatorAssignment
{
    public long Id { get; set; }
    public TranslatorAssignmentStatusEnum Status { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }

    public string TranslatorId { get; set; }
    public virtual User Translator { get; set; }
    public long TranslationTaskId { get; set; }
    public virtual TranslationTask TranslationTask { get; set; }
}
