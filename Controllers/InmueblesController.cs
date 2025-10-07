using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaApp.Models;
using InmobiliariaApp.Data.Repositorios;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmuebles repoInmuebles;
        private readonly RepositorioPropietarios repoPropietarios;

        public InmueblesController(IConfiguration configuration)
        {
            repoInmuebles = new RepositorioInmuebles(configuration);
            repoPropietarios = new RepositorioPropietarios(configuration);
        }

        // GET: /Inmuebles
        public IActionResult Index()
        {
            var lista = repoInmuebles.ObtenerTodos();
            return View(lista);
        }

        // GET: /Inmuebles/Details/5
        public IActionResult Details(int id)
        {
            var inmueble = repoInmuebles.ObtenerPorId(id);
            if (inmueble == null) return NotFound();
            return View(inmueble);
        }

        // GET: /Inmuebles/Create
        public IActionResult Create()
        {
            CargarComboPropietarios();
            CargarTiposInmueble();
            return View();
        }

        // POST: /Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error de ModelState: " + e.ErrorMessage);
                }

                ModelState.AddModelError(string.Empty, "Hay errores en el formulario. Revisá los campos.");
                CargarComboPropietarios(inmueble.IdPropietario);
                CargarTiposInmueble(inmueble.Tipo);
                return View(inmueble);
            }

            try
            {
                var nuevoId = repoInmuebles.Alta(inmueble);
                if (nuevoId <= 0)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo guardar el inmueble en la base de datos.");
                    CargarComboPropietarios(inmueble.IdPropietario);
                    CargarTiposInmueble(inmueble.Tipo);
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                CargarComboPropietarios(inmueble.IdPropietario);
                CargarTiposInmueble(inmueble.Tipo);
                return View(inmueble);
            }

            TempData["Msg"] = "Inmueble creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Inmuebles/Edit/5
        public IActionResult Edit(int id)
        {
            var inmueble = repoInmuebles.ObtenerPorId(id);
            if (inmueble == null) return NotFound();

            CargarComboPropietarios(inmueble.IdPropietario);
            CargarTiposInmueble(inmueble.Tipo);
            return View(inmueble);
        }

        // POST: /Inmuebles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error de ModelState: " + e.ErrorMessage);
                }

                ModelState.AddModelError(string.Empty, "Hay errores en el formulario. Revisá los campos.");
                CargarComboPropietarios(inmueble.IdPropietario);
                CargarTiposInmueble(inmueble.Tipo);
                return View(inmueble);
            }

            try
            {
                var filas = repoInmuebles.Modificacion(inmueble);
                if (filas <= 0)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el inmueble.");
                    CargarComboPropietarios(inmueble.IdPropietario);
                    CargarTiposInmueble(inmueble.Tipo);
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al actualizar: {ex.Message}");
                CargarComboPropietarios(inmueble.IdPropietario);
                CargarTiposInmueble(inmueble.Tipo);
                return View(inmueble);
            }

            TempData["Msg"] = "Inmueble actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Inmuebles/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var inmueble = repoInmuebles.ObtenerPorId(id);
            if (inmueble == null) return NotFound();
            return View(inmueble);
        }

        // POST: /Inmuebles/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var filas = repoInmuebles.Baja(id);
                if (filas <= 0)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo eliminar el inmueble.");
                    var inmueble = repoInmuebles.ObtenerPorId(id);
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al eliminar: {ex.Message}");
                var inmueble = repoInmuebles.ObtenerPorId(id);
                return View(inmueble);
            }

            TempData["Msg"] = "Inmueble eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Combo propietarios: Apellido + Nombre
        private void CargarComboPropietarios(int? idSeleccionado = null)
        {
            var propietarios = repoPropietarios.GetAll()
                .Select(p => new
                {
                    p.IdPropietario,
                    NombreCompleto = p.Apellido + " " + p.Nombre
                });

            ViewBag.Propietarios = new SelectList(
                propietarios,
                "IdPropietario",
                "NombreCompleto",
                idSeleccionado
            );
        }

        // Combo tipos de inmueble (enum/diccionario simple)
        private void CargarTiposInmueble(string? tipoSeleccionado = null)
        {
            var tipos = new[]
            {
                new { Valor = "Casa", Texto = "Casa" },
                new { Valor = "Departamento", Texto = "Departamento" },
                new { Valor = "Duplex", Texto = "Dúplex" },
                new { Valor = "Monoambiente", Texto = "Monoambiente" },
                new { Valor = "PH", Texto = "PH" },
            };

            ViewBag.Tipos = new SelectList(tipos, "Valor", "Texto", tipoSeleccionado);
        }
    }
}
