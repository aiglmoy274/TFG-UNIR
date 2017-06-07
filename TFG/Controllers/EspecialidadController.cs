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
    /// Clase que contiene los métodos para la gestión de las especialidad del profesorado
    /// Métodos:
    ///     public EspecialidadController(ApplicationDbContext context)
    ///     public async Task<IActionResult> Index()
    ///     [HttpGet] public async Task<IActionResult> CrearEditarEspecialidad(int? id)
    ///     [HttpPost] public async Task<IActionResult> CrearEditarEspecialidad(int? id, [Bind("Id,Numero,Descripcion")] Especialidad especialidad)
    ///     [HttpPost] public async Task<IActionResult> BorrarEspecialidad(int[] selBorrar)
    ///     private bool EspecialidadExiste(int id)
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class EspecialidadController : Controller
    {
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public EspecialidadController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /// <summary>
        /// Carga una lista de especialidades, para ser mostrada en la vista, ordenada por el número de especialidad.        
        /// </summary>
        /// <returns>Vista con la lista de especialidad</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Especialidad.OrderBy(c=>c.Numero).ToListAsync());
        }

        /// <summary>
        /// Método que se encarga de cargar un formulario con los datos vacios, en caso de que se vaya a crear una especialidad, o relleno con los datos
        /// de la especialidad determinada por id en caso de querer editar
        /// </summary>
        /// <param name="id">Identificador de la especialidad a editar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarEspecialidad(int? id)
        {
 
            Especialidad especialidad = new Especialidad();                
            
            if (id!=null) //Si id es distinto de NULL se desea editar una especialidad
            {
                //Se recuperan los datos de la especialida a editar según el identificador
                especialidad = await _context.Especialidad.SingleOrDefaultAsync(m => m.Id == id);
                if (especialidad == null)//Se comprueba si se ha recuperado alguna especialidad
                {
                    return NotFound(); //Se muestra página de NO ECONTRADO
                }                
            }
            return View(especialidad);//Se llama a la vista
        }

        /// <summary>
        /// Método que se encarga de crear o editar en la BBDD los datos de la especialidad recibidos de la vista una vez se ha pulsado el botón GUARDAR        
        /// </summary>
        /// <param name="id">Identificador de la especialidad a Editar, en caso de ser una acción de crear una especialidad se recibirá como NULL</param>
        /// <param name="especialidad">Datos de la especialidad recibidos de la vista para ser grados en BBDD</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al método desde un lugar distinto al formulario creado vía GET
        public async Task<IActionResult> CrearEditarEspecialidad(int? id, [Bind("Id,Numero,Descripcion")] Especialidad especialidad)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {
                    if (id == null) //Nueva Especialidad
                    {
                        //Se añaden los datos al modelo
                        _context.Add(especialidad);
                        //Se graban en BBDD
                        await _context.SaveChangesAsync();
                        //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                        return RedirectToAction("Index");
                    }
                    else //Editar Especialidad
                    {
                        try
                        {
                            //Se actualizan los datos en el modelo
                            _context.Update(especialidad);
                            //Se graban en BBDD
                            await _context.SaveChangesAsync();
                            //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateException)//Se captura si ha habido un error al grabar en BBDD
                        {
                            if (!EspecialidadExiste(especialidad.Id))//Se comprueba que la especialidad que se ha querido editar existe
                            {
                                return NotFound(); //Se muestra página de NO ECONTRADO
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarEspecialidad-01)");
                            }
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarEspecialidad-02)");
                }
 

            }
            return View(especialidad);//Se vuelve a la vista para mostrar los errores
        }

        /// <summary>
        /// Método que se encarga de borrar de la BBDD la lista de especialidades recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de las especialidades que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarEspecialidad(int[] selBorrar)
        {
            foreach (int item in selBorrar)//Se recorrer el array
            {
                //Se recupera la especialidad según el id
                var especialidad = await _context.Especialidad.SingleOrDefaultAsync(m => m.Id == item);
                if (especialidad != null)
                {
                    try
                    {
                        //Se elimina del modelo la especialidad a borrar
                        _context.Especialidad.Remove(especialidad);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch(DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarEspecialidad-01)");
                    }
                }
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Método que comprueba que existe la especialidad según el identificador recibido
        /// </summary>
        /// <param name="id">Identificador de la especialidd que se desea comprobar su existencia</param>
        private bool EspecialidadExiste(int id)
        {
            return _context.Especialidad.Any(e => e.Id == id);
        }
    }
}
