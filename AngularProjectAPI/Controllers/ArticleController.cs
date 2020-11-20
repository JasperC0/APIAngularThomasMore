using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly NewsContext _context;

        public ArticleController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Article/User/5
        [Authorize]
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetUserArticles(int userId)
        {
            var articles = await _context.Articles.Where(x => x.UserID == userId).ToListAsync();

            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (!(roleID.Equals(2) || roleID.Equals(3)))
            {
                return Unauthorized();
            }

            if (articles.Any())
            {
                int userID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
                if (!(roleID.Equals(3) || userID.Equals(articles[0].UserID)))
                {
                    return Unauthorized();
                }
            }
            if (articles == null)
            {
                return NotFound();
            }



            return articles;
        }

        // GET: api/Article/Status/1
        [Authorize]
        [HttpGet("Status/{statusId}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetStatusArticles(int statusId)
        {
            var articles = await _context.Articles.Include(x => x.Tag).Include(x => x.User).Where(x => x.ArticleStatusID == statusId).ToListAsync();

            if (articles == null)
            {
                return NotFound();
            }

            return articles;
        }

        // GET: api/Article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Article/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (!(roleID.Equals(2) || roleID.Equals(3)))
            {
                return Unauthorized();
            }

            int userID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
            if (!(roleID.Equals(3) || userID.Equals(article.UserID)))
            {
                return Unauthorized();
            }

            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Article
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (!(roleID.Equals(2) || roleID.Equals(3)))
            {
                return Unauthorized();
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.ArticleID }, article);
        }

        // DELETE: api/Article/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (!(roleID.Equals(2) || roleID.Equals(3)))
            {
                return Unauthorized();
            }

            var article = await _context.Articles.FindAsync(id);
            int userID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
            if (!(roleID.Equals(3) || userID.Equals(article.UserID)))
            {
                return Unauthorized();
            }

            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
