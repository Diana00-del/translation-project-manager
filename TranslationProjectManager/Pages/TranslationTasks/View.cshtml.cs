using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;
using TranslationProjectManager.Services;

namespace TranslationProjectManager.Pages.TranslationTasks
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public SelectList Translators { get; set; }

        public IEnumerable<TranslatorAssignmentTableView> Invites { get; set; }

        public IEnumerable<TranslationProjectFileTableView> TranslationTaskFiles { get; set; }

        public IEnumerable<CommentTableView> Comments { get; set; }
        
        public TaskViewModel Task { get; set; }

        [BindProperty]
        public InviteModel InviteInput { get; set; }

        [BindProperty]
        public FilesInputModel FilesInput { get; set; }

        [BindProperty]
        public CommentInputModel CommentInput { get; set; }
        
        public class TaskViewModel
        {
            [Display(Name = "Id")]
            public long Id { get; set; }

            [Display(Name = "Title")]
            public string Title { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            public TaskStatusEnum StatusEnum { get; set; }

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

            [Display(Name = "Deadline"), DataType(DataType.Date)]
            public DateTime Deadline { get; set; }

            [Display(Name = "Budget")]
            public decimal Budget { get; set; }

            [Display(Name = "Currency")]
            public string Currency { get; set; }

            public long ProjectId { get; set; }

            [Display(Name = "Project")]
            public string ProjectTitle { get; set; }
        }

        public class InviteModel
        {
            [Required, Display(Name = "Translator")]
            public string TranslatorId { get; set; }
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
            Translators = new SelectList(_context.Users.Select(u => new { Id = u.Id, Name = $"{u.FirstName} {u.LastName}" }), "Id", "Name");
        }

        public void OnGet()
        {
            var task = _context.TranslationTasks
                .Include(tt => tt.TranslationProject)
                .FirstOrDefault(tt => tt.Id == Id);

            Task = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = Data.TaskStatus.FromValue(task.Status).Title,
                StatusEnum = task.Status,
                SourceLanguage = task.SourceLanguage,
                TargetLanguage = task.TargetLanguage,
                Accuracy = Accuracy.FromValue(task.Accuracy).Title,
                Industry = Industry.FromValue(task.Industry).Title,
                Unit = Unit.FromValue(task.Unit).Title,
                Amount = task.Amount,
                ExpectedStart = task.ExpectedStart,
                Deadline = task.Deadline,
                Budget = task.Budget,
                Currency = task.Currency,
                ProjectId = task.TranslationProjectId,
                ProjectTitle = task.TranslationProject.Title
            };

            Invites = _context.TranslatorAssignments
                .Include(ta => ta.Translator)
                .Where(ta => ta.TranslationTaskId == Id)
                .Select(ta => new TranslatorAssignmentTableView()
                {
                    Id = ta.Id,
                    Status = TranslatorAssignmentStatus.FromValue(ta.Status),
                    Price = ta.Price,
                    Currency = ta.Currency,
                    TranslatorId = ta.TranslatorId,
                    TranslatorFullName = $"{ta.Translator.FirstName} {ta.Translator.LastName}",
                });

            TranslationTaskFiles = _context.TranslationTasks
                .Include(tt => tt.VisibleFiles)
                .Where(tt => tt.Id == Id)
                .SelectMany(tt => tt.VisibleFiles)
                .Select(tpf => new TranslationProjectFileTableView()
                {
                    Id = tpf.Id,
                    Name = tpf.File.Name,
                });

            Comments = _context.TranslationTaskComments
                .Include(tp => tp.Author)
                .Where(tp => tp.TranslationTaskId == Id)
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

        public async Task<IActionResult> OnGetAcceptOffer(long inviteId)
        {
            var invite = await _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefaultAsync(ta => ta.Id == inviteId);

            invite.Status = TranslatorAssignmentStatusEnum.Accepted;

            var remainingInvites = _context.TranslatorAssignments
                .Where(ta => ta.TranslationTaskId == invite.TranslationTaskId && ta.Id != inviteId);

            foreach (var remainingInvite in remainingInvites)
            {
                remainingInvite.Status = TranslatorAssignmentStatusEnum.InviteCancelled;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/TranslationTasks/View", new { id = Id });
        }

        public async Task<IActionResult> OnGetRejectOffer(long inviteId)
        {
            var invite = await _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefaultAsync(ta => ta.Id == inviteId);

            invite.Status = TranslatorAssignmentStatusEnum.OfferRejected;
            await _context.SaveChangesAsync();

            return RedirectToPage("/TranslationTasks/View", new { id = Id });
        }

        public async Task<IActionResult> OnGetCancelInvite(long inviteId)
        {
            var invite = await _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefaultAsync(ta => ta.Id == inviteId);

            invite.Status = TranslatorAssignmentStatusEnum.InviteCancelled;
            await _context.SaveChangesAsync();

            return RedirectToPage("/TranslationTasks/View", new { id = Id });
        }

        public IActionResult OnPostInvite()
        {
            var invite = new TranslatorAssignment()
            {
                Status = TranslatorAssignmentStatusEnum.Awaiting,
                TranslatorId = InviteInput.TranslatorId,
                TranslationTaskId = Id
            };

            _context.TranslatorAssignments.Add(invite);
            _context.SaveChanges();

            return RedirectToPage("/TranslationTasks/View", new { Id = Id });
        }

        public IActionResult OnPostMoveToStatus(TaskStatusEnum status)
        {
            var task = _context.TranslationTasks.Find(Id);
            task.Status = status;
            _context.SaveChanges();

            return RedirectToPage("/TranslationTasks/View", new { Id = Id });
        }
        public IActionResult OnPostAddComment()
        {
            var comment = new TranslationTaskComment
            {
                TranslationTaskId = Id,
                Comment = CommentInput.Comment,
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                AuthorId = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name).Id
            };

            _context.TranslationTaskComments.Add(comment);
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/View", new { Id = Id });
        }

        public async Task<IActionResult> OnPostUploadFiles()
        {
            var task = _context.TranslationTasks
                .Include(tt => tt.TranslationProject)
                .FirstOrDefault(tt => tt.Id == Id);

            var project = task.TranslationProject;

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
                        TranslationProject = project,
                        VisibleIn = new List<TranslationTask>() { task }
                    };

                    _context.Files.Add(fileDescriptor);
                    _context.TranslationProjectFiles.Add(projectFile);

                    _context.SaveChanges();
                }
            }

            return RedirectToPage("/TranslationTasks/View", new { Id = Id });
        }
    }

    public class TranslatorAssignmentTableView
    {
        public long Id { get; set; }
        public TranslatorAssignmentStatus Status { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }

        public string TranslatorId { get; set; }
        public string TranslatorFullName { get; set; }
    }

    public class TranslationProjectFileTableView
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class CommentTableView
    {
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public string AuthorId { get; set; }
        public string AuthorFullName { get; set; }
    }
}
