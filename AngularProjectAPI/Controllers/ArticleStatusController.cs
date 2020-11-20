using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleStatusController : ControllerBase
    {
        private readonly NewsContext _context;

        public ArticleStatusController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/ArticleStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleStatus>>> GetArticleStatuses()
        {
            return await _context.ArticleStatuses.ToListAsync();
        }

        // GET: api/ArticleStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleStatus>> GetArticleStatus(int id)
        {
            var articleStatus = await _context.ArticleStatuses.FindAsync(id);

            if (articleStatus == null)
            {
                return NotFound();
            }

            return articleStatus;
        }      
    }
}
