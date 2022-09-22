using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslationTasks
{
    public class CreateModel : PageModel
    {
        [FromQuery(Name = "projectId")]
        public long ProjectId { get; set; }

        public SelectList Accuracies { get; set; }
        public SelectList Industries { get; set; }
        public SelectList Units { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "Title")]
            public string Title { get; set; }

            [Required, Display(Name = "Description"), DataType(DataType.MultilineText)]
            public string Description { get; set; }

            [Required, Display(Name = "Source Language")]
            public string SourceLanguage { get; set; }

            [Required, Display(Name = "Target Language")]
            public string TargetLanguage { get; set; }

            [Required, Display(Name = "Accuracy")]
            public AccuracyEnum Accuracy { get; set; }

            [Required, Display(Name = "Industry")]
            public IndustryEnum Industry { get; set; }

            [Required, Display(Name = "Unit")]
            public UnitEnum Unit { get; set; }

            [Required, Display(Name = "Amount")]
            public long Amount { get; set; }

            [Required, Display(Name = "Expected Start Date"), DataType(DataType.Date)]
            public DateTime ExpectedStart { get; set; }

            [Required, Display(Name = "Deadline"), DataType(DataType.Date)]
            public DateTime Deadline { get; set; }

            [Required, Display(Name = "Budget")]
            public decimal Budget { get; set; }

            [Required, Display(Name = "Currency")]
            public string Currency { get; set; }
        }

        private readonly TranslationProjectManagerContext _context;

        public CreateModel(TranslationProjectManagerContext context)
        {
            _context = context;
            Accuracies = new SelectList(Accuracy.All, "Value", "Title");
            Industries = new SelectList(Industry.All, "Value", "Title");
            Units = new SelectList(Unit.All, "Value", "Title");
        }

        public void OnGet()
        {
            var project = _context.TranslationProjects.Find(ProjectId);

            Input = new InputModel()
            {
                Title = project.Title,
                Description = project.Description,
                SourceLanguage = project.SourceLanguage,
                TargetLanguage = project.TargetLanguage,
                Accuracy = project.Accuracy,
                Industry = project.Industry,
                Unit = project.Unit,
                Amount = project.Amount,
                ExpectedStart = project.ExpectedStart,
                Deadline = project.InternalDeadline,
                Budget = project.Budget,
                Currency = project.Currency
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var task = new TranslationTask()
            {
                Title = Input.Title,
                Status = TaskStatusEnum.ReadyToStart,
                Description = Input.Description,
                SourceLanguage = Input.SourceLanguage,
                TargetLanguage = Input.TargetLanguage,
                Accuracy = Input.Accuracy,
                Industry = Input.Industry,
                Unit = Input.Unit,
                Amount = Input.Amount,
                ExpectedStart = DateTime.SpecifyKind(Input.ExpectedStart, DateTimeKind.Utc),
                Deadline = DateTime.SpecifyKind(Input.Deadline, DateTimeKind.Utc),
                Budget = Input.Budget,
                Currency = Input.Currency,
                TranslationProjectId = ProjectId
            };

            _context.TranslationTasks.Add(task);
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/View", new { Id = ProjectId });
        }
    }
}
