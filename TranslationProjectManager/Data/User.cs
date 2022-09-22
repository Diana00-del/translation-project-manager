using Microsoft.AspNetCore.Identity;

namespace TranslationProjectManager.Data;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public virtual ICollection<TranslatorAssignment> TranslatorAssignments { get; set; }
    public virtual ICollection<TranslationProject> TranslationProjects { get; set; }
    public virtual ICollection<TranslationProjectComment> TranslationProjectComments { get; set; }
    public virtual ICollection<TranslationTaskComment> TranslationTaskComments { get; set; }
}

