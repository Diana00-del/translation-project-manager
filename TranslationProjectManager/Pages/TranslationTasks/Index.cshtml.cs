using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslationTasks
{
    public class IndexModel : PageModel
    {
        public IEnumerable<TranslationTaskTableView> TranslationTasks { get; set; }

        private readonly TranslationProjectManagerContext _context;

        public IndexModel(TranslationProjectManagerContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            TranslationTasks = _context.TranslationTasks
                .Include(tt => tt.TranslationProject)
                .Include(tt => tt.TranslatorAssignments).ThenInclude(ta => ta.Translator)
                .Select(tt => new TranslationTaskTableView()
                {
                    Id = tt.Id,
                    Title = tt.Title,
                    TaskStatus = Data.TaskStatus.FromValue(tt.Status),
                    SourceLanguage = tt.SourceLanguage,
                    TargetLanguage = tt.TargetLanguage,
                    Unit = Unit.FromValue(tt.Unit),
                    Amount = tt.Amount,
                    ExpectedStart = tt.ExpectedStart,
                    Deadline = tt.Deadline,
                    Budget = tt.Budget,
                    Currency = tt.Currency,
                    TranslationProjectId = tt.TranslationProject.Id,
                    TranslationProjectTitle = tt.TranslationProject.Title,
                    InvitesStats = tt.GetInvitesStatistics(),
                    AcceptedInvite = tt.GetAcceptedInvite()
                });
        }
    }

    public class TranslationTaskTableView
    {

        public class InvitesStatistics
        {
            public long InvitedTotal { get; set; }
            public long OffersSent { get; set; }
            public long OffersRejected { get; set; }
        }

        public class AcceptedInviteInfo
        {
            public string TranslatorId { get; set; }
            public string TranslatorFullName { get; set; }
            public decimal? Price { get; set; }
            public string Currency { get; set; }
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public Data.TaskStatus TaskStatus { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public Unit Unit { get; set; }
        public long Amount { get; set; }
        public DateTime ExpectedStart { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }

        public long TranslationProjectId { get; set; }
        public string TranslationProjectTitle { get; set; }

        public InvitesStatistics InvitesStats { get; set; }
        public AcceptedInviteInfo AcceptedInvite { get; set; }
    }

    public static class TranslationTaskExtensions
    {
        public static TranslationTaskTableView.InvitesStatistics GetInvitesStatistics(this TranslationTask task)
        {
            return new TranslationTaskTableView.InvitesStatistics
            {
                InvitedTotal = task.TranslatorAssignments.Count(),
                OffersSent = task.TranslatorAssignments.Count(ta => ta.Status == TranslatorAssignmentStatusEnum.OfferSent || ta.Status == TranslatorAssignmentStatusEnum.OfferRejected || ta.Status == TranslatorAssignmentStatusEnum.Accepted),
                OffersRejected = task.TranslatorAssignments.Count(ta => ta.Status == TranslatorAssignmentStatusEnum.InviteRejected)
            };
        }

        public static TranslationTaskTableView.AcceptedInviteInfo GetAcceptedInvite(this TranslationTask task)
        {
            var acceptedInvite = task.TranslatorAssignments.FirstOrDefault(ta => ta.Status == TranslatorAssignmentStatusEnum.Accepted);

            if (acceptedInvite == null)
                return null;

            return new TranslationTaskTableView.AcceptedInviteInfo()
            {
                TranslatorId = acceptedInvite.TranslatorId,
                TranslatorFullName = $"{acceptedInvite.Translator.FirstName} {acceptedInvite.Translator.LastName}",
                Price = acceptedInvite.Price,
                Currency = acceptedInvite.Currency
            };
        }
    }
}
