using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslationTasks
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

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

        public EditModel(TranslationProjectManagerContext context)
        {
            _context = context;
            Accuracies = new SelectList(Accuracy.All, "Value", "Title");
            Industries = new SelectList(Industry.All, "Value", "Title");
            Units = new SelectList(Unit.All, "Value", "Title");
        }

        public void OnGet()
        {
            var task = _context.TranslationTasks.Find(Id);

            Input = new InputModel
            {
                Title = task.Title,
                Description = task.Description,
                SourceLanguage = task.SourceLanguage,
                TargetLanguage = task.TargetLanguage,
                Accuracy = task.Accuracy,
                Industry = task.Industry,
                Unit = task.Unit,
                Amount = task.Amount,
                ExpectedStart = task.ExpectedStart,
                Deadline = task.Deadline,
                Budget = task.Budget,
                Currency = task.Currency
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var task = _context.TranslationTasks.Find(Id);

            task.Title = Input.Title;
            task.Description = Input.Description;
            task.SourceLanguage = Input.SourceLanguage;
            task.TargetLanguage = Input.TargetLanguage;
            task.Accuracy = Input.Accuracy;
            task.Industry = Input.Industry;
            task.Unit = Input.Unit;
            task.Amount = Input.Amount;
            task.ExpectedStart = DateTime.SpecifyKind(Input.ExpectedStart, DateTimeKind.Utc);
            task.Deadline = DateTime.SpecifyKind(Input.Deadline, DateTimeKind.Utc);
            task.Budget = Input.Budget;
            task.Currency = Input.Currency;

            _context.TranslationTasks.Update(task);
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/Index", new { Id = task.TranslationProjectId });
        }
    }
}
