namespace TranslationProjectManager.Data;

public class Client
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
    public string VAT { get; set; }

    // Wszystkie projekty tego klienta
    public virtual ICollection<TranslationProject> TranslationProjects { get; set; }
}
