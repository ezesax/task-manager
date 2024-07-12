using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    public class TasksController : ApiController
    {
        private DBContext _context = new DBContext();

        // GET: api/Tasks
        public IQueryable<Task> GetTasks()
        {
            return _context.Tasks;  // Devuelve todas las tareas
        }

        // GET: api/Tasks/5
        public IHttpActionResult GetTask(int id)
        {
            Task task = _context.Tasks.Find(id);  // Busca una tarea por ID
            if (task == null)
            {
                return NotFound();  // Devuelve 404 si la tarea no se encuentra
            }

            return Ok(task);  // Devuelve la tarea encontrada
        }

        // POST: api/Tasks
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Valida el modelo antes de agregar la tarea
            }

            _context.Tasks.Add(task);  // Agrega una nueva tarea
            _context.SaveChanges();  // Guarda los cambios en la base de datos

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);  // Devuelve una respuesta con la tarea creada
        }

        // PUT: api/Tasks/5
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Valida el modelo antes de actualizar la tarea
            }

            if (id != task.Id)
            {
                return BadRequest();  // Verifica que el ID de la tarea coincida con el ID en la URL
            }

            _context.Entry(task).State = System.Data.Entity.EntityState.Modified;  // Marca la tarea como modificada
            _context.SaveChanges();  // Guarda los cambios en la base de datos

            return StatusCode(HttpStatusCode.NoContent);  // Devuelve un estado 204
        }

        // DELETE: api/Tasks/5
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = _context.Tasks.Find(id);  // Busca una tarea por ID
            if (task == null)
            {
                return NotFound();  // Devuelve 404 si la tarea no se encuentra
            }

            _context.Tasks.Remove(task);  // Elimina la tarea
            _context.SaveChanges();  // Guarda los cambios en la base de datos

            return Ok(task);  // Devuelve la tarea eliminada
        }
    }
}
