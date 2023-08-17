using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;


namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly ApiTestContext _dbcontext;

        public ProductoController(ApiTestContext _context)
        {
            _dbcontext = _context;
        }
        [HttpGet]
        [Route("List")]

        public IActionResult Lista() {
            List<Producto> lista = new List<Producto>();

            try {
                lista = _dbcontext.Productos.Include(c => c.IdCategoriaNavigation).ToList(); 
                return StatusCode(statusCode: 200, new {mensaje = "Esta bien", Response = lista});
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 200, new { ex.Message, Response = lista });
            }
            }

        [HttpGet]
        [Route("Get*{IdProducto:int}")]

        public IActionResult Obtener(int IdProducto)
        {
            Producto Oproducto = _dbcontext.Productos.Find(IdProducto);

            if (Oproducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                Oproducto = _dbcontext.Productos.Include(c => c.IdCategoriaNavigation).Where(p => p.IdProducto == IdProducto).FirstOrDefault();
                return StatusCode(statusCode: 200, new { mensaje = "Esta bien", Response = Oproducto });
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 200, new { ex.Message, Response = Oproducto });
            }

        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try {
                _dbcontext.Productos.Add(objeto);
                _dbcontext.SaveChanges();
                return StatusCode(statusCode: 200, new { mensaje = "Esta bien" });
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 200, new { ex.Message,});
            }
        }



        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit([FromBody] Producto objeto)
        {
            Producto Oproducto = _dbcontext.Productos.Find(objeto.IdProducto);

            if (Oproducto == null)
            {
                return BadRequest("Producto no encontrado");
            }
            try
            {
                Oproducto.CodigoBarra = objeto.CodigoBarra is null ? Oproducto.CodigoBarra : objeto.CodigoBarra;
                Oproducto.Descripcion = objeto.Descripcion is null ? Oproducto.Descripcion : objeto.Descripcion;
                Oproducto.Marca = objeto.Marca is null ? Oproducto.Marca : objeto.Marca;
                Oproducto.IdCategoria = objeto.IdCategoria is null ? Oproducto.IdCategoria : objeto.IdCategoria;
                Oproducto.Precio = objeto.Precio is null ? Oproducto.Precio : objeto.Precio;

                _dbcontext.Productos.Update(Oproducto);
                _dbcontext.SaveChanges();
                return StatusCode(statusCode: 200, new { mensaje = "Esta bien" });
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 200, new { ex.Message, });
            }
        }

        [HttpDelete]
        [Route("Delete/{IdProducto:int}")]
        public IActionResult Eliminar(int IdProducto)
        {
            Producto Oproducto = _dbcontext.Productos.Find(IdProducto);

            if (Oproducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                _dbcontext.Productos.Remove(Oproducto);
                _dbcontext.SaveChanges();
                return StatusCode(statusCode: 200, new { mensaje = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 200, new { ex.Message });
            }
        }

    }

}
