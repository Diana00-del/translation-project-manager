using TranslationProjectManager.Data;

namespace TranslationProjectManager.Services;

public interface IClientService
{
    IEnumerable<Client> GetAll();
    Client Get(long id);
    Client Create(Client client);
    Client Update(Client client);
}

public class ClientService : IClientService
{
    private readonly TranslationProjectManagerContext context;

    public ClientService(TranslationProjectManagerContext context)
    {
        this.context = context;
    }

    public Client Create(Client client)
    {
        context.Clients.Add(client);
        context.SaveChanges();
        return client;
    }

    public Client Get(long id)
    {
        return context.Clients.Find(id);
    }

    public IEnumerable<Client> GetAll()
    {
        return context.Clients;
    }

    public Client Update(Client client)
    {
        context.Clients.Update(client);
        context.SaveChanges();
        return client;
    }
}
