using System.Runtime.Intrinsics.Arm;
using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendeesController : ControllerBase
{
    private readonly DataContext _context;

    public AtendeesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Atendee>> GetAtendeess(string? Email = null, string? name = null)
    {
        
        var query = _context.Atendees!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        if (Email != null)
            query = query.Where(x => x.Email != null && x.Email.ToUpper().Contains(Email.ToUpper()));

        return query.ToList();
         
    }
    [HttpGet("{iD}")]
    public ActionResult<TextReader> GetAtendees(int iD)
    {
        var atendee = _context.Atendees!.Find(iD);

        if (atendee == null)
        {
            return NotFound();
        }

        return Ok(atendee);
    }

    [HttpPut("{iD}")]
    public IActionResult PutEvent(int iD , Atendee atendee)
    { 
        var dbaAtendee = _context.Atendees!.AsNoTracking().FirstOrDefault(x => x.Id == atendee.Id);
        if (iD != atendee.Id || dbaAtendee == null)

        {
            return NotFound();
        }

        _context.Update(atendee);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Atendee> PostTest(Atendee atendee)
    {
        var dbExercise = _context.Atendees!.Find(atendee.Id);
        if (dbExercise == null)
        {
            _context.Add(atendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAtendees), new { Id = atendee.Id }, atendee);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{iD}")]
    public IActionResult DeleteTest(int Id)
    {
        var Event = _context.Atendees!.Find(Id);
        if (Event == null)
        {
            return NotFound();
        }

        _context.Remove(Atendees);
        _context.SaveChanges();

        return NoContent();
    }
}

