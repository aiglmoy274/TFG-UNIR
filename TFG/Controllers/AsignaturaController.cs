using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TFG.Data;
using TFG.Models;
using Microsoft.AspNetCore.Authorization;

namespace TFG.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AsignaturaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignaturaController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Asignaturas
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var lstComunidadAutonoma = _context.ComunidadAutonoma.OrderBy(x => x.Descripcion);
            ViewBag.ComunidadAutonomaId = new SelectList(lstComunidadAutonoma, "ComunidadAutonomaId", "Descripcion");                                
            if (lstComunidadAutonoma != null)
            {
                var comunidadAutonoma = lstComunidadAutonoma.FirstOrDefault();
                var lstPlanesEstudioComunidadAutonoma = await _context.PlanEstudio.Where(p => p.ComunidadAutonomaId == comunidadAutonoma.ComunidadAutonomaId).OrderBy(p=>p.Descripcion).ToListAsync();
                var planEstudio = lstPlanesEstudioComunidadAutonoma.FirstOrDefault();
                ViewBag.PlanEstudioId = new SelectList(lstPlanesEstudioComunidadAutonoma, "Id", "Descripcion",planEstudio.Id);
                var lstCursosEscolaresNivelEducativo = await _context.CursoEscolar.Where(p => p.NivelEducativo.Id == planEstudio.NivelEducativoId).OrderBy(p => p.Descripcion).ToListAsync();
                ViewBag.CursoEscolarId = new SelectList(lstCursosEscolaresNivelEducativo, "Id","Descripcion");

            }
            
            return View();
        }
        

        
        // POST: Asignaturas
        [HttpPost]
        public async Task<IActionResult> Index(int ComunidadAutonomaId, int? PlanEstudioId, int? CursoEscolarId)
        {
            try
            {
                ComunidadAutonoma comunidadAutonoma = await _context.ComunidadAutonoma.SingleOrDefaultAsync(m => m.ComunidadAutonomaId == ComunidadAutonomaId);
                ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion", comunidadAutonoma.ComunidadAutonomaId);
                var lstPlanesEstudioComunidadAutonoma = await _context.PlanEstudio.Where(p => p.ComunidadAutonomaId == comunidadAutonoma.ComunidadAutonomaId).OrderBy(p => p.Descripcion).ToListAsync();
                ViewBag.PlanEstudioId = new SelectList(lstPlanesEstudioComunidadAutonoma, "Id", "Descripcion", PlanEstudioId);
                PlanEstudio planEstudio = await _context.PlanEstudio.SingleOrDefaultAsync(m => m.Id == PlanEstudioId);
                var lstCursosEscolaresNivelEducativo = await _context.CursoEscolar.Where(p => p.NivelEducativo.Id == planEstudio.NivelEducativoId).OrderBy(p => p.Descripcion).ToListAsync();
                ViewBag.CursoEscolarId = new SelectList(lstCursosEscolaresNivelEducativo, "Id", "Descripcion", CursoEscolarId);

                var asignaturas = _context.Asignatura
                    .Include(c => c.PlanEstudio)
                    .Include(c => c.PlanEstudio.ComunidadAutonoma)
                    .Include(c => c.CursoEscolar)
                    .Include(c => c.Especialidad)
                    .Where(c => c.PlanEstudio.ComunidadAutonomaId == ComunidadAutonomaId && c.PlanEstudioId == PlanEstudioId && c.CursoEscolarId == CursoEscolarId)
                    .OrderBy(c => c.PlanEstudio.ComunidadAutonoma.Descripcion)
                    .ThenBy(c => c.PlanEstudio.Descripcion)
                    .ThenBy(c => c.CursoEscolar.Descripcion)
                    .ThenByDescending(c => c.HorasSemanales)
                    .AsNoTracking();
                return View(await asignaturas.ToListAsync());
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al filtrar los datos (Index-01)");
            }
            return View();
            
        }
        


        [HttpGet]
        public async Task<IActionResult> CrearEditarAsignatura(int? id)
        {
 
            Asignatura asignatura = new Asignatura();                
            
            if (id!=null) //Editar
            {
                asignatura = await _context.Asignatura
                    .Include(c => c.PlanEstudio)
                    .Include(c => c.CursoEscolar)
                    .SingleOrDefaultAsync(m => m.Id == id);
                ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion", asignatura.PlanEstudio.ComunidadAutonomaId);
                ViewBag.PlanEstudioId = new SelectList(_context.PlanEstudio.OrderBy(x => x.Descripcion).Where(x => x.ComunidadAutonomaId == asignatura.PlanEstudio.ComunidadAutonomaId), "Id", "Descripcion", asignatura.PlanEstudioId);
                ViewBag.CursoEscolarId = new SelectList(_context.CursoEscolar.OrderBy(x => x.Descripcion).Where(x=>x.NivelEducativoId==asignatura.PlanEstudio.NivelEducativoId), "Id", "Descripcion", asignatura.CursoEscolarId);
                ViewBag.EspecialidadId = new SelectList(_context.Especialidad.OrderBy(x => x.Numero), "Id", "Descripcion", asignatura.EspecialidadId);
                if (asignatura == null)
                {
                    return NotFound();
                }
                
            }
            else
            {
                var lstComunidadesAutonomas = _context.ComunidadAutonoma.OrderBy(x => x.Descripcion);
                ViewBag.ComunidadAutonomaId = new SelectList(lstComunidadesAutonomas, "ComunidadAutonomaId", "Descripcion");
                var lstPlanesEstudio = _context.PlanEstudio.OrderBy(x => x.Descripcion).Where(x => x.ComunidadAutonomaId == lstComunidadesAutonomas.FirstOrDefault().ComunidadAutonomaId);
                ViewBag.PlanEstudioId = new SelectList(lstPlanesEstudio, "Id", "Descripcion");
                var lstCursosEscolares = _context.CursoEscolar.OrderBy(x => x.Descripcion).Where(x => x.NivelEducativoId == lstPlanesEstudio.FirstOrDefault().NivelEducativoId);
                ViewBag.CursoEscolarId = new SelectList(lstCursosEscolares, "Id", "Descripcion");
                ViewBag.EspecialidadId = new SelectList(_context.Especialidad.OrderBy(x => x.Numero), "Id", "Descripcion");

            }
            return View(asignatura);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEditarAsignatura(int? id, [Bind("Id,Descripcion,HorasSemanales,PlanEstudioId,CursoEscolarId,EspecialidadId")] Asignatura asignatura, int ComunidadAutonomaId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) //Nueva Asignatura
                    {
                        _context.Add(asignatura);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    else //Editar Asignatura
                    {
                        try
                        {
                            _context.Update(asignatura);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!AsignaturaExists(asignatura.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarAsignatura-01)");
                            }
                        }
                    }

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarAsignatura-02)");
                }
            }
            var lstComunidadesAutonomas = _context.ComunidadAutonoma.OrderBy(x => x.Descripcion);            
            ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion", ComunidadAutonomaId);
            var lstPlanesEstudio = _context.PlanEstudio.OrderBy(x => x.Descripcion).Where(x => x.ComunidadAutonomaId == ComunidadAutonomaId);
            ViewBag.PlanEstudioId = new SelectList(lstPlanesEstudio, "Id", "Descripcion");
            var lstCursosEscolares = _context.CursoEscolar.OrderBy(x => x.Descripcion).Where(x => x.NivelEducativoId == lstPlanesEstudio.FirstOrDefault().NivelEducativoId);
            ViewBag.CursoEscolarId = new SelectList(lstCursosEscolares, "Id", "Descripcion");
            ViewBag.EspecialidadId = new SelectList(_context.Especialidad.OrderBy(x => x.Numero), "Id", "Descripcion");

            return View(asignatura);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarAsignatura(int[] selBorrar)
        {
            foreach (int item in selBorrar)
            {
                var asignatura = await _context.Asignatura.SingleOrDefaultAsync(m => m.Id == item);
                _context.Asignatura.Remove(asignatura);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public ActionResult PopulatePlanEstudio(int? ComunidadAutonomaId)
        {

            var query = _context.PlanEstudio.Where(x => x.ComunidadAutonomaId == ComunidadAutonomaId).OrderBy(x=>x.Descripcion).ToList().Select(x=>new { Id = x.Id, Descripcion = x.Descripcion });


            return Json(query);
        }

        public ActionResult PopulateCursosEscolares(int? PlanEstudioId)
        {
            int iNivelEducativo = _context.PlanEstudio.Where(x => x.Id == PlanEstudioId).FirstOrDefault().NivelEducativoId;
            var query = _context.CursoEscolar.Where(x => x.NivelEducativoId == iNivelEducativo).OrderBy(x => x.Descripcion).ToList().Select(x => new { Id = x.Id, Descripcion = x.Descripcion });
            return Json(query);
        }


        private bool AsignaturaExists(int id)
        {
            return _context.Asignatura.Any(e => e.Id == id);
        }
    }
}
