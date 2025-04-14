using APBD_05.Data;
using APBD_05.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalController : ControllerBase
{
    private readonly List<Animal> _animals = AnimalRepository.animals;
    private readonly List<Visits> _visits = VisitsRepository.visits;

    public IActionResult GetAll()
    {
        return Ok(_animals);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal == null) return NotFound();
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult add(Animal animalToAdd)
    {
        var id = _animals.Max(x => x.Id) + 1;
        var animals = new Animal { Id = id, Name = animalToAdd.Name, Category = animalToAdd.Category, Weight = animalToAdd.Weight,
            FurColor = animalToAdd.FurColor };
        _animals.Add(animals);
        return CreatedAtAction(nameof(GetById), new { id = id}, animals);
    }

    [HttpPut("{id}")]
    public IActionResult update(Animal animalToUpdate)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == animalToUpdate.Id);
        if (animal == null) return NotFound();
        animal.Name = animalToUpdate.Name;
        animal.Category = animalToUpdate.Category;
        animal.Weight = animalToUpdate.Weight;
        animal.FurColor = animalToUpdate.FurColor;
        return Ok(animal);
    }

    [HttpDelete("{id}")]
    public IActionResult delete(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal == null) return NotFound();
        _animals.Remove(animal);
        return NoContent();
    }
   
    
}

[ApiController]
[Route("api/animals/{animalId:int}/visits")]
public class VisitsController : ControllerBase
{
    private readonly List<Animal> _animals = AnimalRepository.animals;
    private readonly List<Visits> _visits = VisitsRepository.visits;
    
    [HttpGet]
    public IActionResult GetVisitsForAnimal(int animalId)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == animalId);
        if (animal == null) return NotFound();

        var animalVisits = _visits.Where(v => v.AnimalId == animalId).ToList();
        return Ok(animalVisits);
    }
    
    [HttpPost]
    public IActionResult AddVisitForAnimal(int animalId, Visits visit)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == animalId);
        if (animal == null) return NotFound();
        var newId = _visits.Max(x => x.Id) + 1;
        var newVisit = new Visits
        {
            Id = newId,
            AnimalId = animalId,
            DateOfVisit = visit.DateOfVisit,
            Describtion = visit.Describtion,
            Price = visit.Price
        };

        _visits.Add(newVisit);
        return CreatedAtAction(nameof(GetVisitsForAnimal), new { animalId = animalId }, newVisit);
    }

   
}