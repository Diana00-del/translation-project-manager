using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Models.Clients
{
    public class ViewModel
    {
        public long Id { get; set; }
        public ClientTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string? VAT { get; set; }

        public SelectList ClientTypes { get; set; }

    }
}
