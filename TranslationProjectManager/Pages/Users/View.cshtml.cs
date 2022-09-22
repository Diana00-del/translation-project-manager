using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.Users
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public User User { get; set; }
        public IEnumerable<TranslationProjectTableView> TranslationProjects { get; set; }
        public IEnumerable<TranslatorAssignmentTableView> TranslatorAssignments { get; set; }
        public IEnumerable<IdentityRole<string>> UserRoles { get; set; }
        public IEnumerable<IdentityUserClaim<string>> UserClaims { get; set; }

        private readonly TranslationProjectManagerContext context;

        public ViewModel(TranslationProjectManagerContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            User = context.Users.Find(Id);
            TranslationProjects = context.TranslationProjects.Where(tp => tp.ManagerId == Id)
                .Include(tp => tp.TranslationTasks)
                .Include(tp => tp.Client)
                .Select(tp => new TranslationProjectTableView() {
                    Id = tp.Id,
                    ProjectStatus = ProjectStatus.FromValue(tp.Status),
                    Title = tp.Title,
                    SourceLanguage = tp.SourceLanguage,
                    TargetLanguage = tp.TargetLanguage,
                    Client = tp.Client,
                    Industry = Industry.FromValue(tp.Industry),
                    Unit = Unit.FromValue(tp.Unit),
                    CharactersNumber = tp.Amount,
                    ExpectedStart = tp.ExpectedStart,
                    ExternalDeadline = tp.ExternalDeadline,
                    Budget = tp.Budget,
                    Currency = tp.Currency,
                    TasksCount = tp.TranslationTasks.Count,
                });
            TranslatorAssignments = context.TranslatorAssignments.Where(ta => ta.TranslatorId == Id)
                .Include(ta => ta.TranslationTask)
                .Select(ta => new TranslatorAssignmentTableView()
                {
                    Id = ta.Id,
                    TranslatorAssignmentStatus = TranslatorAssignmentStatus.FromValue(ta.Status),
                    PriceRequested = ta.Price,
                    CurrencyRequested = ta.Currency,
                    TaskId = ta.TranslationTask.Id,
                    TaskTitle = ta.TranslationTask.Title,
                    TaskStatus = Data.TaskStatus.FromValue(ta.TranslationTask.Status),
                    SourceLanguage = ta.TranslationTask.SourceLanguage,
                    TargetLanguage = ta.TranslationTask.TargetLanguage,
                    Industry = Industry.FromValue(ta.TranslationTask.Industry),
                    Unit = Unit.FromValue(ta.TranslationTask.Unit),
                    Amount = ta.TranslationTask.Amount,
                    ExpectedStart = ta.TranslationTask.ExpectedStart,
                    Deadline = ta.TranslationTask.Deadline,
                    Budget = ta.TranslationTask.Budget,
                    Currency = ta.TranslationTask.Currency
                });

            var userRoleIds = context.UserRoles.Where(ur => ur.UserId == Id).Select(ur => ur.RoleId).ToList();
            UserRoles = context.Roles.Where(r => userRoleIds.Contains(r.Id));
            UserClaims = context.UserClaims.Where(uc => uc.UserId == Id);
        }
    }

    public class TranslationProjectTableView
    {
        public long Id { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Title { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public Client Client { get; set; }
        public Industry Industry { get; set; }
        public Unit Unit { get; set; }
        public long CharactersNumber { get; set; }
        public DateTime ExpectedStart { get; set; }
        public DateTime ExternalDeadline { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }
        public long TasksCount { get; set; }
    }

    public class TranslatorAssignmentTableView
    {
        public long Id { get; set; }
        public TranslatorAssignmentStatus TranslatorAssignmentStatus { get; set; }
        public decimal? PriceRequested { get; set; }
        public string CurrencyRequested { get; set; }

        public long TaskId { get; set; }
        public string TaskTitle { get; set; }

        public Data.TaskStatus TaskStatus { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public Industry Industry { get; set; }
        public Unit Unit { get; set; }
        public long Amount { get; set; }
        public DateTime ExpectedStart { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }
    }
}
