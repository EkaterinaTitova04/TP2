using JuliePro.Data;
using JuliePro.Models;
using JuliePro.Models.ViewModels;
using JuliePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Linq;
using System.Threading.Tasks;

namespace JuliePro.Controllers
{
    public class TrainersController : Controller
    {
        private readonly JulieProDbContext _context;
        private readonly IServiceBaseAsync<Trainer> _trainerService;


        public TrainersController(JulieProDbContext context, IServiceBaseAsync<Trainer> trainerService)
        {
            _context = context;
            _trainerService = trainerService;
        }

        [HttpPost]
        public async Task<IActionResult> Filter(TrainerSearchViewModelFilter filter)
        {
            var trainers = await _trainerService.GetFilteredTrainersAsync(filter);

            var certificationCenters = await _trainerService.GetCertificationCentersAsync();
            ViewBag.CertificationCenters = new SelectList(certificationCenters);

            return View("Index", trainers);
        }

        // GET: Trainers
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10; 

            var trainers = _context.Trainer.Include(t => t.Speciality).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                trainers = trainers.Where(t => t.FirstName.Contains(searchString) || t.LastName.Contains(searchString));
            }

            var pagedTrainers = await trainers.OrderBy(t => t.LastName).ToPagedListAsync(pageNumber, pageSize);

            var model = new TrainerSearchViewModelFilter
            {
                Items = pagedTrainers,
                SearchString = searchString
            };

            var certificationCenters = await _trainerService.GetCertificationCentersAsync();
            ViewBag.CertificationCenters = new SelectList(certificationCenters);

            return View(model);
        }


        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .Include(t => t.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            ViewData["SpecialityId"] = new SelectList(_context.Speciality, "Id", "Name");
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Photo,SpecialityId")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialityId"] = new SelectList(_context.Speciality, "Id", "Name", trainer.SpecialityId);
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            ViewData["SpecialityId"] = new SelectList(_context.Speciality, "Id", "Name", trainer.SpecialityId);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Photo,SpecialityId")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialityId"] = new SelectList(_context.Speciality, "Id", "Name", trainer.SpecialityId);
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .Include(t => t.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainer.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainer.Remove(trainer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
          return (_context.Trainer?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }
    }
}
