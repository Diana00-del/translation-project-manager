using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslationProjects
{
    public class CreateModel : PageModel
    {
        public SelectList Accuracies { get; set; }
        public SelectList Industries { get; set; }
        public SelectList Units { get; set; }
        public SelectList Managers { get; set; }
        public SelectList Clients { get; set; }


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

            [Required, Display(Name = "Internal Deadline"), DataType(DataType.Date)]
            public DateTime InternalDeadline { get; set; }

            [Required, Display(Name = "External Deadline"), DataType(DataType.Date)]
            public DateTime ExternalDeadline { get; set; }

            [Required, Display(Name = "Budget")]
            public decimal Budget { get; set; }

            [Required, Display(Name = "Currency")]
            public string Currency { get; set; }

            [Required, Display(Name = "Manager")]
            public string ManagerId { get; set; }

            [Required, Display(Name = "Client")]
            public long ClientId { get; set; }
        }

        private readonly TranslationProjectManagerContext _context;

        public CreateModel(TranslationProjectManagerContext context)
        {
            _context = context;
            Accuracies = new SelectList(Accuracy.All, "Value", "Title");
            Industries = new SelectList(Industry.All, "Value", "Title");
            Units = new SelectList(Unit.All, "Value", "Title");
            Managers = new SelectList(_context.Users.Select(u => new { Id = u.Id, Name = $"{u.FirstName} {u.LastName}" }), "Id", "Name");
            Clients = new SelectList(_context.Clients.Select(c => new { Id = c.Id, Name = c.Name }), "Id", "Name");
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var project = new TranslationProject()
            {
                Status = ProjectStatusEnum.Draft,
                Title = Input.Title,
                Description = Input.Description,
                SourceLanguage = Input.SourceLanguage,
                TargetLanguage = Input.TargetLanguage,
                Accuracy = Input.Accuracy,
                Industry = Input.Industry,
                Unit = Input.Unit,
                Amount = Input.Amount,
                ExpectedStart = DateTime.SpecifyKind(Input.ExpectedStart, DateTimeKind.Utc),
                InternalDeadline = DateTime.SpecifyKind(Input.InternalDeadline, DateTimeKind.Utc),
                ExternalDeadline = DateTime.SpecifyKind(Input.ExternalDeadline, DateTimeKind.Utc),
                Budget = Input.Budget,
                Currency = Input.Currency,
                ManagerId = Input.ManagerId,
                ClientId = Input.ClientId
            };

            _context.TranslationProjects.Add(project);
            _context.SaveChanges();

            return RedirectToPage("/TranslationProjects/Index");
        }
    }
}
