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
    //Note til næste gang: Lav en ny model som er ment for DB kun, som indeholder Summonername + Region, scaffold det osv for support
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
            var getDB = await _context.Riot.ToListAsync();
            var list = getDB.Select(x => _Api.GetUserFromAPi(x)).ToList();

            return View(list);
        }

        // GET: Riot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riotModel = await _context.Riot
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
      //  public async Task<IActionResult> Create([Bind("Id,SummonerName,Region,Rank,LastMatch,RemainingGames,TimeRemain")] RiotModel riotModel)
        public async Task<IActionResult> Create([Bind("Id,SummonerName,Region")] RiotDBModel riotModel)
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

            var riotModel = await _context.Riot.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,SummonerName,Region")] RiotDBModel riotModel)
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

            var riotModel = await _context.Riot
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
            var riotModel = await _context.Riot.FindAsync(id);
            _context.Riot.Remove(riotModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiotModelExists(int id)
        {
            return _context.Riot.Any(e => e.Id == id);
        }
    }
}
