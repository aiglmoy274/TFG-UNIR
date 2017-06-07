//Autor: AGUST�N IGLESIAS MOYA
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
    /// Clase que contiene los m�todos para la gesti�n de los cursos escolares
    /// M�todos:
    ///     public CursoEscolarController(ApplicationDbContext context)
    ///     public async Task<IActionResult> Index()
    ///     [HttpGet] public async Task<IActionResult> CrearEditarCursoEscolar(int? id)
    ///     [HttpPost] public async Task<IActionResult> CrearEditarCursoEscolar(int? id, [Bind("Id,Descripcion,NivelEducativoId")] CursoEscolar cursoEscolar)
    ///     [HttpPost] public async Task<IActionResult> BorrarCursoEscolar(int[] selBorrar)
    ///     private bool CursoEscolarExiste(int id)
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase s�lo est� autorizado a usuarios con perfil de Administrador
    public class CursoEscolarController : Controller
    {
        /// <summary>
        /// Objeto que permite la gesti�n de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="context"></param>
        public CursoEscolarController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /// <summary>
        /// Carga una lista de cursos escolares, para ser mostrada en la vista, ordenada por la descripci�n del nivel educativo y despu�s por la descripci�n 
        /// del curso escolar
        /// </summary>
        /// <returns>Vista con la lista de especialidad</returns>
        public async Task<IActionResult> Index()
        {
            var cursosEscolares = _context.CursoEscolar
                .Include(c => c.NivelEducativo)
                .OrderBy(c=>c.NivelEducativo.Descripcion)
                .ThenBy(c=>c.Descripcion)
                .AsNoTracking();
            return View(await cursosEscolares.ToListAsync());
        }

        /// <summary>
        /// M�todo que se encarga de cargar un formulario con los datos vacios, en caso de que se vaya a crear un curso escolar, o relleno con los datos
        /// del curso escolar determinada por id en caso de querer editar
        /// </summary>
        /// <param name="id">Identificador del curso escolar a editar</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarCursoEscolar(int? id)
        {

            CursoEscolar cursoEscolar = new CursoEscolar();
            if (id != null) //Si id es distinto de NULL se desea editar un curso escolar
            {
                //Se recuperan los datos del curso escolar a editar seg�n el identificador
                cursoEscolar = await _context.CursoEscolar.SingleOrDefaultAsync(m => m.Id == id);
                if (cursoEscolar == null)//Se comprueba si se ha recuperado alg�n curso escolar
                {
                    return NotFound();//Se muestra p�gina de NO ECONTRADO
                }
                //Carga la lista de niveles educativos para ser mostrada en la vista a trav�s de un <SELECT>
                //seleccionando el valor del nivel educativo a la que pertenece el plan de estudio
                ViewBag.NivelEducativoId = new SelectList(_context.NivelEducativo.OrderBy(x => x.Descripcion), "Id", "Descripcion", cursoEscolar.NivelEducativoId);
            }
            else
            {
                //Carga la lista de niveles educativos para ser mostrada en la vista a trav�s de un <SELECT>
                ViewBag.NivelEducativoId = new SelectList(_context.NivelEducativo.OrderBy(x => x.Descripcion), "Id", "Descripcion");
            }
            return View(cursoEscolar);//Se llama a la vista
        }

        /// <summary>
        /// M�todo que se encarga de crear o editar en la BBDD los datos del curso escolar recibidos de la vista una vez se ha pulsado el bot�n GUARDAR        
        /// </summary>
        /// <param name="id">Identificador del curso escolar a Editar, en caso de ser una acci�n de crear un curso escolar se recibir� como NULL</param>
        /// <param name="especialidad">Datos del curso escolar recibidos de la vista para ser grados en BBDD</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]// Evita que se llame al m�todo desde un lugar distinto al formulario creado v�a GET
        public async Task<IActionResult> CrearEditarCursoEscolar(int? id, [Bind("Id,Descripcion,NivelEducativoId")] CursoEscolar cursoEscolar)
        {
            if (ModelState.IsValid)//Se comprueba que los datos del formulario sean v�lidos
            {
                try
                {                
                    if (id==null) //Nuevo Curso Escolar
                    {
                        //Se a�aden los datos al modelo
                        _context.Add(cursoEscolar);
                        //Se graban en BBDD
                        await _context.SaveChangesAsync();
                        //Se redirecciona a la vista Index donde aparece la lista de los cursos escolares
                        return RedirectToAction("Index");
                    }
                    else //Editar Curso Escolar
                    {
                        try
                        {
                            //Se actualizan los datos en el modelo
                            _context.Update(cursoEscolar);
                            //Se graban en BBDD
                            await _context.SaveChangesAsync();
                            //Se redirecciona a la vista Index donde aparece la lista de las especialidades
                            return RedirectToAction("Index");
                        }
                        catch (DbUpdateConcurrencyException)//Excepci�n si ha habido error de concurrencia a la hora de actualizar los datos
                        {
                            if (!CursoEscolarExiste(cursoEscolar.Id))//Se comprueba que la especialidad que se ha querido editar existe
                            {
                                return NotFound();//Se muestra p�gina de NO ECONTRADO
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Int�ntelo pasado unos instantes, si persiste el error p�ngase en contacto con el administrador del sistema. (CrearEditarCursoEscolar-01)");
                            }
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Int�ntelo pasado unos instantes, si persiste el error p�ngase en contacto con el administrador del sistema. (CrearEditarCursoEscolar-02)");
                }
            }
            return View(cursoEscolar);//Se vuelve a la vista para mostrar los errores
        }

        /// <summary>
        /// M�todo que se encarga de borrar de la BBDD la lista de cursos escolares recibida
        /// </summary>
        /// <param name="selBorrar">array con los identificadores de los cursos escolares que se deesean borrar</param>
        /// <returns>Redirecciona a la vista index</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarCursoEscolar(int[] selBorrar)
        {
            foreach (int item in selBorrar)//Se recorrer el array
            {
                //Se recupera el curso escolar seg�n el id
                var cursoEscolar = await _context.CursoEscolar.SingleOrDefaultAsync(m => m.Id == item);
                if (cursoEscolar != null)
                {
                    try
                    {
                        //Se elimina del modelo el curso escolar a borrar
                        _context.CursoEscolar.Remove(cursoEscolar);
                        //Se graba en BBDD
                        await _context.SaveChangesAsync();
                    }
                    catch(DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "Error al actualizar los datos. Int�ntelo pasado unos instantes, si persiste el error p�ngase en contacto con el administrador del sistema. (BorrarCursoEscolar-01)");
                    }

                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// M�todo que comprueba que existe el cursos escolar seg�n el identificador recibido
        /// </summary>
        /// <param name="id">Identificador del curso escolar que se desea comprobar su existencia</param>      
        private bool CursoEscolarExiste(int id)
        {
            return _context.CursoEscolar.Any(e => e.Id == id);
        }
    }
}
