namespace TranslationProjectManager.Data
{
    public class TranslationProjectFile
    {
        public long Id { get; set; }

        public long FileId { get; set; }
        public virtual File File { get; set; }

        public long TranslationProjectId { get; set; }
        public virtual TranslationProject TranslationProject { get; set; }

        public virtual IEnumerable<TranslationTask> VisibleIn { get; set; }
    }
}
