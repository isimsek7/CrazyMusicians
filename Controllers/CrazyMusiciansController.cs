using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CrazyMusicians.Models;

namespace CrazyMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrazyMusiciansController : ControllerBase
    {
        public static List<Musician> _musicians = new()
        {
            new Musician { ID = 1, Name = "Ahmet Çalgı", Profession = "Famous Instrument Player", FunFeature = "Always plays the wrong note, but very entertaining" },
            new Musician { ID = 2, Name = "Zeynep Melodi", Profession = "Popular Melody Writer", FunFeature = "Her songs are often misunderstood but very popular" },
            new Musician { ID = 3, Name = "Cemil Akor", Profession = "Crazy Chordist", FunFeature = "Changes chords frequently, but surprisingly talented" },
            new Musician { ID = 4, Name = "Fatma Nota", Profession = "Surprise Note Producer", FunFeature = "Always prepares surprises while producing notes" },
            new Musician { ID = 5, Name = "Hasan Ritim", Profession = "Rhythm Monster", FunFeature = "Plays every rhythm in his own style, doesn't fit but it's funny" },
            new Musician { ID = 6, Name = "Elif Armoni", Profession = "Harmony Master", FunFeature = "Sometimes plays harmonies wrong, but very creative" },
            new Musician { ID = 7, Name = "Ali Perde", Profession = "Pitch Performer", FunFeature = "Plays each pitch in a different way; always surprising" },
            new Musician { ID = 8, Name = "Ayşe Rezonans", Profession = "Resonance Expert", FunFeature = "Expert in resonance, but sometimes makes a lot of noise" },
            new Musician { ID = 9, Name = "Murat Ton", Profession = "Tone Enthusiast", FunFeature = "Differences in his tones are sometimes funny but very interesting" },
            new Musician { ID = 10, Name = "Selin Akor", Profession = "Chord Wizard", FunFeature = "Creates a magical atmosphere when changing chords" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Musician>> GetAllMusicians() => Ok(_musicians);

        [HttpGet("{id:int}")]
        public ActionResult<Musician> GetMusicianById(int id) =>
            _musicians.FirstOrDefault(m => m.ID == id) is Musician musician
                ? Ok(musician)
                : NotFound();

        [HttpGet("search")]
        public ActionResult<IEnumerable<Musician>> SearchMusicians([FromQuery] string? name, [FromQuery] string? profession)
        {
            var result = _musicians.AsEnumerable();

            if (!string.IsNullOrEmpty(name))
                result = result.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(profession))
                result = result.Where(m => m.Profession.Contains(profession, StringComparison.OrdinalIgnoreCase));

            return result.Any() ? Ok(result.ToList()) : NotFound("No musicians found matching the search criteria.");
        }

        [HttpPost]
        public ActionResult<Musician> CreateMusician([FromBody] Musician newMusician)
        {
            if (_musicians.Any(m => m.ID == newMusician.ID))
                return Conflict("Musician with this ID already exists.");

            _musicians.Add(newMusician);
            return CreatedAtAction(nameof(GetMusicianById), new { id = newMusician.ID }, newMusician);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateMusician(int id, [FromBody] Musician updatedMusician)
        {
            var musician = _musicians.FirstOrDefault(m => m.ID == id);
            if (musician is null) return NotFound();

            musician.Name = updatedMusician.Name;
            musician.Profession = updatedMusician.Profession;
            musician.FunFeature = updatedMusician.FunFeature;

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PatchMusician(int id, [FromBody] string funFeature)
        {
            var musician = _musicians.FirstOrDefault(m => m.ID == id);
            if (musician is null) return NotFound();

            musician.FunFeature = funFeature;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMusician(int id)
        {
            var musician = _musicians.FirstOrDefault(m => m.ID == id);
            if (musician is null) return NotFound();

            _musicians.Remove(musician);
            return NoContent();
        }
    }
}
