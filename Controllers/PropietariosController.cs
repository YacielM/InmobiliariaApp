using InmobiliariaApp.Models;
 using InmobiliariaApp.Data.Repositorios;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace InmobiliariaApp.Controllers
 {
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietarios _repositorioPropietarios;
        public PropietariosController(RepositorioPropietarios repositorioPropietarios)
        {
            _repositorioPropietarios = repositorioPropietarios;
        }
        // GET: Propietarios
        public IActionResult Index()
        {
            var propietarios = _repositorioPropietarios.GetAll();
            return View(propietarios);
        }
        // GET: Propietarios/Details/5
        public IActionResult Details(int id)
        {
            var propietario = _repositorioPropietarios.GetById(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }
        // GET: Propietarios/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                _repositorioPropietarios.Insert(propietario);
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }
        // GET: Propietarios/Edit/5
        public IActionResult Edit(int id)
        {
            var propietario = _repositorioPropietarios.GetById(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }
        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Propietario propietario)
        {
            if (id != propietario.IdPropietario)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _repositorioPropietarios.Update(propietario);
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }
        // GET: Propietarios/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var propietario = _repositorioPropietarios.GetById(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }
        // POST: Propietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorioPropietarios.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
 }