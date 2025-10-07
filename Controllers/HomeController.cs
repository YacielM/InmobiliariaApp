using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using InmobiliariaApp.Data.Repositorios;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositorioUsuarios repoUsuarios;

        public HomeController(RepositorioUsuarios repoUsuarios)
        {
            this.repoUsuarios = repoUsuarios;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string clave, string? returnUrl = null)
        {
            var usuario = repoUsuarios.ObtenerPorEmail(email);

            if (usuario == null || usuario.Clave != clave) // ⚠️ en real: comparar hash
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email), // email
                new Claim("FullName", usuario.Nombre + " " + usuario.Apellido), // 👈 nombre completo
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Restringido()
        {
            return View();
        }

        [Authorize]
        public IActionResult EditarPerfil()
        {
            var idClaim = User.FindFirst("IdUsuario");
            if (idClaim == null)
            {
                return Unauthorized();
            }
            var id = int.Parse(idClaim.Value);
            var usuario = repoUsuarios.ObtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditarPerfil(Usuario model, IFormFile? avatarFile)
        {
            var idClaim = User.FindFirst("IdUsuario");
            if (idClaim == null)
            {
                return Unauthorized();
            }
            var id = int.Parse(idClaim.Value);
            var usuario = repoUsuarios.ObtenerPorId(id);
            if (usuario == null) return NotFound();

            if (!string.IsNullOrEmpty(model.Clave))
                usuario.Clave = model.Clave;

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", avatarFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    avatarFile.CopyTo(stream);
                }
                usuario.Avatar = "/avatars/" + avatarFile.FileName;
            }

            if (model.Avatar == "REMOVE")
                usuario.Avatar = null;

            repoUsuarios.Modificacion(usuario);
            TempData["Msg"] = "Perfil actualizado.";
            return RedirectToAction("EditarPerfil");
        }
    }
}
