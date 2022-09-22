namespace TranslationProjectManager.Data
{
    public class TranslationTaskComment
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TranslationTaskId { get; set; }
        public virtual TranslationTask TranslationTask { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}
