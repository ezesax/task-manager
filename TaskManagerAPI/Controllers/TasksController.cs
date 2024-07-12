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
            return _context.Tasks;
        }

        // GET: api/Tasks/5
        public IHttpActionResult GetTask(int id)
        {
            Task task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // POST: api/Tasks
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        }

        // PUT: api/Tasks/5
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Tasks/5
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(task);
        }
    }
}
