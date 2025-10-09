using InmobiliariaApp.Models;
using InmobiliariaApp.Data.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilinos _repositorioInquilinos;

        public InquilinosController(IConfiguration configuration)
        {
            _repositorioInquilinos = new RepositorioInquilinos(configuration);
        }

        // GET: Inquilinos
        public IActionResult Index()
        {
            var inquilinos = _repositorioInquilinos.ObtenerTodos();
            return View(inquilinos);
        }

        // GET: Inquilinos/Details/5
        public IActionResult Details(int id)
        {
            var inquilino = _repositorioInquilinos.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                _repositorioInquilinos.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Edit/5
        public IActionResult Edit(int id)
        {
            var inquilino = _repositorioInquilinos.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.IdInquilino)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _repositorioInquilinos.Modificacion(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var inquilino = _repositorioInquilinos.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorioInquilinos.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}