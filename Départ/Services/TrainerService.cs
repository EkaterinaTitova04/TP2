using JuliePro.Data;
using JuliePro.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JuliePro.Services
{
    public class TrainerService : IServiceBaseAsync<Trainer>
    {
        private readonly JulieProDbContext _context; 

        public TrainerService(JulieProDbContext context)
        {
            _context = context;
        }

        public async Task<Trainer> CreateAsync(Trainer entity)
        {
            await _context.Set<Trainer>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {

            var entity = await this.GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<Trainer>().Remove(entity);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IReadOnlyList<Trainer>> GetAllAsync()
        {
            return await _context.Set<Trainer>().ToListAsync();
        }

        public async Task<Trainer?> GetByIdAsync(int id)
        {
            return await _context.Set<Trainer>().FindAsync(id);
        }

        public async Task EditAsync(Trainer entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _context.Update<Trainer>(entity);
            else _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task<List<Trainer>> GetAllAsync(TrainerSearchViewModelFilter filter)
        {
            var query = _context.Trainer.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name));
           
            return await query.ToListAsync();
        }

        public async Task<List<Trainer>> GetFilteredTrainersAsync(TrainerSearchViewModelFilter filter)
        {
            var query = _context.Trainer.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name));
            }
            return await query.ToListAsync();

        }

        public async Task<List<string>> GetCertificationCentersAsync()
        {
            return await _context.Certifications
                                 .Select(c => c.CertificationCenter)
                                 .Distinct()
                                 .ToListAsync();
        }
    }
}
