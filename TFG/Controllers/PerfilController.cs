using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TFG.Models.ViewModels;
using TFG.Models;
using Microsoft.AspNetCore.Authorization;

namespace TFG.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PerfilController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PerfilController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            List<PerfilListViewModel> model = new List<PerfilListViewModel>();
            model = _roleManager.Roles.Select(r => new
            {
                Id = r.Id,
                Nombre = r.Name,
                NumeroUsuario = r.Users.Count
            }).ToList()
            .Select(r => new PerfilListViewModel
            {
                Id = r.Id,
                Nombre = r.Nombre,
                NumeroUsuario = r.NumeroUsuario
            }).ToList();
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CrearEditarPerfil(string id)
        {
            PerfilViewModel model = new PerfilViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
                if (identityRole != null)
                {
                    model.Id = identityRole.Id;
                    model.PerfilNombre = identityRole.Name;                    
                }
            }
            return View(model);
            //return PartialView("_AddEditApplicationRole", model);
        }
        [HttpPost]
        public async Task<IActionResult> CrearEditarPerfil(string id,PerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(id))
                {
                    IdentityRole applicationRole = new IdentityRole();
                    applicationRole.Name = model.PerfilNombre;
                    IdentityResult perfilResultado = await _roleManager.CreateAsync(applicationRole);
                    if (perfilResultado.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    IdentityRole applicationRole = await _roleManager.FindByIdAsync(id);
                    applicationRole.Name = model.PerfilNombre;
                    IdentityResult perfilResultado = await _roleManager.UpdateAsync(applicationRole);
                    if (perfilResultado.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarPerfil(string[] selBorrar)
        {
            foreach (string item in selBorrar)
            {
                IdentityRole applicationRole = await _roleManager.FindByIdAsync(item);
                if (applicationRole != null)
                {
                    IdentityResult roleRuslt = _roleManager.DeleteAsync(applicationRole).Result;
                }
            }
            return RedirectToAction("Index");
        }

    }
}