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
    /// Clase que contiene los métodos para la gestión de los niveles educativos
    /// Métodos:
    ///     public NivelEducativoController(ApplicationDbContext context)
    ///     public async Task<IActionResult> Index()
    ///     [HttpGet] public async Task<IActionResult> CrearEditarNivelEducativo(int? id)
    ///     [HttpPost] public async Task<IActionResult> CrearEditarNivelEducativo(int? id, [Bind("Id,Descripcion")] NivelEducativo nivelEducativo)
    ///     [HttpPost] public async Task<IActionResult> BorrarNivelEducativo(int[] selBorrar)
    ///     private bool NivelEducativoExiste(int id)
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class NivelEducativoController : Controller
    {
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public NivelEducativoController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /// <summary>
        /// Carga una lista de los niveles educativos, para ser mostrada en la vista, ordenada en  la descripción.        
        /// </summary>
        /// <returns>Vista con la lista de Niveles educativos</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.NivelEducativo.OrderBy(c=>c.Descripcion).ToListAsync());
        }

        /// <summary>
        /// Método que se encarga de cargar un formulario con los datos vacios, en caso de que se vaya a crear un nivel educativo, o relleno con los datos
        /// del nivel educativo determinado por id en caso de querer editar
        /// </summary>
        /// <param name="id">Identificador del nivel educativo a editar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarNivelEducativo(int? id)
        {
 
            NivelEducativo nivelEducativo = new NivelEducativo();                
            
            if (id!=null) //Si id es distinto de NULL se desea editar un nivel educativo
            {
                //Se recuperan los datos del nivel educativo a editar según el identificador
                nivelEducativo = await _context.NivelEducativo.SingleOrDefaultAsync(m => m.Id == id);
                if (nivelEducativo == null)//Se comprueba si se ha recuperado algún nivel educativo
                {
                    return NotFound(); //Se muestra página de NO ECONTRADO
                }
                
            }
            return View(nivelEducativo);//Se llama a la vista
        }

        /// <summary>
        /// Método que se encarga de crear o editar en la BBDD los datos del Nivel educativo recibidos de la vista una vez se ha pulsado el botón GUARDAR        
        /// </summary>
        /// <param name="id">Identificador del nivel educativo a Editar, en caso de ser una acción de crear un nivel educativo se recibirá como NULL</param>
        /// <param name="nivelEducativo">Datos del nivel educativo recibidos de la vista para ser grados en BBDD</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Evita que se llame al método desde un lugar distinto al del formulario creado vía GET
        public async Task<IActionResult> CrearEditarNivelEducativo(int? id, [Bind("Id,Descripcion")] NivelEducativo nivelEducativo)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean válidos
            {
                try
                {
                    if (id == null) //Nuevo Nivel Educativo
                    {
                        //Se añaden los datos al modelo
                        _context.Add(nivelEducativo);
                        //Se graban en BBDD
                        await _context.SaveChangesAsync();
                        //Se redirecciona a la vista Index donde aparece la lista de los planes de estudio
                        return RedirectToAction("Index");
                    }
                    else //Editar Nivel Educativo
                    {
                        try
                        {
                            //Se actualizan los datos en el modelo
                            _context.Update(nivelEducativo);
                            //Se graban en BBDD
                            await _context.SaveChangesAsync();
                            //Se redirecciona a la vista Index donde aparece la lista de los planes de estudio
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateConcurrencyException)//Excepción si ha habido error de concurrencia a la hora de actualizar los datos
                        {
                            if (!NivelEducativoExiste(nivelEducativo.Id))//Se comprueba que el nivel educativo que se ha querido editar existe
                            {
                                return NotFound(); //Se muestra página de NO ECONTRADO
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarNivelEducativo-01)");
                            }
                        }
                    }
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarNivelEducativo-02)");
                }
                
            }
            return View(nivelEducativo);//Se vuelve a la vista para mostrar los errores
        }

        /// <summary>
        /// Método que se encarga de borrar de la BBDD la lista de niveles educativos recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de los niveles educativos que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarNivelEducativo(int[] selBorrar)
        {
            foreach (int item in selBorrar)//Se recorrer el array
            {
                //Se recupera el plan de estudio según el id
                var nivelEducativo = await _context.NivelEducativo.SingleOrDefaultAsync(m => m.Id == item);
                if (nivelEducativo != null)
                {
                    try
                    {
                        //Se elimina del modelo el nivel educativo a borrar
                        _context.NivelEducativo.Remove(nivelEducativo);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarNivelEducativo-01)");
                    }
                }
            }
            return RedirectToAction("Index");//Se redirecciona a Index
        }

        /// <summary>
        /// Método que comprueba que existe el nivel educativo según el identificador recibido
        /// </summary>
        /// <param name="id">Identificador del nivel educativo que se desea comprobar su existencia</param>
        private bool NivelEducativoExiste(int id)
        {
            return _context.NivelEducativo.Any(e => e.Id == id);
        }
    }
}
