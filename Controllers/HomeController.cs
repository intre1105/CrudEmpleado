using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBCRUDEMPLEADO.Models;
using DBCRUDEMPLEADO.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DBCRUDEMPLEADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbcrudempleadoContext _DBcontext;

        public HomeController(DbcrudempleadoContext context)
        {
            _DBcontext = context;
        }

        public IActionResult Index()
        {
            //List<Empleado> listaEmpleado = _DBcontext.Empleados.Include(c => c.oCargo).ToList();
            //List<Cargo> listaCargo = _DBcontext.Cargos.ToList();

            EmpleadoVM oEmpleadoVM = new EmpleadoVM()
            {
                oListaEmpleado = _DBcontext.Empleados.Include(c => c.oCargo).ToList()
            };

            return View(oEmpleadoVM);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetEmpleados()
        //{
        //    try
        //    {
        //        var empleados = await _DBcontext.Empleados.Include(e => e.oCargo).ToListAsync();
        //        return Json(empleados);
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        Debug.WriteLine($"Error en la base de datos: {dbEx.Message}");
        //        return StatusCode(500, "Error en la base de datos");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error al obtener empleados: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        // Traer un elemento empleado vacio con una lista de cargo 
        [HttpGet]
        public IActionResult Empleado_Detalle()
        {
            EmpleadoVM oEmpleadoVM = new EmpleadoVM()
            {
                oEmpleado = new Empleado(),
                oListaCargo = _DBcontext.Cargos.Select(cargo => new SelectListItem()
                {
                    Text = cargo.Descripcion,
                    Value = cargo.IdCargo.ToString()
                }).ToList()
            };

            return View(oEmpleadoVM);
        }

        // Guardar en la DBCRUDEMPLEADO
        [HttpPost]
        public IActionResult Empleado_Detalle(EmpleadoVM oEmpleadoVM)
        {
            if (oEmpleadoVM.oEmpleado.IdEmpleado == 0)
            {
                _DBcontext.Empleados.Add(oEmpleadoVM.oEmpleado);
            }

            _DBcontext.SaveChanges();

            // Redireccional vista
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult GetEmpleados()
        {
            var empleados = _DBcontext.Empleados
                .Include(e => e.oCargo)
                .Select(e => new
                {
                    e.IdEmpleado,
                    e.NombreCompleto,
                    e.Correo,
                    e.Telefono,
                    CargoDescripcion = e.oCargo != null ? e.oCargo.Descripcion : "Sin Cargo"
                })
                .ToList();

            return Json(empleados);
        }

        [HttpGet]
        public JsonResult GetEmpleadoById(int idEmpleado)
        {
            var empleado = _DBcontext.Empleados
                .Include(e => e.oCargo)
                .Where(e => e.IdEmpleado == idEmpleado)
                .Select(e => new
                {
                    e.IdEmpleado,
                    e.NombreCompleto,
                    e.Correo,
                    e.Telefono,
                    IdCargo = e.oCargo != null ? e.oCargo.IdCargo : (int?)null,
                    CargoDescripcion = e.oCargo != null ? e.oCargo.Descripcion : "Sin Cargo"
                })
                .FirstOrDefault();

            if (empleado == null)
            {
                return Json(new { success = false, message = "Empleado no encontrado" });
            }

            return Json(new { success = true, empleado });
        }

        [HttpGet]
        public JsonResult GetCargos()
        {
            var cargos = _DBcontext.Cargos
                .Select(c => new
                {
                    c.IdCargo,
                    c.Descripcion
                })
                .ToList();

            return Json(cargos);
        }
    }
}
