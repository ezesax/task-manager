using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using BCrypt.Net;
using TaskManagerAPI.Models;

public class UsersController : ApiController
{
    private DBContext _context = new DBContext();

    // GET: api/users
    public IHttpActionResult GetUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    // GET: api/users/5
    public IHttpActionResult GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/users
    public IHttpActionResult PostUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
    }

    // PUT: api/users/5
    public IHttpActionResult PutUser(int id, User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != user.Id)
        {
            return BadRequest();
        }

        if (!string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        _context.Entry(user).State = EntityState.Modified;
        _context.SaveChanges();

        return StatusCode(HttpStatusCode.NoContent);
    }

    // DELETE: api/users/5
    public IHttpActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok(user);
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
