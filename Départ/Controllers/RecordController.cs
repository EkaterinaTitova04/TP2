using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JuliePro.Data;
using JuliePro.Models;
using JuliePro.Models.ViewModels;

namespace JuliePro.Controllers
{
    public class RecordController : Controller
    {
        private readonly JulieProDbContext _context;

        public RecordController(JulieProDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _context.Records
                .Include(r => r.Trainer)
                .Include(r => r.Customer)
                .ToListAsync();
            return View(records);
        }

        public IActionResult Create()
        {
            var viewModel = new RecordViewModel
            {
                TrainerList = new SelectList(_context.Trainer, "Id", "Name"),
                CustomerList = new SelectList(_context.Customer, "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.TrainerList = new SelectList(_context.Trainer, "Id", "Name", viewModel.Record.TrainerId);
            viewModel.CustomerList = new SelectList(_context.Customer, "Id", "Name", viewModel.Record.CustomerId);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = await _context.Records
                .Include(r => r.Trainer)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            var viewModel = new RecordViewModel
            {
                Record = record,
                TrainerList = new SelectList(_context.Trainer, "Id", "Name", record.TrainerId),
                CustomerList = new SelectList(_context.Customer, "Id", "Name", record.CustomerId)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecordViewModel viewModel)
        {
            if (id != viewModel.Record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    _context.Update(viewModel.Record);
                    await _context.SaveChangesAsync();
                
                
                    if (!RecordExists(viewModel.Record.Id))
                    {
                        return NotFound();
                    }

                
                return RedirectToAction(nameof(Index));
            }
            viewModel.TrainerList = new SelectList(_context.Trainer, "Id", "Name", viewModel.Record.TrainerId);
            viewModel.CustomerList = new SelectList(_context.Customer, "Id", "Name", viewModel.Record.CustomerId);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var record = await _context.Records
                .Include(r => r.Trainer)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(record);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.Records
                .Include(r => r.Trainer)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return View(record);
        }


        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
