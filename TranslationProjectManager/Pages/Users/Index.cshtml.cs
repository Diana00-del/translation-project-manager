using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;
using TranslationProjectManager.Services;

namespace TranslationProjectManager.Pages.Users
{
    public class IndexModel : PageModel
    {
        public IEnumerable<UserTableView> Users { get; set; }

        private readonly TranslationProjectManagerContext context;

        public IndexModel(TranslationProjectManagerContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            Users = context.Users
                .Include(u => u.TranslationProjects)
                .Include(u => u.TranslatorAssignments)
                .Select(u => new UserTableView()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = $"{u.LastName} {u.FirstName}",
                    PhoneNumber = u.PhoneNumber,
                    Address = $"{u.Country}, {u.State}, {u.City}, {u.Address}, {u.Zip}",
                    ProjectsStats = new UserTableView.ProjectsStatistics()
                    {
                        ProjectsCount = u.TranslationProjects.Count,
                        NotStartedCount = u.TranslationProjects.Count(p => p.Status == ProjectStatusEnum.Draft || p.Status == ProjectStatusEnum.ReadyToStart),
                        InProgressCount = u.TranslationProjects.Count(p => p.Status == ProjectStatusEnum.InProgress || p.Status == ProjectStatusEnum.ReadyToFinalize),
                        FinalizedCount = u.TranslationProjects.Count(p => p.Status == ProjectStatusEnum.Finalized),
                        CancelledCount = u.TranslationProjects.Count(p => p.Status == ProjectStatusEnum.Cancelled)
                    },
                    TasksStats = new UserTableView.TasksStatistics()
                    {
                        TasksCount = u.TranslatorAssignments.Count,
                        NotStartedCount = u.TranslatorAssignments.Count(ta => ta.TranslationTask.Status == TaskStatusEnum.ReadyToStart),
                        InProgressCount = u.TranslatorAssignments.Count(ta => ta.TranslationTask.Status == TaskStatusEnum.InProgress || ta.TranslationTask.Status == TaskStatusEnum.ReadyToFinalize),
                        FinalizedCount = u.TranslatorAssignments.Count(ta => ta.TranslationTask.Status == TaskStatusEnum.Finalized),
                        CancelledCount = u.TranslatorAssignments.Count(ta => ta.TranslationTask.Status == TaskStatusEnum.Cancelled),
                        RejectedByUserCount = u.TranslatorAssignments.Count(ta => ta.Status == TranslatorAssignmentStatusEnum.InviteRejected)
                    }
                });
        }
    }

    public class UserTableView
    {
        public class ProjectsStatistics
        {
            public int ProjectsCount { get; set; }
            public int NotStartedCount { get; set; }
            public int InProgressCount { get; set; }
            public int FinalizedCount { get; set; }
            public int CancelledCount { get; set; }
        }

        public class TasksStatistics
        {
            public int TasksCount { get; set; }
            public int NotStartedCount { get; set; }
            public int InProgressCount { get; set; }
            public int FinalizedCount { get; set; }
            public int CancelledCount { get; set; }
            public int RejectedByUserCount { get; set; }
        }        
        
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ProjectsStatistics ProjectsStats { get; set; }
        public TasksStatistics TasksStats { get; set; }
    }
}
