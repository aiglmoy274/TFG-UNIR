using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TFG.Models;
using TFG.Models.ViewModels;
using TFG.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TFG.Controllers
{
    /// <summary>
    /// Clase que contiene los métodos para la gestión de usuarios
    /// </summary>
    [Authorize(Roles = "Administrador")]//El acceso de la clase sólo está autorizado a usuarios con perfil de Administrador
    public class UsuarioController : Controller
    {

        
        /// <summary>
        /// Objeto que suministra acceso a la API que permite la gestión de usuarios almacenados de forma permanente
        /// </summary>        
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Objeto que permite la gestión de los objetos contenidos en el modelo
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Objeto que suministra acceso a la API que permite la gestión de roles almacenados de forma permanente
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor que se encarga de inicializar las propiedades de la clase
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="context"></param>
        public UsuarioController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;

            
        }

        /// <summary>
        /// Carga una lista de usuarios, para cada usuario se recupera: el Id, el Código del Centro, el Nombre del Centro y la comunidad autonoma a la que pertenece
        /// </summary>
        /// <returns>Vista con la lista de UsuarioViewModel</returns>
        public IActionResult Index()
        {       
            


            List<UsuarioViewModel> lstUsuario = new List<UsuarioViewModel>();
            //Carga los usuarios del sistema en una lista de UsuarioViewModel
            lstUsuario = _userManager.Users.Select(r => new
            {
                CodigoCentro = r.CodigoCentro,
                NombreCentro = r.NombreCentro,
                DescripcionComunidadAutonoma = r.ComunidadAutonoma.Descripcion,
                Id = r.Id
            }
                ).OrderBy(x=>x.NombreCentro)
            .Select(r=>new UsuarioViewModel
            {
                Id=r.Id,
                CodigoCentro = r.CodigoCentro,
                NombreCentro = r.NombreCentro,                
                DescripcionComunidadAutonoma = r.DescripcionComunidadAutonoma
                
            }).ToList();
            return View(lstUsuario);
        }

        /// <summary>
        /// Método que se encarga de cargar un formulario con los datos vacios en caso de que se vaya a crear un usuario o relleno con los datos
        /// del usuario determinado por id en caso de querer editar
        /// </summary>
        /// <param name="id">Id del usuario que se desea editar</param>
        /// <returns>Vista con los datos del usuario a crear o editar</returns>
        [HttpGet]
        public async Task<IActionResult> CrearEditarUsuario(string id)
        {
            //Se crea un objeto de tipo UsuarioViewModel que tendrá los datos a pasar a la vista
            UsuarioViewModel model = new UsuarioViewModel();

            if (!String.IsNullOrEmpty(id)) //Se recibe el ID del usuario=>EDITAR
            {
                
                ApplicationUser usuario = await _userManager.FindByIdAsync(id);//Se recuperan los datos del usuario a editar
                if (usuario != null)
                { 
                    //Se cargan los datos del usuario a editar en el objeto de tipo UsuarioViewModel que se pasará a la vista
                    model.Id = usuario.Id;
                    model.CodigoCentro = usuario.CodigoCentro;
                    model.Email = usuario.Email;
                    model.NombreCentro = usuario.NombreCentro;         
                    //Se recuperan las Comunidades Autónomas que se pasarán a la vista para que sean mostrada en un SELECT y se selecciona la comunidad autónoma del usuario a editar
                    ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x=>x.Descripcion), "ComunidadAutonomaId", "Descripcion", usuario.ComunidadAutonomaId);

                    string NombrePerfil = await GetRole(usuario); //Se recupera la lista de roles del usuario a editar
                    //Se crea un objeto SelectList para mostrar la lista de Roles del sistema en un SELECT apareciendo seleccionado el rol del usuario a editar
                    ViewBag.Perfiles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name",NombrePerfil);
                }
            }
            else //NUEVO USUARIO
            {
                //Se recupera la lista de comunidades autónomas para mostrar a través de un SELECT en la vista
                ViewBag.ComunidadAutonomaId = new SelectList(_context.ComunidadAutonoma.OrderBy(x => x.Descripcion), "ComunidadAutonomaId", "Descripcion");
                //Se recupera la lista de roles para mostrar a través de un SELECT en la vista
                ViewBag.Perfiles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            }

            return View(model);            
        }

        /// <summary>
        /// Método que se encarga de crear o editar los datos del usuario recibidos de la vista una vez se ha pulsado el botón GUARDAR        
        /// </summary>
        /// <param name="id">Identificador del usuario a Editar, en caso de ser una acción de crear un usuario se recibirá como NULL</param>
        /// <param name="ComunidadAutonomaId">Id de la comunidad autónoma del usuario</param>
        /// <param name="PerfilNombre">Nombre del rol al que pertenece el usuario</param>
        /// <param name="model">Datos introducidos en el formulario sobre el usuario para que sea grabado en BBDD</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Evita que se llame al método desde un lugar distinto al del formulario creado vía GET
        public async Task<IActionResult> CrearEditarUsuario(string id,int ComunidadAutonomaId,string PerfilNombre, UsuarioViewModel model)
        {
            if (ModelState.IsValid)//Se comprueba que los datos enviados en el formulario son correcto
            {
                if (String.IsNullOrEmpty(id)) //Nuevo usuario
                {
                    ApplicationUser usuario = new ApplicationUser();
                    usuario.Email = model.Email;
                    usuario.CodigoCentro = model.CodigoCentro;
                    usuario.NombreCentro = model.NombreCentro;
                    usuario.UserName = model.CodigoCentro;
                    usuario.ComunidadAutonomaId = ComunidadAutonomaId;   
                    
                    //TODO: Generar password automática y envío de mail.
                    string password = "Unir+1234";


                    
                    IdentityResult usuarioResultado = await _userManager.CreateAsync(usuario,password);  //Se crea el usuario                                      
                    if (usuarioResultado.Succeeded)//Usuario creado de forma correcta
                    {
                        
                        usuarioResultado = await _userManager.AddToRoleAsync(usuario, PerfilNombre); //Se asigna el rol al usuario
                        if (usuarioResultado.Succeeded)//Rol asignado correctamente
                        {
                            return RedirectToAction("Index");//Se redirige al Index del controlador para cargar la lista de usuarios
                        }
                        
                    }
                }
                else //Editar usuario
                {
                    ApplicationUser usuario = await _userManager.FindByIdAsync(id);//Se recuperan los datos del usuario a editar
                    //Se asignan los datos introducios en el formulario
                    usuario.Email = model.Email;
                    usuario.CodigoCentro = model.CodigoCentro;
                    usuario.UserName = model.CodigoCentro;
                    usuario.NombreCentro = model.NombreCentro;
                    usuario.ComunidadAutonomaId = ComunidadAutonomaId;
                    

                    IdentityResult usuarioResultado = await _userManager.UpdateAsync(usuario);//Se actualizan los datos del usuario
                    if (usuarioResultado.Succeeded) //Usuario actualizado correctamente
                    {

                        IList<string> lstRoles = await _userManager.GetRolesAsync(usuario);//Se recupera la lista de roles del usuario
                        IdentityResult identityResultado = await _userManager.RemoveFromRolesAsync(usuario, lstRoles);//Se borran los roles del usuario

                        if (identityResultado.Succeeded)//Roles borrado correctamente
                        {
                            usuarioResultado = await _userManager.AddToRoleAsync(usuario, PerfilNombre);//Se asigna el nuevo rol al usuario
                            if (usuarioResultado.Succeeded)//Rol asignado correctamente
                            {
                                return RedirectToAction("Index");//Se redirige al Index del controlador para cargar la lista de usuarios
                            }
                            
                        }
                    }
                }
            }
            //Si ha habido algún error se vuelve a la vista y se muestran los errores
            return View(model);
        }

        /// <summary>
        /// Recupera el rol asignado al usuario
        /// </summary>
        /// <param name="usuario">Usuario sobre el que se desea recuperar el rol</param>
        /// <returns>Nombre del rol asignado al usuario</returns>
        private async Task<string> GetRole (ApplicationUser usuario)
        {
            string NombrePerfil = "";
            if (usuario != null)//Se ha recibido un usuario para hacer la consulta
            { 
                IList<string> lstPerfiles = await _userManager.GetRolesAsync(usuario);//Se obtiene la lista de roles del usuario

                if (lstPerfiles.FirstOrDefault() != null)
                {
                    NombrePerfil = lstPerfiles.FirstOrDefault();//Se recupera el primer perfil de la lista
                }
            }
            return NombrePerfil;//Devuelve el nombre del perfil
        }

    }
}