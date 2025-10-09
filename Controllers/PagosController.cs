using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaApp.Models;
using InmobiliariaApp.Data.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly RepositorioPagos repoPagos;
        private readonly RepositorioContratos repoContratos;

        public PagosController(IConfiguration configuration)
        {
            repoPagos = new RepositorioPagos(configuration);
            repoContratos = new RepositorioContratos(configuration);
        }

        // GET: /Pagos
        public IActionResult Index()
        {
            var lista = repoPagos.ObtenerTodos();
            return View(lista);
        }

        // GET: /Pagos/Details/5
        public IActionResult Details(int id)
        {
            var pago = repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();
            return View(pago);
        }

        // GET: /Pagos/Create
        public IActionResult Create(int? idContrato = null)
        {
            CargarContratos(idContrato);

            var pago = new Pago();

            if (idContrato.HasValue)
            {
                pago.IdContrato = idContrato.Value;
                var contrato = repoContratos.ObtenerPorId(idContrato.Value);
                if (contrato != null)
                {
                    pago.Importe = contrato.MontoMensual; // precarga importe
                    pago.Fecha = DateTime.Today;          // sugerencia de fecha
                }
            }

            return View(pago);
        }

        // POST: /Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (pago.Importe <= 0)
            {
                ModelState.AddModelError("Importe", "El importe debe ser mayor a 0.");
            }

            if (!ModelState.IsValid)
            {
                CargarContratos(pago.IdContrato);
                return View(pago);
            }

            repoPagos.Alta(pago);
            TempData["Msg"] = "Pago registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Pagos/Edit/5
        public IActionResult Edit(int id)
        {
            var pago = repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();
            CargarContratos(pago.IdContrato);
            return View(pago);
        }

        // POST: /Pagos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pago pago)
        {
            if (pago.Importe <= 0)
            {
                ModelState.AddModelError("Importe", "El importe debe ser mayor a 0.");
            }

            if (!ModelState.IsValid)
            {
                CargarContratos(pago.IdContrato);
                return View(pago);
            }

            repoPagos.Modificacion(pago);
            TempData["Msg"] = "Pago actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Pagos/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var pago = repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();
            return View(pago);
        }

        // POST: /Pagos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            repoPagos.Baja(id);
            TempData["Msg"] = "Pago eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Método privado para cargar contratos en el combo
        private void CargarContratos(int? idSeleccionado = null)
        {
            var contratos = repoContratos.ObtenerTodos()
                .Select(c => new
                {
                    c.IdContrato,
                    Texto = $"Contrato #{c.IdContrato} - Inmueble {c.Inmueble?.Direccion} - Inquilino {c.Inquilino?.NombreCompleto}"
                });

            ViewBag.Contratos = new SelectList(contratos, "IdContrato", "Texto", idSeleccionado);
        }

        public IActionResult PorContrato(int idContrato)
        {
            var lista = repoPagos.ObtenerPorContrato(idContrato);
            ViewBag.IdContrato = idContrato;
            return View("Index", lista);
        }
    }
}
