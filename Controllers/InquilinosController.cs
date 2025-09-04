using InmobiliariaApp.Models;
 using InmobiliariaApp.Data.Repositorios;
 using Microsoft.AspNetCore.Mvc;
 namespace InmobiliariaApp.Controllers
 {
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilinos _repositorioInquilinos;
        public InquilinosController(RepositorioInquilinos repositorioInquilinos)
        {
            _repositorioInquilinos = repositorioInquilinos;
        }
        // GET: Inquilinos
        public IActionResult Index()
        {
            var inquilinos = _repositorioInquilinos.GetAll();
            return View(inquilinos);
        }
        // GET: Inquilinos/Details/5
        public IActionResult Details(int id)
        {
            var inquilino = _repositorioInquilinos.GetById(id);
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
                _repositorioInquilinos.Insert(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }
        // GET: Inquilinos/Edit/5
        public IActionResult Edit(int id)
        {
            var inquilino = _repositorioInquilinos.GetById(id);
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
                _repositorioInquilinos.Update(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }
        // GET: Inquilinos/Delete/5
        public IActionResult Delete(int id)
        {
            var inquilino = _repositorioInquilinos.GetById(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }
        // POST: Inquilinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorioInquilinos.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
 }