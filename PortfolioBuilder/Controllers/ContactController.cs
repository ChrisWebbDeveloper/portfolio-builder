using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.Data;
using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace PortfolioBuilder.Controllers
{
    public class ContactController : ApplicationController
    {
        public ContactController(ApplicationDbContext context) : base(context)
        {

        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            var contact = await _context.ContactDetails
                .SingleOrDefaultAsync(m => m.Id == 1);
            return View(contact);
        }
        // GET: Contact/Edit
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var contact = await _context.ContactDetails.FindAsync(1);
            return View(contact);
        }

        // POST: Contact/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Message,Name,Location,Email,PhoneNo,Facebook,Instagram,LinkedIn")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }
    }
}
