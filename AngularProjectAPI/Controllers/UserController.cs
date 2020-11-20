using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using AngularProjectAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly NewsContext _context;

        public UserController(IUserService userService, NewsContext context)
        {
            _userService = userService;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (roleID != 3)
            {
                return Unauthorized();
            }
            var username = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/Role/2
        [Authorize]
        [HttpGet("Role/{roleId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserArticles(int roleId)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (roleID != 3)
            {
                return Unauthorized();
            }

            var users = await _context.Users.Where(x => x.RoleID == roleId).ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        //do not authorize
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            user.Password = savedPasswordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpPost("Journalist")]
        public async Task<ActionResult<User>> PostJournalist(User user)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (roleID != 3)
            {
                return Unauthorized();
            }

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            user.Password = savedPasswordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            //getuser
            User user = _context.Users.Where(x => x.Username == userParam.Username).FirstOrDefault();
            /* Fetch the stored value */
            string savedPasswordHash = user.Password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(userParam.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return Unauthorized();


            var userAuthenticate = _userService.Authenticate(userParam.Username, savedPasswordHash);

            if (userAuthenticate == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            int roleID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

            if (roleID != 3)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(id);

            var articles = await _context.Articles.Where(x => x.UserID == user.UserID).ToListAsync();

            foreach (Article article in articles)
            {
                var articleComments = await _context.Comments.Where(x => x.ArticleID == article.ArticleID).ToListAsync();
                var articleLikes = await _context.Likes.Where(x => x.ArticleID == article.ArticleID).ToListAsync();

                foreach (Like like in articleLikes)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }

                foreach (Comment comment in articleComments)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }

            var comments = await _context.Comments.Where(x => x.UserID == user.UserID).ToListAsync();
            var likes = await _context.Likes.Where(x => x.UserID == user.UserID).ToListAsync();

            foreach (Like like in likes)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }

            foreach (Comment comment in comments)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}