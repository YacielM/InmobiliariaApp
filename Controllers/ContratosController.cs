using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaApp.Models;
using InmobiliariaApp.Data.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly RepositorioContratos repo;
        private readonly RepositorioInmuebles repoInmuebles;
        private readonly RepositorioInquilinos repoInquilinos;
        private readonly RepositorioPagos repoPagos;

        public ContratosController(IConfiguration configuration)
        {
            repo = new RepositorioContratos(configuration);
            repoInmuebles = new RepositorioInmuebles(configuration);
            repoInquilinos = new RepositorioInquilinos(configuration);
            repoPagos = new RepositorioPagos(configuration); 
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
            if (repo.ExisteSuperposicion(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin))
            {
                ModelState.AddModelError("", "Ya existe un contrato que se superpone en esas fechas para este inmueble.");
            }
            if (ModelState.IsValid)
            {
                contrato.CreadoPor = User.Identity?.Name ?? "Sistema";
                if (string.IsNullOrEmpty(contrato.CreadoPor))
                {
                    contrato.CreadoPor = "Sistema";
                }
                repo.Alta(contrato);
                TempData["Msg"] = "Contrato creado correctamente.";
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
            if (repo.ExisteSuperposicion(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin, contrato.IdContrato))
            {
                ModelState.AddModelError("", "Ya existe un contrato que se superpone en esas fechas para este inmueble.");
            }

            if (ModelState.IsValid)
            {
                repo.Modificacion(contrato);
                TempData["Msg"] = "Contrato actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto", contrato.IdInquilino);
            return View(contrato);
        }


        // GET: /Contratos/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            return View(contrato);
        }

        // POST: /Contratos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Si el contrato tiene pagos, no permitir borrarlo
            var pagosDelContrato = repoPagos.ObtenerPorContrato(id); // ya lo tenés
            if (pagosDelContrato.Any())
            {
                TempData["Msg"] = "No se puede borrar: el contrato tiene pagos asociados.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                repo.Baja(id);
                TempData["Msg"] = "Contrato eliminado correctamente.";
            }
            catch (Exception)
            {
                TempData["Msg"] = "No se pudo eliminar el contrato por una restricción de base de datos.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Contratos/PorInmueble/5
        public IActionResult PorInmueble(int id)
        {
            var lista = repo.ObtenerPorInmueble(id);
            ViewBag.Inmueble = repoInmuebles.ObtenerPorId(id);
            return View(lista);
        }


        // GET: /Contratos/Renovar/5
        public IActionResult Renovar(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            if (contrato.FechaFin > DateTime.Today)
            {
                TempData["Msg"] = "No se puede renovar un contrato que aún está vigente.";
                return RedirectToAction(nameof(Index));
            }

            // precarga datos
            var nuevo = new Contrato
            {
                IdInmueble = contrato.IdInmueble,
                IdInquilino = contrato.IdInquilino,
                MontoMensual = contrato.MontoMensual,
                FechaInicio = contrato.FechaFin.AddDays(1),
                FechaFin = contrato.FechaFin.AddYears(1)
            };

            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerTodos(), "IdInmueble", "Direccion", nuevo.IdInmueble);
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerTodos(), "IdInquilino", "NombreCompleto", nuevo.IdInquilino);

            return View("Create", nuevo);
        }

        // GET: /Contratos/TerminarAnticipado/5
        public IActionResult TerminarAnticipado(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            ViewBag.Contrato = contrato;
            ViewBag.SugeridoMulta = Math.Round(contrato.MontoMensual * 0.5m, 2); // ejemplo: 50% de un mes
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TerminarAnticipadoConfirmado(int id, decimal multa)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            if (contrato.FechaFin <= DateTime.Today)
            {
                TempData["Msg"] = "El contrato ya finalizó, no puede terminarse anticipadamente.";
                return RedirectToAction(nameof(Index));
            }

            var fechaCorte = DateTime.Today;
            var usuario = User.Identity?.Name ?? "Sistema";

            //Aca pasamos quien lo terminó
            repo.TerminarAnticipado(id, fechaCorte, multa, usuario);

            // Registrar pago de multa
            var pagoMulta = new Pago
            {
                IdContrato = id,
                Fecha = fechaCorte,
                Importe = multa,
                CreadoPor = usuario // aca guarda quien registro el pago
            };
            repoPagos.Alta(pagoMulta);

            TempData["Msg"] = "Contrato terminado anticipadamente. Multa registrada como pago.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Contratos/PagarMulta/5
        public IActionResult PagarMulta(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            // Validaciones de negocio
            if (!contrato.FechaTerminacionAnticipada.HasValue || !(contrato.MultaTerminacion.HasValue && contrato.MultaTerminacion.Value > 0))
            {
                TempData["Msg"] = "Este contrato no tiene multa pendiente.";
                return RedirectToAction(nameof(Index));
            }

            if (contrato.MultaPagada)
            {
                TempData["Msg"] = "La multa ya fue abonada.";
                return RedirectToAction(nameof(Index));
            }

            // Registrar pago de multa
            var pagoMulta = new Pago
            {
                IdContrato = id,
                Fecha = DateTime.Today,
                Importe = contrato.MultaTerminacion.Value
            };
            repoPagos.Alta(pagoMulta);

            // Marcar como pagada
            repo.MarcarMultaPagada(id);

            TempData["Msg"] = "Multa abonada correctamente y registrada como pago.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Vigentes()
        {
            var lista = repo.ObtenerVigentes();
            return View(lista);
        }

    }
}
