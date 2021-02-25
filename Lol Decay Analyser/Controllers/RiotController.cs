using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lol_Decay_Analyser.Data;
using Lol_Decay_Analyser.Models;
using Lol_Decay_Analyser.Helper_Classes;

namespace Lol_Decay_Analyser.Controllers
{
    public class RiotController : Controller
    {
        private readonly RiotContext _context;
        private readonly RiotConnection _Api;

        public RiotController(RiotContext context, RiotConnection Api)
        {
            _context = context;
            _Api = Api;
        }

        // GET: Riot
        public async Task<IActionResult> Index()
        {
            List<RiotModel> list = new List<RiotModel>();
            var test = await _context.Riots.ToListAsync();
            //add foreach
            foreach (var item in test)
            {
                list.Add(_Api.GetUserFromAPi(item));
            }
            return View(list);
        }

        // GET: Riot/Details/5
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

        // GET: Riot/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Riot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SummonerName,LastMatch,Rank,TimeRemain,Region")] RiotModel riotModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riotModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riotModel);
        }

        // GET: Riot/Edit/5
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

        // POST: Riot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SummonerName,LastMatch,Rank,TimeRemain,Region")] RiotModel riotModel)
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

        // GET: Riot/Delete/5
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

        // POST: Riot/Delete/5
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
