using System.Runtime.Intrinsics.Arm;
using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendeesController : ControllerBase
{
    private readonly DataContext _context;

    public AttendeesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Attendee>> GetAttendees(string? Email = null, string? Name = null)
    {
        
        var query = _context.Attendees!.AsQueryable();

        if (Name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(Name.ToUpper()));

        if (Email != null)
            query = query.Where(x => x.Email != null && x.Email.Remove(-10).Contains(Email.Remove(-10)));

        return query.ToList();
         
    }
    [HttpGet("{id}")]
    public ActionResult<TextReader> GetAttendee(int id)
    {
        var Attendee = _context.Attendees!.Find(id);

        if (Attendee == null)
        {
            return NotFound();
        }

        return Ok(Attendee);
    }

    [HttpPut("{id}")]
    public IActionResult PutAttendee(int id , Attendee Attendee)
    { 
        var dbaAttendee = _context.Attendees!.AsNoTracking().FirstOrDefault(x => x.id == Attendee.id);
        if (id != Attendee.id || dbaAttendee == null)

        {
            return NotFound();
        }

        _context.Update(Attendee);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Attendee> PostAttendee(Attendee Attendee)
    {
        var dbExercise = _context.Attendees!.Find(Attendee.id);
        if (dbExercise == null)
        {
            if (!Attendee.Email!.Contains("@"))
            {return BadRequest();}
            _context.Add(Attendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendee), new { id = Attendee.id }, Attendee);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAttendee(int id)
    {
        var Attendee = _context.Attendees!.Find(id);
        if (Attendee == null)
        {
            return NotFound();
        }

        _context.Remove(Attendee);
        _context.SaveChanges();

        return NoContent();
    }
}

