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
    public ActionResult<IEnumerable<Event>> GetEvents(string? name = null, DateTime? date = null, String? Location = null)
    {
        var query = _context.Events!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.name != null && x.name.ToUpper().Contains(name.ToUpper()));

        if (Location != null)
            query = query.Where(x => x.Location != null && x.Location.ToUpper().Contains(Location.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{iD}")]
    public ActionResult<TextReader> GetEvent(int iD)
    {
        var Event = _context.Events!.Find(iD);

        if (Event == null)
        {
            return NotFound();
        }

        return Ok(Event);
    }

    [HttpPut("{iD}")]
    public IActionResult PutEvent(int iD , Event events)
    { 
        var dbEvent = _context.Events!.AsNoTracking().FirstOrDefault(x => x.iD == events.iD);
        if (iD != events.iD || dbEvent == null)

        {
            return NotFound();
        }

        _context.Update(events);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Event> PostTest(Event events)
    {
        var dbExercise = _context.Events!.Find(events.iD);
        if (dbExercise == null)
        {
            _context.Add(events);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEvent), new { Id = events.iD }, events);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{iD}")]
    public IActionResult DeleteTest(int iD)
    {
        var Event = _context.Events!.Find(iD);
        if (Event == null)
        {
            return NotFound();
        }

        _context.Remove(Event);
        _context.SaveChanges();

        return NoContent();
    }
}

