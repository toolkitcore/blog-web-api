using Api.Core.Entities;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Presentation.Controller
{
    public class BlogController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync() => Ok(await _context.Blogs.Include(x => x.Topics).Take(3).ToListAsync());
        [HttpPost]
        public async Task<IActionResult> post(Blog request) => Ok(await _context.Blogs.Include(x => x.Topics).Take(3).ToListAsync());
    }
}