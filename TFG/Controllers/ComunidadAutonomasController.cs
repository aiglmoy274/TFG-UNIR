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
    /// Clase que contiene los métodos para la gestión de las comunidades autónomas
    /// Métodos:
    ///     public ComunidadAutonomasController(ApplicationDbContext context)
    ///     public async Task<IActionResult> Index()
    ///     [HttpGet] public async Task<IActionResult> CrearEditarComunidadesAutonomas(int? id)
    ///     [HttpPost] public async Task<IActionResult> CrearEditarComunidadesAutonomas(int? id, [Bind("ComunidadAutonomaId,Descripcion")] ComunidadAutonoma comunidadAutonoma)
    ///     [HttpPost] public async Task<IActionResult> BorrarComunidadesAutonomas(int[] selBorrar)
    ///     private bool ComunidadAutonomaExiste(int id)
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class ComunidadAutonomasController : Controller
    {
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public ComunidadAutonomasController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /// <summary>
        /// Carga una lista de comunidades autónomas, para ser mostrada en la vista, ordenada por la descripción
        /// </summary>
        /// <returns>Vista con la lista de comunidades autónomas</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComunidadAutonoma.OrderBy(c=>c.Descripcion).ToListAsync());
        }

        /// <summary>
        /// Método que se encarga de cargar un formulario con los datos vacios, en caso de que se vaya a crear una comunidad autónoma, o relleno con los datos
        /// de la comunidad autónoma determinada por id en caso de querer editar
        /// </summary>
        /// <param name="id">Identificador de la comunidad autónoma a editar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarComunidadesAutonomas(int? id)
        {
 
            ComunidadAutonoma comunidadAutonoma = new ComunidadAutonoma();                
            
            if (id!=null) //Si id es distinto de NULL se desea editar un curso escolar
            {
                //Se recuperan los datos de la comunidad autónoma a editar según el identificador
                comunidadAutonoma = await _context.ComunidadAutonoma.SingleOrDefaultAsync(m => m.ComunidadAutonomaId == id);
                if (comunidadAutonoma == null)//Se comprueba si se ha recuperado alguna comunidad autónoma
                {
                    return NotFound();//Se muestra página de NO ECONTRADO
                }
                
            }
            return View(comunidadAutonoma);//Se llama a la vista
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEditarComunidadesAutonomas(int? id, [Bind("ComunidadAutonomaId,Descripcion")] ComunidadAutonoma comunidadAutonoma)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) //Nueva Comunidad autónoma
                    {
                        //Se añaden los datos al modelo
                        _context.Add(comunidadAutonoma);
                        //Se graban en BBDD
                        await _context.SaveChangesAsync();
                        //Se redirecciona a la vista Index donde aparece la lista de las comunidades autónomas
                        return RedirectToAction("Index");
                    }
                    else //Editar Comunidad Autónoma
                    {
                        try
                        {
                            comunidadAutonoma.ComunidadAutonomaId = (int)id;
                            //Se actualizan los datos en el modelo
                            _context.Update(comunidadAutonoma);
                            //Se graban en BBDD
                            await _context.SaveChangesAsync();
                            //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateConcurrencyException)//Excepción si ha habido error de concurrencia a la hora de actualizar los datos
                        {
                            if (!ComunidadAutonomaExiste(comunidadAutonoma.ComunidadAutonomaId))//Se comprueba que la especialidad que se ha querido editar existe
                            {
                                return NotFound();//Se muestra página de NO ECONTRADO
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarComunidadesAutonomas-01)");
                            }
                        }
                    }

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (CrearEditarComunidadesAutonomas-02)");
                }
            }
            return View(comunidadAutonoma);
        }

        /// <summary>
        /// Método que se encarga de borrar de la BBDD la lista de comunidades autónomas recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de las comunidades autónomas que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarComunidadesAutonomas(int[] selBorrar)
        {
            foreach (int item in selBorrar)//Se recorrer el array
            {
                //Se recupera la comunidad autónoma  según el id
                var comunidadAutonoma = await _context.ComunidadAutonoma.SingleOrDefaultAsync(m => m.ComunidadAutonomaId == item);
                if (comunidadAutonoma != null)
                {
                    try
                    {
                        //Se elimina del modelo la comunidad autónoma a borrar
                        _context.ComunidadAutonoma.Remove(comunidadAutonoma);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Inténtelo pasado unos instantes, si persiste el error póngase en contacto con el administrador del sistema. (BorrarCursoEscolar-01)");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método que comprueba que existe la comunidad autónoma según el identificador recibido
        /// </summary>
        /// <param name="id">Identificador del curso escolar que se desea comprobar su existencia</param>   
        private bool ComunidadAutonomaExiste(int id)
        {
            return _context.ComunidadAutonoma.Any(e => e.ComunidadAutonomaId == id);
        }
    }
}
