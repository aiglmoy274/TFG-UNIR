//Autor: AGUSTÍN IGLESIAS MOYA
//Fecha: 27/04/2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TFG.Data;
using TFG.Models;
using TFG.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TFG.Controllers
{
    /// <summary>
    /// Clase que contiene los métodos para la gestión de las planificaciones que realiza el usuario
    /// Métodos:

    [Authorize(Roles = "Usuario")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class PlanificacionController : Controller
    {
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;


        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public PlanificacionController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlanificacionId"></param>
        /// <param name="GrupoId"></param>
        /// <param name="pestana"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? PlanificacionId,int? GrupoId,int? pestana)
        {
            if (pestana != null)
            {
                ViewBag.pestana = pestana.Value;
            }
            else
            {
                ViewBag.pestana = 0;
            }

            PlanficacionesViewModel planificacionesVM = new PlanficacionesViewModel();


            ApplicationUser usuario = await _userManager.FindByNameAsync(User.Identity.Name);
            var lstPlanificaciones = await _context.Planificacion.Where(c => c.UsuarioId == usuario.Id).OrderBy(c => c.Descripcion).ToListAsync();
            if (PlanificacionId == null)
            {
                var planificacion = lstPlanificaciones.FirstOrDefault();
                if (planificacion != null)
                {
                    PlanificacionId = planificacion.Id;
                    planificacionesVM.Planificacion = planificacion;

                }
                else
                {
                    PlanificacionId = 0;
                    planificacionesVM.Planificacion = new Planificacion();
                }
            }
            else
            {
                Planificacion planificacion = _context.Planificacion.Find(PlanificacionId);
                if (planificacion.UsuarioId != usuario.Id)
                {
                    return RedirectToAction("AccessDenied","Account");
                }
                planificacionesVM.Planificacion = planificacion;
            }
            ViewBag.PlanificacionId = new SelectList(lstPlanificaciones, "Id", "Descripcion", PlanificacionId);
            ViewBag.EspecialidadId = new SelectList(await _context.Especialidad.OrderBy(c => c.Descripcion).ToListAsync(), "Id", "Descripcion");
            
            var lstPlanesEstudioComunidadAutonoma = await _context.PlanEstudio.Where(p => p.ComunidadAutonomaId == usuario.ComunidadAutonomaId).OrderBy(p => p.Descripcion).ToListAsync();
            var planEstudio = lstPlanesEstudioComunidadAutonoma.FirstOrDefault();
            ViewBag.PlanEstudioId = new SelectList(lstPlanesEstudioComunidadAutonoma, "Id", "Descripcion", planEstudio.Id);
            var lstCursosEscolaresNivelEducativo = await _context.CursoEscolar.Where(p => p.NivelEducativo.Id == planEstudio.NivelEducativoId).OrderBy(p => p.Descripcion).ToListAsync();
            ViewBag.CursoEscolarId = new SelectList(lstCursosEscolaresNivelEducativo, "Id", "Descripcion");


                       

            planificacionesVM.PlanificacionReduccion = new PlanificacionReduccion();
            planificacionesVM.PlanificacionReduccion.PlanificacionId = PlanificacionId.Value;

            planificacionesVM.PlanificacionAsignatura = new PlanificacionAsignatura();
            planificacionesVM.PlanificacionAsignatura.PlanificacionId = PlanificacionId.Value;

            planificacionesVM.PlanificacionId = PlanificacionId.Value;

            planificacionesVM.PlanificacionesReduccion = await _context.PlanificacionReduccion.Where(c => c.PlanificacionId == PlanificacionId).OrderBy(c => c.Especialidad.Descripcion).ThenBy(c=> c.Descripcion).ToListAsync();

            planificacionesVM.Grupo = new Grupo();
            planificacionesVM.Grupo.PlanificacionId = PlanificacionId.Value;
            var lstGrupos= await _context.Grupo.Where(c => c.PlanificacionId == PlanificacionId)
                .OrderBy(c => c.CursoEscolar.Descripcion)
                .ThenBy(c=> c.Descripcion)
                .Include(c=>c.CursoEscolar)
                .ToListAsync();
            planificacionesVM.Grupos = lstGrupos;
            if (GrupoId != null)
            {
                ViewBag.GrupoId = GrupoId.Value;
            }
            else
            {
                ViewBag.GrupoId = 0;
                if (lstGrupos.Count !=0)
                {
                    ViewBag.GrupoId = lstGrupos.FirstOrDefault().Id;
                }
                
            }


            var grResultadosReduccion = _context.PlanificacionReduccion
                .Where(c => c.PlanificacionId == PlanificacionId)
                .Include(c=>c.Especialidad)
                .GroupBy(c => c.EspecialidadId)
                .Select(c=> new {
                    especialidad = c.First().Especialidad,
                    horas = c.Sum(x=>x.HorasSemanales)
                }).ToList();

            var grResultadosAsignatura = _context.PlanificacionAsignatura
                .Where(c => c.PlanificacionId == PlanificacionId)
                .Include(c => c.Especialidad)
                .GroupBy(c => c.EspecialidadId)
                .Select(c => new {
                    especialidad = c.First().Especialidad,
                    horas = c.Sum(x => x.HorasSemanales)
                }).ToList();

            int horasSemanales = planificacionesVM.Planificacion.HorasSemanales;

            List<ResultadosViewModel> resultados = new List<ResultadosViewModel>();
            foreach (var item in grResultadosAsignatura)
            {
                ResultadosViewModel res = resultados.FirstOrDefault(x=>x.EspecialidadId==item.especialidad.Id);
                if (res != null)
                {
                    res.HorasSemanalesAsignaturas += item.horas;
                    res.TotalHorasSemanales += item.horas;
                    res.NumProfesores = res.TotalHorasSemanales / horasSemanales;
                    res.HorasFaltanCuadrarProfesor = ((res.NumProfesores + 1) * horasSemanales) - res.TotalHorasSemanales;
                }
                else
                {
                    res = new ResultadosViewModel();
                    res.EspecialidadId = item.especialidad.Id;
                    res.Especialidad = item.especialidad;
                    res.HorasSemanalesAsignaturas = item.horas;
                    res.HorasSemanalesReduccion = 0;
                    res.TotalHorasSemanales = item.horas;
                    res.NumProfesores = res.TotalHorasSemanales / horasSemanales;
                    res.HorasFaltanCuadrarProfesor = ((res.NumProfesores + 1) * horasSemanales) - res.TotalHorasSemanales;
                    resultados.Add(res);
                }                
            }

            foreach (var item in grResultadosReduccion)
            {
                ResultadosViewModel res = resultados.FirstOrDefault(x => x.EspecialidadId == item.especialidad.Id);
                if (res != null)
                {
                    res.HorasSemanalesReduccion += item.horas;
                    res.TotalHorasSemanales += item.horas;
                    res.NumProfesores = res.TotalHorasSemanales / horasSemanales;
                    res.HorasFaltanCuadrarProfesor = ((res.NumProfesores + 1) * horasSemanales) - res.TotalHorasSemanales;
                }
                else
                {
                    res = new ResultadosViewModel();
                    res.EspecialidadId = item.especialidad.Id;
                    res.Especialidad = item.especialidad;
                    res.HorasSemanalesReduccion = item.horas;
                    res.HorasSemanalesAsignaturas = 0;
                    res.TotalHorasSemanales = item.horas;
                    res.NumProfesores = res.TotalHorasSemanales / horasSemanales;
                    res.HorasFaltanCuadrarProfesor = ((res.NumProfesores + 1) * horasSemanales) - res.TotalHorasSemanales;
                    resultados.Add(res);
                }
                
            }

            planificacionesVM.Resultados = resultados.OrderBy(c=>c.Especialidad.Descripcion).ToList(); 

            return View(planificacionesVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planificacionAsignatura"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al método desde un lugar distinto al formulario creado vía GET
        public async Task<IActionResult> CrearPlanificacionAsignatura([Bind("GrupoId,AsignaturaId,EspecialidadId,PlanificacionId,HorasSemanales")] PlanificacionAsignatura planificacionAsignatura)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {

                    //Se añaden los datos al modelo
                    _context.Add(planificacionAsignatura);
                    //Se graban en BBDD
                    await _context.SaveChangesAsync();
                    //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                    return RedirectToAction("Index", new { PlanificacionId = planificacionAsignatura.PlanificacionId, pestana = 1 });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearPlanificacionReduccion-01)");
                }


            }
            //return View(planificacion);//Se vuelve a la vista para mostrar los errores
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planificacionReduccion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al método desde un lugar distinto al formulario creado vía GET
        public async Task<IActionResult> CrearPlanificacionReduccion([Bind("Descripcion,HorasSemanales,EspecialidadId,PlanificacionId")] PlanificacionReduccion planificacionReduccion)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {

                    //Se añaden los datos al modelo
                    _context.Add(planificacionReduccion);
                    //Se graban en BBDD
                    await _context.SaveChangesAsync();
                    //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                    return RedirectToAction("Index", new { PlanificacionId = planificacionReduccion.PlanificacionId});
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearPlanificacionReduccion-01)");
                }


            }
            //return View(planificacion);//Se vuelve a la vista para mostrar los errores
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al método desde un lugar distinto al formulario creado vía GET
        public async Task<IActionResult> CrearGrupo([Bind("Descripcion,CursoEscolarId,PlanificacionId")] Grupo grupo)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {

                    //Se añaden los datos al modelo
                    _context.Add(grupo);
                    //Se graban en BBDD
                    await _context.SaveChangesAsync();
                    //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                    return RedirectToAction("Index", new { PlanificacionId = grupo.PlanificacionId, pestana = 1 });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearPlanificacionReduccion-01)");
                }


            }
            //return View(planificacion);//Se vuelve a la vista para mostrar los errores
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al método desde un lugar distinto al formulario creado vía GET
        public async Task<IActionResult> CrearPlanificacion([Bind("Descripcion,HorasSemanales")] Planificacion planificacion)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {
                    ApplicationUser usuario = await _userManager.FindByNameAsync(User.Identity.Name);
                    planificacion.Usuario = usuario;
                    //Se añaden los datos al modelo
                    _context.Add(planificacion);
                    //Se graban en BBDD
                    await _context.SaveChangesAsync();
                    //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                    return RedirectToAction("Index",new { PlanificacionId=planificacion.Id});
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearPlanificacion-01)");
                }
 

            }
            //return View(planificacion);//Se vuelve a la vista para mostrar los errores
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Método que se encarga de borrar de la BBDD la lista de especialidades recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de las especialidades que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpGet]//TODO: Ver si se puede hacer por POST
        public async Task<IActionResult> BorrarPlanificacion(int Id)
        {

            var planificacion = await _context.Planificacion.SingleOrDefaultAsync(m => m.Id == Id);
            if (planificacion != null)
            {
                try
                {
                    //Se elimina del modelo la planificación a borrar
                    _context.Planificacion.Remove(planificacion);
                    //Se graba en BBDD
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarEspecialidad-01)");
                }
            }            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selBorrarPlanificacionReduccion"></param>
        /// <param name="PlanificacionId"></param>
        /// <returns></returns>
        public async Task<IActionResult> BorrarPlanificacionReduccion(int[] selBorrarPlanificacionReduccion,int PlanificacionId)
        {            
            foreach (int item in selBorrarPlanificacionReduccion)//Se recorrer el array
            {                
                //Se recupera la planificacionReducción según el id
                var planificacionReduccion = await _context.PlanificacionReduccion.SingleOrDefaultAsync(m => m.Id == item);
                if (planificacionReduccion != null)
                {
                    try
                    {
                        //Se elimina del modelo la especialidad a borrar
                        _context.PlanificacionReduccion.Remove(planificacionReduccion);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarEspecialidad-01)");
                    }
                }
            }
            return RedirectToAction("Index", new { PlanificacionId = PlanificacionId, pestana = 0 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selBorrarGrupo"></param>
        /// <param name="PlanificacionId"></param>
        /// <returns></returns>
        public async Task<IActionResult> BorrarGrupo(int[] selBorrarGrupo, int PlanificacionId)
        {
            foreach (int item in selBorrarGrupo)//Se recorrer el array
            {
                //Se recupera la planificacionReducción según el id
                var grupo = await _context.Grupo.SingleOrDefaultAsync(m => m.Id == item);
                if (grupo != null)
                {
                    try
                    {
                        //Se elimina del modelo la especialidad a borrar
                        _context.Grupo.Remove(grupo);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarEspecialidad-01)");
                    }
                }
            }
            return RedirectToAction("Index", new { PlanificacionId = PlanificacionId, pestana = 1 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selBorrarPlanificacionAsignatura"></param>
        /// <param name="PlanificacionId"></param>
        /// <returns></returns>
        public async Task<IActionResult> BorrarPlanificacionAsignatura(int[] selBorrarPlanificacionAsignatura, int PlanificacionId)
        {
            foreach (int item in selBorrarPlanificacionAsignatura)//Se recorrer el array
            {
                //Se recupera la planificacionReducción según el id
                var planificacionAsignatura = await _context.PlanificacionAsignatura.SingleOrDefaultAsync(m => m.Id == item);
                if (planificacionAsignatura != null)
                {
                    try
                    {
                        //Se elimina del modelo la especialidad a borrar
                        _context.PlanificacionAsignatura.Remove(planificacionAsignatura);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarEspecialidad-01)");
                    }
                }
            }
            return RedirectToAction("Index", new { PlanificacionId = PlanificacionId, pestana = 1 });
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlanificacionIdPlaRed"></param>
        /// <param name="DescripcionPlaRed"></param>
        /// <param name="HorasSemanalesPlaRed"></param>
        /// <param name="EspecialidadId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditarPlanificacionReduccion(int PlanificacionIdPlaRed, string DescripcionPlaRed, int HorasSemanalesPlaRed, int EspecialidadId)
        {
            try
            {
                PlanificacionReduccion plaRed = _context.PlanificacionReduccion.Find(PlanificacionIdPlaRed);
                plaRed.Descripcion = DescripcionPlaRed;
                plaRed.HorasSemanales = HorasSemanalesPlaRed;
                plaRed.EspecialidadId = EspecialidadId;
                //Se actualizan los datos en el modelo
                _context.Update(plaRed);
                //Se graban en BBDD
                await _context.SaveChangesAsync();
                //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                return RedirectToAction("Index", new { PlanificacionId =  plaRed.PlanificacionId, pestana = 0 });
            }
            catch (DbUpdateException)//Se captura si ha habido un error al grabar en BBDD
            {
                if (!PlanificacionReduccionExiste(PlanificacionIdPlaRed))//Se comprueba que la especialidad que se ha querido editar existe
                {
                    return NotFound(); //Se muestra página de NO ECONTRADO
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarEspecialidad-01)");
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método que comprueba que existe la especialidad según el identificador recibido
        /// </summary>
        /// <param name="id">Identificador de la especialidd que se desea comprobar su existencia</param>
        private bool PlanificacionReduccionExiste(int id)
        {
            return _context.PlanificacionReduccion.Any(e => e.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlanEstudioId"></param>
        /// <returns></returns>
        public ActionResult PopulateCursosEscolares(int? PlanEstudioId)
        {
            int iNivelEducativo = _context.PlanEstudio.Where(x => x.Id == PlanEstudioId).FirstOrDefault().NivelEducativoId;
            var query = _context.CursoEscolar
                .Where(x => x.NivelEducativoId == iNivelEducativo)
                .OrderBy(x => x.Descripcion)
                .ToList()
                .Select(x => new { Id = x.Id, Descripcion = x.Descripcion });
            return Json(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GrupoId"></param>
        /// <returns></returns>
        public ActionResult CargarPlanificacionAsignaturasPorGrupos(int GrupoId)
        {
            var query = _context.PlanificacionAsignatura
                .Where(c => c.GrupoId == GrupoId)
                .Include(c=>c.Asignatura)
                .Include(c=>c.Especialidad)
                .OrderByDescending(x => x.HorasSemanales)
                .ToList()
                .Select(x => new {
                    id = x.Id,
                    especialidad = x.Especialidad.Descripcion,
                    asignatura = x.Asignatura.Descripcion,
                    horasSemanales = x.HorasSemanales
                });
            return Json(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GrupoId"></param>
        /// <returns></returns>
        public ActionResult CargarAsignaturas(int GrupoId)
        {
            //TODO: Filtrar por la comunidad autónoma del centro educativo
            Grupo grupo = _context.Grupo.Where(c => c.Id == GrupoId).FirstOrDefault();            
            if (grupo != null)
            {
                var query = _context.Asignatura
                    .Where(c=>c.CursoEscolarId==grupo.CursoEscolarId)
                    .OrderBy(c=>c.Descripcion)
                    .ToList()
                    .Select(x => new { Id = x.Id, Descripcion = x.Descripcion, EspecialidadId = x.EspecialidadId, HorasSemanales = x.HorasSemanales });
                return Json(query);

            }
            return null;
        }

        public ActionResult CargarDatosAsignaturas(int AsignaturaId)
        {
            var query = _context.Asignatura.Where(c => c.Id == AsignaturaId).First();
            return Json(query);
        }

        public ActionResult EditarPlanificacionReduccion(int? Id)
        {
            var query = _context.PlanificacionReduccion.Where(x => x.Id == Id).First();
            return Json(query);
        }

    }
}
