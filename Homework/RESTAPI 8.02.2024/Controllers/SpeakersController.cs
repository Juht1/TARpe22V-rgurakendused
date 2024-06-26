using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly DataContext _context;

    public SpeakersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Speaker>> GetSpeakers(string? Email = null, string? name = null)
    {
        
        var query = _context.Speakers!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.name != null && x.name.ToUpper().Contains(name.ToUpper()));

        if (Email != null)
            query = query.Where(x => x.Email != null && x.Email.ToUpper().Contains(Email.ToUpper()));

        return query.ToList();
         
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetSpeaker(int id)
    {
        var speaker = _context.Speakers!.Find(id);

        if (speaker == null)
        {
            return NotFound();
        }

        return Ok(speaker);
    }

    [HttpPut("{id}")]
    public IActionResult PutSpeaker(int id, Speaker speaker)
    {
        var dbSpeaker = _context.Speakers!.AsNoTracking().FirstOrDefault(x => x.id == speaker.id);
        if (id != speaker.id || dbSpeaker == null)
        {
            return NotFound();
        }

        _context.Update(speaker);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Speaker> PostSpeaker(Speaker speaker)
    {
        var dbExercise = _context.Speakers!.Find(speaker.id);
        if (dbExercise == null)
        {
            _context.Add(speaker);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSpeaker), new { Id = speaker.id }, speaker);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSpeaker(int id)
    {
        var speaker = _context.Speakers!.Find(id);
        if (speaker == null)
        {
            return NotFound();
        }

        _context.Remove(speaker);
        _context.SaveChanges();

        return NoContent();
    }
}
