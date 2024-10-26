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
            List<Empleado> listaEmpleado = _DBcontext.Empleados.Include(c => c.oCargo).ToList();
            //List<Cargo> listaCargo = _DBcontext.Cargos.ToList();
            return View(listaEmpleado);
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
    }
}
