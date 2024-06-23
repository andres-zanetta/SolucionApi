using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using Proyecto_Modelo;


namespace ProyectoWebApii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly Empleado_Data _empleadoData;

        public EmpleadoController(Empleado_Data empleado_Data ) //INYECCION DE DEPENDENCIA
        {
            _empleadoData = empleado_Data;
        }

        [HttpGet] 
        public async Task<ActionResult> Lista()
        {
            List<Empleado> Lista = await _empleadoData.Lista();
            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody]Empleado objeto)
        {
            bool respuesta = await _empleadoData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new {isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<ActionResult> Editar([FromBody] Empleado objeto)
        {
            bool respuesta = await _empleadoData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            bool respuesta = await _empleadoData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
