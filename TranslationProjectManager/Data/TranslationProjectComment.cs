namespace TranslationProjectManager.Data
{
    public class TranslationProjectComment
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TranslationProjectId { get; set; }
        public virtual TranslationProject TranslationProject { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}
