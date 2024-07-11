using System.Data.Entity;
using System.Net;
using System.Web.Http;
using System.Linq;
using TaskManagerAPI.Models;

public class ProjectsController : ApiController
{
    private DBContext _context = new DBContext();

    // GET: api/projects
    public IHttpActionResult GetProjects()
    {
        var projects = _context.Projects.ToList();
        return Ok(projects);
    }

    // GET: api/projects/5
    public IHttpActionResult GetProject(int id)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    // POST: api/projects
    public IHttpActionResult PostProject(Project project)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Projects.Add(project);
        _context.SaveChanges();

        return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
    }

    // PUT: api/projects/5
    public IHttpActionResult PutProject(int id, Project project)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != project.Id)
        {
            return BadRequest();
        }

        _context.Entry(project).State = EntityState.Modified;
        _context.SaveChanges();

        return StatusCode(HttpStatusCode.NoContent);
    }

    // DELETE: api/projects/5
    public IHttpActionResult DeleteProject(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return Ok(project);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}
