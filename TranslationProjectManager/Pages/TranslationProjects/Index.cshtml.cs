using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslationProjects
{
    public class IndexModel : PageModel
    {
        public IEnumerable<TranslationProjectTableView> TranslationProjects { get; set; }

        private readonly TranslationProjectManagerContext _context;
        
        public IndexModel(TranslationProjectManagerContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            TranslationProjects = _context.TranslationProjects
                .Include(tp => tp.Client)
                .Include(tp => tp.Manager)
                .Include(tp => tp.TranslationTasks)
                .Select(tp => new TranslationProjectTableView()
                {
                    Id = tp.Id,
                    ProjectStatus = ProjectStatus.FromValue(tp.Status),
                    Title = tp.Title,
                    SourceLanguage = tp.SourceLanguage,
                    TargetLanguage = tp.TargetLanguage,
                    Industry = Industry.FromValue(tp.Industry),
                    Unit = Unit.FromValue(tp.Unit),
                    CharactersNumber = tp.Amount,
                    ExpectedStart = tp.ExpectedStart,
                    InternalDeadline = tp.InternalDeadline,
                    ExternalDeadline = tp.ExternalDeadline,
                    Budget = tp.Budget,
                    Currency = tp.Currency,
                    Manager = tp.Manager,
                    Client = tp.Client,
                    TasksStats = new TranslationProjectTableView.TasksStatistics()
                    {
                        TasksCount = tp.TranslationTasks.Count,
                        NotStartedCount = tp.TranslationTasks.Count(tp => tp.Status == TaskStatusEnum.ReadyToStart),
                        InProgressCount = tp.TranslationTasks.Count(tp => tp.Status == TaskStatusEnum.InProgress || tp.Status == TaskStatusEnum.ReadyToFinalize),
                        FinalizedCount = tp.TranslationTasks.Count(tp => tp.Status == TaskStatusEnum.Finalized),
                        CancelledCount = tp.TranslationTasks.Count(tp => tp.Status == TaskStatusEnum.Cancelled)
                    }
                });
        }
    }

    public class TranslationProjectTableView
    {
        public class TasksStatistics
        {
            public int TasksCount { get; set; }
            public int NotStartedCount { get; set; }
            public int InProgressCount { get; set; }
            public int FinalizedCount { get; set; }
            public int CancelledCount { get; set; }
        }
        
        public long Id { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Title { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public Industry Industry { get; set; }
        public Unit Unit { get; set; }
        public long CharactersNumber { get; set; }
        public DateTime ExpectedStart { get; set; }
        public DateTime InternalDeadline { get; set; }
        public DateTime ExternalDeadline { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }

        public User Manager { get; set; }
        public Client Client { get; set; }

        public TasksStatistics TasksStats { get; set; }
    }
}
