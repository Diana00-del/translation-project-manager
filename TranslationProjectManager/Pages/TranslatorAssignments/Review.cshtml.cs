using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Pages.TranslatorAssignments
{
    public class ReviewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public IEnumerable<TranslationProjectFileTableView> TranslationTaskFiles { get; set; }

        public InviteViewModel Invite { get; set; }

        [BindProperty]
        public OfferInputModel OfferInput { get; set; }

        public class InviteViewModel
        {
            [Display(Name = "Id")]
            public long Id { get; set; }

            [Display(Name = "Title")]
            public string Title { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

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
        }

        public class OfferInputModel
        {
            [Required, Display(Name = "Price")]
            public decimal Price { get; set; }

            [Required, Display(Name = "Currency")]
            public string Currency { get; set; }
        }

        private readonly TranslationProjectManagerContext _context;

        public ReviewModel(TranslationProjectManagerContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            var invite = _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefault(ta => ta.Id == Id);

            Invite = new InviteViewModel
            {
                Id = invite.Id,
                Title = invite.TranslationTask.Title,
                Description = invite.TranslationTask.Description,
                SourceLanguage = invite.TranslationTask.SourceLanguage,
                TargetLanguage = invite.TranslationTask.TargetLanguage,
                Accuracy = Accuracy.FromValue(invite.TranslationTask.Accuracy).Title,
                Industry = Industry.FromValue(invite.TranslationTask.Industry).Title,
                Unit = Unit.FromValue(invite.TranslationTask.Unit).Title,
                Amount = invite.TranslationTask.Amount,
                ExpectedStart = invite.TranslationTask.ExpectedStart,
                Deadline = invite.TranslationTask.Deadline
            };

            OfferInput = new OfferInputModel
            {
                Price = invite.Price ?? 0m,
                Currency = invite.Currency
            };

            TranslationTaskFiles = _context.TranslatorAssignments
                .Include(tt => tt.TranslationTask)
                .Where(tt => tt.Id == Id)
                .SelectMany(tt => tt.TranslationTask.VisibleFiles)
                .Select(tpf => new TranslationProjectFileTableView()
                {
                    Id = tpf.Id,
                    Name = tpf.File.Name,
                });
        }

        public IActionResult OnPostAccept()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var invite = _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefault(ta => ta.Id == Id);

            invite.Status = TranslatorAssignmentStatusEnum.OfferSent;
            invite.Price = OfferInput.Price;
            invite.Currency = OfferInput.Currency;

            _context.Update(invite);
            _context.SaveChanges();

            return RedirectToPage("/TranslatorAssignments/Index");
        }

        public IActionResult OnPostReject()
        {
            var invite = _context.TranslatorAssignments
                .Include(ta => ta.TranslationTask)
                .FirstOrDefault(ta => ta.Id == Id);

            invite.Status = TranslatorAssignmentStatusEnum.InviteRejected;

            _context.Update(invite);
            _context.SaveChanges();

            return RedirectToPage("/TranslatorAssignments/Index");
        }
    }

    public class TranslationProjectFileTableView
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
