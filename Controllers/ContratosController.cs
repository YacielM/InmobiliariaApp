using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaApp.Models;
using InmobiliariaApp.Data.Repositorios;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaApp.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContratos repo;
        private readonly RepositorioInmuebles repoInmuebles;
        private readonly RepositorioInquilinos repoInquilinos;

        public ContratosController(IConfiguration configuration)
        {
            repo = new RepositorioContratos();
            repoInmuebles = new RepositorioInmuebles(configuration);
            repoInquilinos = new RepositorioInquilinos(configuration);
        }

        // GET: /Contratos
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: /Contratos/Details/5
        public IActionResult Details(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            return View(contrato);
        }

        // GET: /Contratos/Create
        public IActionResult Create()
        {
            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion");
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto");
            return View();
        }

        // POST: /Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto", contrato.IdInquilino);
            return View(contrato);
        }

        // GET: /Contratos/Edit/5
        public IActionResult Edit(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto", contrato.IdInquilino);
            return View(contrato);
        }

        // POST: /Contratos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repo.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto", contrato.IdInquilino);
            return View(contrato);
        }

        // GET: /Contratos/Delete/5
        public IActionResult Delete(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            return View(contrato);
        }

        // POST: /Contratos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}