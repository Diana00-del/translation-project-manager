using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;
using TranslationProjectManager.Services;

namespace TranslationProjectManager.Pages.TranslationProjects
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public IEnumerable<TranslationTaskTableView> TranslatorAssignments { get; set; }

        public IEnumerable<TranslationProjectFileTableView> TranslationProjectFiles { get; set; }

        public IEnumerable<CommentTableView> Comments { get; set; }

        public ProjectViewModel Project { get; set; }

        [BindProperty]
        public FilesInputModel FilesInput { get; set; }

        [BindProperty]
        public CommentInputModel CommentInput { get; set; }

        public class ProjectViewModel
        {
            [Display(Name = "Id")]
            public long Id { get; set; }

            [Display(Name = "Title")]
            public string Title { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            public ProjectStatusEnum StatusEnum { get; set; }

            [Display(Name = "Source Language")]
            public string SourceLanguage { get; set; }

            [Display(Name = "Target Language")]
            public string TargetLanguage { get; set; }

            [Display(Name = "Accuracy")]
            public string Accuracy { get; set; }

            [Display(Name = "Industry")]
            public string Industry { get; set; }

            [Display(Name = "Unit")]
            public string Unit { get; set; }

            [Display(Name = "Amount")]
            public long Amount { get; set; }

            [Display(Name = "Expected Start"), DataType(DataType.Date)]
            public DateTime ExpectedStart { get; set; }

            [Display(Name = "Internal Deadline"), DataType(DataType.Date)]
            public DateTime InternalDeadline { get; set; }

            [Display(Name = "External Deadline"), DataType(DataType.Date)]
            public DateTime ExternalDeadline { get; set; }

            [Display(Name = "Budget")]
            public decimal Budget { get; set; }

            [Display(Name = "Currency")]
            public string Currency { get; set; }

            public string ManagerId { get; set; }

            [Display(Name = "Manager")]
            public string Manager { get; set; }

            public long ClientId { get; set; }

            [Display(Name = "Client")]
            public string Client { get; set; }
        }

        public class FilesInputModel
        {
            [Required, Display(Name = "Files"), DataType(DataType.Upload)]
            public IFormFileCollection Files { get; set; }
        }

        public class CommentInputModel
        {
            [Required, Display(Name = "Comment")]
            public string Comment { get; set; }
        }

        private readonly TranslationProjectManagerContext _context;
        private readonly IFileStorageService _fileStorageService;

        public ViewModel(TranslationProjectManagerContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public void OnGet()
        {
            var project = _context.TranslationProjects
                .Include(tp => tp.Client)
                .Include(tp => tp.Manager)
                .FirstOrDefault(tp => tp.Id == Id);

            Project = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Status = ProjectStatus.FromValue(project.Status).Title,
                StatusEnum = project.Status,
                Description = project.Description,
                SourceLanguage = project.SourceLanguage,
                TargetLanguage = project.TargetLanguage,
                Accuracy = Accuracy.FromValue(project.Accuracy).Title,
                Industry = Industry.FromValue(project.Industry).Title,
                Unit = Unit.FromValue(project.Unit).Title,
                Amount = project.Amount,
                ExpectedStart = project.ExpectedStart,
                InternalDeadline = project.InternalDeadline,
                ExternalDeadline = project.ExternalDeadline,
                Budget = project.Budget,
                Currency = project.Currency,
                ManagerId = project.ManagerId,
                Manager = $"{project.Manager.FirstName} {project.Manager.LastName}",
                ClientId = project.ClientId,
                Client = project.Client.Name
            };

            TranslatorAssignments = _context.TranslationTasks
                .Include(tt => tt.TranslatorAssignments)
                .Where(tt => tt.TranslationProjectId == Id)
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
                    InvitesStats = tt.GetInvitesStatistics(),
                    AcceptedInvite = tt.GetAcceptedInvite()
                });

            TranslationProjectFiles = _context.TranslationProjectFiles
                .Include(tpf => tpf.VisibleIn)
                .Include(tpf => tpf.File)
                .Where(tpf => tpf.TranslationProjectId == Id)
                .Select(tpf => new TranslationProjectFileTableView()
                {
                    Id = tpf.Id,
                    Name = tpf.File.Name,
                    VisibleInTasks = tpf.VisibleIn.Select(vi => new TranslationProjectFileTableView.TaskReference() { Id = vi.Id, Title = vi.Title}).ToList(),
                });

            Comments = _context.TranslationProjectComments
                .Include(tp => tp.Author)
                .Where(tp => tp.TranslationProjectId == Id)
                .OrderBy(tp => tp.CreatedAt)
                .Select(tp => new CommentTableView()
                {
                    CreatedAt = tp.CreatedAt,
                    Comment = tp.Comment,
                    AuthorId = tp.AuthorId,
                    AuthorFullName = $"{tp.Author.FirstName} {tp.Author.LastName}",
                });
        }

        public async Task<FileResult> OnGetDownloadFile(long fileId)
        {
            var file = await _context.TranslationProjectFiles
                .Include(tpf => tpf.File)
                .FirstOrDefaultAsync(tpf => tpf.Id == fileId);

            return File(await _fileStorageService.GetFile(file.File.Path), "application/octet-stream", file.File.Name);
        }

        public IActionResult OnPostMoveToStatus(ProjectStatusEnum status)
        {
            var project = _context.TranslationProjects.Find(Id);

            project.Status = status;
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/View", new { Id = Id });
        }

        public IActionResult OnPostAddComment()
        {   
            var comment = new TranslationProjectComment
            {
                TranslationProjectId = Id,
                Comment = CommentInput.Comment,
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                AuthorId = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name).Id
            };

            _context.TranslationProjectComments.Add(comment);
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/View", new { Id = Id });
        }

        public async Task<IActionResult> OnPostUploadFiles()
        {
            var project = _context.TranslationProjects.Find(Id);

            foreach (var file in FilesInput.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = await _fileStorageService.SaveFile(FileType.ProjectFile, file.OpenReadStream());
                    var fileDescriptor = new Data.File
                    {
                        Name = file.FileName,
                        Path = filePath,
                        MimeType = file.ContentType
                    };

                    var projectFile = new TranslationProjectFile
                    {
                        File = fileDescriptor,
                        TranslationProject = project
                    };

                    _context.Files.Add(fileDescriptor);
                    _context.TranslationProjectFiles.Add(projectFile);

                    _context.SaveChanges();
                }
            }

            return RedirectToPage("/TranslationProjects/View", new { Id = Id });
        }
    }

    public class TranslationTaskTableView
    {

        public class InvitesStatistics
        {
            public int InvitedTotal { get; set; }
            public int OffersSent { get; set; }
            public int OffersRejected { get; set; }
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

        public InvitesStatistics InvitesStats { get; set; }
        public AcceptedInviteInfo AcceptedInvite { get; set; }
    }

    public class TranslationProjectFileTableView
    {
        public class TaskReference
        {
            public long Id { get; set; }
            public string Title { get; set; }
        }
        
        public long Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<TaskReference> VisibleInTasks { get; set; }
    }

    public class CommentTableView
    {
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public string AuthorId { get; set; }
        public string AuthorFullName { get; set; }
    }

    public static class TranslationTaskExtensions
    {
        public static TranslationTaskTableView.InvitesStatistics GetInvitesStatistics(this TranslationTask task)
        {
            return new TranslationTaskTableView.InvitesStatistics
            {
                InvitedTotal = task.TranslatorAssignments.Count,
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
