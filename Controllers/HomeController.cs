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

            //EmpleadoVM oEmpleadoVM = new EmpleadoVM()
            //{
            //    oListaEmpleado = _DBcontext.Empleados.Include(c => c.oCargo).ToList()
            //};

            //return View(oEmpleadoVM);

            return View();
        }

        // Obtener lista de todos los empleados
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

        // Obtener cargos para el combo box en la vista
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

        // Guardar nuevo empleado
        [HttpPost]
        public IActionResult GuardarEmpleado([FromBody] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                if (empleado == null)
                {
                    return BadRequest(new { success = false, message = "Datos inválidos" });
                }

                try
                {
                    // Datos del empleado listo para guardae
                    _DBcontext.Empleados.Add(empleado);
                    _DBcontext.SaveChanges();

                    return Ok(new { success = true, message = "Empleado guardado correctamente" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Error al guardar el empleado", error = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "Error en los datos del empleado" });
            }
        }

        // Obtener empleado por Id para editar
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

        // Actualizar empleado
        [HttpPost]
        public IActionResult ActualizarEmpleado([FromBody] Empleado empleado)
        {
            try
            {
                // Buscar al empleado existente en la base de datos
                var empleadoExistente = _DBcontext.Empleados.Find(empleado.IdEmpleado);
                if (empleadoExistente == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado." });
                }

                // Actualizar los datos del empleado
                empleadoExistente.NombreCompleto = empleado.NombreCompleto;
                empleadoExistente.Correo = empleado.Correo;
                empleadoExistente.Telefono = empleado.Telefono;
                empleadoExistente.IdCargo = empleado.IdCargo;

                // Guardar los cambios en la base de datos
                _DBcontext.SaveChanges();

                return Json(new { success = true, message = "Empleado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al actualizar el empleado.", error = ex.Message });
            }
        }

        // Eliminar empleado
        [HttpPost]
        public JsonResult EliminarEmpleado(int idEmpleado)
        {
            try
            {    
                // Buscar el empleado en la base de datos por su ID
                var empleado = _DBcontext.Empleados.SingleOrDefault(e => e.IdEmpleado == idEmpleado);

                if (empleado != null)
                {
                    // Eliminar el empleado de la base de datos
                    _DBcontext.Empleados.Remove(empleado);

                    // Guardar los cambios en la base de datos
                    _DBcontext.SaveChanges();

                    return Json(new { success = true, message = "Empleado eliminado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "El empleado no existe." });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hubo un problema al eliminar el empleado: " + ex.Message });
            }
        }
    }
}
