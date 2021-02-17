using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lol_Decay_Analyser.Data;
using Lol_Decay_Analyser.Models;

namespace Lol_Decay_Analyser.Controllers
{
    public class RiotController : Controller
    {
        private readonly RiotContext _context;

        public RiotController(RiotContext context)
        {
            _context = context;
        }

        // GET: RiotModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Riots.ToListAsync());
        }

        // GET: RiotModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riotModel = await _context.Riots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riotModel == null)
            {
                return NotFound();
            }

            return View(riotModel);
        }

        // GET: RiotModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RiotModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SummonerName")] RiotModel riotModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riotModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riotModel);
        }

        // GET: RiotModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riotModel = await _context.Riots.FindAsync(id);
            if (riotModel == null)
            {
                return NotFound();
            }
            return View(riotModel);
        }

        // POST: RiotModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SummonerName")] RiotModel riotModel)
        {
            if (id != riotModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(riotModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiotModelExists(riotModel.Id))
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
            return View(riotModel);
        }

        // GET: RiotModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riotModel = await _context.Riots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riotModel == null)
            {
                return NotFound();
            }

            return View(riotModel);
        }

        // POST: RiotModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riotModel = await _context.Riots.FindAsync(id);
            _context.Riots.Remove(riotModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiotModelExists(int id)
        {
            return _context.Riots.Any(e => e.Id == id);
        }
    }
}
