using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBCRUDEMPLEADO.Models.ViewModels
{
    public class EmpleadoVM
    {
        public Empleado? oEmpleado { get; set; }
        public Cargo? oCargo { get; set; } // Agregado para probar
        public List<Empleado>? oListaEmpleado { get; set; } // Agregado para probar
        public List<SelectListItem>? oListaCargo { get; set; } // Lista cargos para deplegable selet en vista              
    }
}
