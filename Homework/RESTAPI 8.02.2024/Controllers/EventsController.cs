using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly DataContext _context;

    public EventsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents(string? Email = null, string? name = null, string? Location = null)
    {
        var query = _context.Events!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.name != null && x.name.ToUpper().Contains(name.ToUpper()));

        if (Location != null)
            query = query.Where(x => x.Location != null && x.Location.ToUpper().Contains(Location.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetEvent(int id)
    {
        var Event = _context.Events!.Find(id);

        if (Event == null)
        {
            return NotFound();
        }

        return Ok(Event);
    }

    [HttpPut("{id}")]
    public IActionResult PutEvent(int id , Event events)
    { 
        var dbEvent = _context.Events!.AsNoTracking().FirstOrDefault(x => x.id == events.id);
        if (id != events.id || dbEvent == null)

        {
            return NotFound();
        }

        _context.Update(events);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Event> PostEvent(Event e)
    {
        var dbExercise = _context.Events!.Find(e.id);
        if (dbExercise == null)
        {
            _context.Add(e);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEvent), new { Id = e.id }, e);
        }
        else
        {
            return Conflict();
        }
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteEvent (int id)
    {
        var Event = _context.Events!.Find(id);
        if (Event == null)
        {
            return NotFound();
        }

        _context.Remove(Event);
        _context.SaveChanges();

        return NoContent();
    }
}

