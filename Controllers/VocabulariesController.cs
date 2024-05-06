using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishApp.Data;
using EnglishApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace EnglishApp.Controllers
{
    public class VocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabulariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vocabularies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vocabulary.ToListAsync());
        }

        // GET: Vocabularies/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // POST: Vocabularies/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Vocabulary.Where(j=>j.VocabTitle.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Vocabularies/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabulary
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        // GET: Vocabularies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vocabularies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize] //Function called to insert new data into database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VocabTitle,VocabDescription")] Vocabulary vocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vocabulary);
        }

        // GET: Vocabularies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabulary.FindAsync(id);
            if (vocabulary == null)
            {
                return NotFound();
            }
            return View(vocabulary);
        }

        // POST: Vocabularies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VocabTitle,VocabDescription")] Vocabulary vocabulary)
        {
            if (id != vocabulary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabularyExists(vocabulary.Id))
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
            return View(vocabulary);
        }

        // GET: Vocabularies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabulary
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        // POST: Vocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabulary = await _context.Vocabulary.FindAsync(id);
            if (vocabulary != null)
            {
                _context.Vocabulary.Remove(vocabulary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabularyExists(int id)
        {
            return _context.Vocabulary.Any(e => e.Id == id);
        }
    }
}
