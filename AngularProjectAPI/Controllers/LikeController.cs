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
    public class LikeController : ControllerBase
    {
        private readonly NewsContext _context;

        public LikeController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Like
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        // GET: api/Like/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }

        //don't authorize
        // GET: api/Like/Article/1
        [HttpGet("Article/{articleId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetlikesArticle(int articleId)
        {
            var likes = await _context.Likes.Where(x => x.ArticleID == articleId).ToListAsync();

            if (likes == null)
            {
                return NotFound();
            }

            return likes;
        }

        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesUser(int userId)

        {
            var likes = await _context.Likes.Where(x => x.UserID == userId).ToListAsync();

            if (likes == null)
            {
                return NotFound();
            }

            return likes;
        }

        // GET: api/Like/User/Article/1/1
        [HttpGet("User/Article/{userId}/{articleId}")]
        public async Task<ActionResult<Like>> GetUserArticleLike(int userId, int articleId)
        {
            var like = await _context.Likes.Where(x => x.UserID == userId).Where(x => x.ArticleID == articleId).FirstAsync();

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }

        // PUT: api/Like/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLike(int id, Like like)
        {
            if (id != like.LikeID)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(id))
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

        // POST: api/Like
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.LikeID }, like);
        }

        // DELETE: api/Like/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.LikeID == id);
        }
    }
}
