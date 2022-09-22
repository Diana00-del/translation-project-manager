using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TranslationProjectManager.Data;
using TranslationProjectManager.Models.Clients;
using TranslationProjectManager.Services;

namespace TranslationProjectManager.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        public IActionResult Index()
        {
            var model = new IndexModel()
            {
                Clients = clientService.GetAll().OrderBy(c => c.Id).ToList()
            };
            
            return View(model);
        }

        public IActionResult View(int id)
        {
            var client = clientService.Get(id);
            var model = new ViewModel()
            {
                Id = client.Id,
                Type = client.Type,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
                City = client.City,
                State = client.State,
                Zip = client.Zip,
                Country = client.Country,
                VAT = client.VAT,
                ClientTypes = new SelectList(ClientType.All, "Value", "Title")
            };
            return View(model);
        }
        
        public IActionResult Create()
        {
            var model = new CreateModel()
            {
                ClientTypes = new SelectList(ClientType.All, "Value", "Title")
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ClientTypes = new SelectList(ClientType.All, "Value", "Title");
                return View(model);
            }

            var client = new Client
            {
                Type = model.Type,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Country = model.Country,
                VAT = model.VAT is null ? "" : model.VAT.Trim()
            };

            clientService.Create(client);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(long id)
        {
            var client = clientService.Get(id);
            
            var model = new EditModel()
            {
                Id = client.Id,
                Type = client.Type,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
                City = client.City,
                State = client.State,
                Zip = client.Zip,
                Country = client.Country,
                VAT = client.VAT,
                ClientTypes = new SelectList(ClientType.All, "Value", "Title")
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(long id, EditModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ClientTypes = new SelectList(ClientType.All, "Value", "Title");
                return View(model);
            }

            var client = new Client
            {
                Id = id,
                Type = model.Type,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Country = model.Country,
                VAT = model.VAT is null ? "" : model.VAT.Trim()
            };

            clientService.Update(client);

            return RedirectToAction("Index");
        }
    }
}



