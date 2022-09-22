using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslatorAssignments
{
    public class IndexModel : PageModel
    {
        public IEnumerable<InviteTableView> Invites { get; set; }

        private readonly TranslationProjectManagerContext _context;

        public IndexModel(TranslationProjectManagerContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            Invites = _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .Select(ta => new InviteTableView
                {
                    Id = ta.Id,
                    Status = TranslatorAssignmentStatus.FromValue(ta.Status),
                    Price = ta.Price,
                    Currency = ta.Currency,
                    Title = ta.TranslationTask.Title,
                    SourceLanguage = ta.TranslationTask.SourceLanguage,
                    TargetLanguage = ta.TranslationTask.TargetLanguage,
                    Accuracy = Accuracy.FromValue(ta.TranslationTask.Accuracy),
                    Industry = Industry.FromValue(ta.TranslationTask.Industry),
                    Unit = Unit.FromValue(ta.TranslationTask.Unit),
                    Amount = ta.TranslationTask.Amount,
                    ExpectedStart = ta.TranslationTask.ExpectedStart,
                    Deadline = ta.TranslationTask.Deadline
                });
        }
    }

    public class InviteTableView
    {
        public long Id { get; set; }
        public TranslatorAssignmentStatus Status { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public string Title { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public Accuracy Accuracy { get; set; }
        public Industry Industry { get; set; }
        public Unit Unit { get; set; }
        public long Amount { get; set; }
        public DateTime ExpectedStart { get; set; }
        public DateTime Deadline { get; set; }
    }
}
