//Autor: AGUSTÍN IGLESIAS MOYA
//Fecha: 22/04/2017

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
    /// <summary>
    /// Clase que contiene los métodos para la gestión de los planes de estudio
    /// Métodos:
    ///     public PlanEstudioController(ApplicationDbContext context)
    ///     [HttpGet] public async Task<IActionResult> CrearEditarPlanEstudio(int? id)
    ///     [HttpPost] public async Task<IActionResult> CrearEditarPlanEstudio(int? id, [Bind("Id,Descripcion,Normativa,ComunidadAutonomaId,NivelEducativoId")] PlanEstudio planEstudio)
    ///     [HttpPost] public async Task<IActionResult> BorrarPlanEstudio(int[] selBorrar)
    ///     private bool PlanEstudioExiste(int id)
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class PlanEstudioController : Controller
    {
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public PlanEstudioController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /// <summary>
        /// Carga una lista de los planes de estudio, para ser mostrada en la vista, ordenada en primer lugar por la descripción de la comunidad autónoma
        /// a la que pertenece y en segundo lugar por la propia descripción del plan de estudio.
        /// Para cada plan de estudio además se incluye información de la comunidad autónoma y del nivel educativo
        /// </summary>
        /// <returns>Vista con la lista de Planes de estudio</returns>
        public async Task<IActionResult> Index()
        {
            var planesEstudios = _context.PlanEstudio
                .Include(c => c.ComunidadAutonoma)
                .Include(c=>c.NivelEducativo)
                .OrderBy(c => c.ComunidadAutonoma.Descripcion)
                .ThenBy(c => c.Descripcion)
                .AsNoTracking();
            return View(await planesEstudios.ToListAsync());

        }

        /// <summary>
        /// Método que se encarga de cargar un formulario con los datos vacios en caso de que se vaya a crear un plan de estudio o relleno con los datos
        /// del plan de estudio determinado por id en caso de querer editar
        /// </summary>
        /// <param name="id">Identificador del plan de estudio a editar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarPlanEstudio(int? id)
        {
 
            PlanEstudio planEstudio = new PlanEstudio();                
            
            if (id!=null) //Si el identificador es distinto de NULL es que se va a cargar los datos para editar
            {
                //Recupera los datos del plan de estudio según el identificador recibido
                planEstudio = await _context.PlanEstudio.SingleOrDefaultAsync(m => m.Id == id); 

                if (planEstudio == null)//Se comprueba si se ha recuperado algún plan de estudio para el id recibido
                {
                    return NotFound(); //Se muestra página de NO ECONTRADO
                }
                //Carga la lista de comunidades autónomas para ser mostrada en la vista a través de un <SELECT>
                //seleccionando el valor de la comunidad autónoma a la que pertenece el plan de estudio
                ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion", planEstudio.ComunidadAutonomaId);
                //Carga la lista de niveles educativos para ser mostrada en la vista a través e un <SELECT>
                //seleccionando el valor del nivel educativo al que pertenece el plan de estudio
                ViewBag.NivelEducativoId = new SelectList(_context.NivelEducativo.OrderBy(x => x.Descripcion), "Id", "Descripcion", planEstudio.NivelEducativoId);
            }
            else //Se trata de un formulario para crear un nuevo plan de estudio
            {
                //Carga la lista de comunidades autónomas para ser mostrada en la vista a través de un <SELECT>
                ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion");
                //Carga la lista de niveles educativos para ser mostrada en la vista a través e un <SELECT>
                ViewBag.NivelEducativoId = new SelectList(_context.NivelEducativo.OrderBy(x => x.Descripcion), "Id", "Descripcion");
            }            
            return View(planEstudio);//Se llama a la vista
        }

        /// <summary>
        /// Método que se encarga de crear o editar en la BBDD los datos del plan de estudio recibidos de la vista una vez se ha pulsado el botón GUARDAR        
        /// </summary>
        /// <param name="id">Identificador del plan de estudio a Editar, en caso de ser una acción de crear un plan de estudio se recibirá como NULL</param>
        /// <param name="planEstudio">Datos del plan de estudio recibidos de la vista para ser grados en BBDD</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Evita que se llame al método desde un lugar distinto al del formulario creado vía GET
        public async Task<IActionResult> CrearEditarPlanEstudio(int? id, [Bind("Id,Descripcion,Normativa,ComunidadAutonomaId,NivelEducativoId")] PlanEstudio planEstudio)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {
                    if (id == null) //Nuevo Plan de estudio
                    {
                        //Se añaden los datos al modelo
                        _context.Add(planEstudio);
                        //Se graban en BBDD
                        await _context.SaveChangesAsync();
                        //Se redirecciona a la vista Index donde aparece la lista de los planes de estudio
                        return RedirectToAction("Index");
                    }
                    else //Editar Plan de estudio
                    {
                        try
                        {
                            //Se actualizan los datos en el modelo
                            _context.Update(planEstudio);
                            //Se graban en BBDD
                            await _context.SaveChangesAsync();
                            //Se redirecciona a la vista Index donde aparece la lista de los planes de estudio
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException)//Excepción si ha habido error de concurrencia a la hora de actualizar los datos
                        {
                            if (!PlanEstudioExiste(planEstudio.Id))//Se comprueba que el plan de estudio que se ha querido editar existe
                            {
                                return NotFound(); //Se muestra página de NO ECONTRADO
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarPlanEstudio-01)");
                            }
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarPlanEstudio-02)");
                }
            }            
            //Carga la lista de comunidades autónomas para ser mostrada en la vista a través de un <SELECT>
            ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion", planEstudio.ComunidadAutonomaId);
            //Carga la lista de niveles educativos para ser mostrada en la vista a través e un <SELECT>
            ViewBag.NivelEducativoId = new SelectList(_context.NivelEducativo.OrderBy(x => x.Descripcion), "Id", "Descripcion",planEstudio.NivelEducativoId);
            return View(planEstudio);//Se vuelve a la vista para mostrar los errores
        }


        /// <summary>
        /// Método que se encarga de borrar de la BBDD la lista de planes de estudio recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de los planes de estudio que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarPlanEstudio(int[] selBorrar)
        {            
            foreach (int item in selBorrar)//Se recorrer el array
            {
                //Se recupera el plan de estudio según el id
                var planEstudio = await _context.PlanEstudio.SingleOrDefaultAsync(m => m.Id == item);
                if (planEstudio != null)//Se comprueba que se ha recuperado algún plan de estudio
                {
                    try
                    {
                        //Se elimina del modelo el plan de estudio
                        _context.PlanEstudio.Remove(planEstudio);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarPlanEstudio-01)");
                    }

                }
            }
            return RedirectToAction("Index");//Se redirecciona a Index
        }


        /// <summary>
        /// Comprueba que existe el plan de estudio según el identificador recibido
        /// </summary>
        /// <param name="id">Identificador del plan de estudio que se desea comprobar su existencia</param>
        /// <returns></returns>
        private bool PlanEstudioExiste(int id)
        {
            return _context.PlanEstudio.Any(e => e.Id == id);
        }
    }
}
