using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AdminPanel()
    {
        var viewModel = new AdminPanelViewModel
        {
            NewsArticles = await _context.NewsArticles.ToListAsync(),
            Users = await _context.Users.ToListAsync(),
            Comments = await _context.Comments.ToListAsync()
        };
        return View(viewModel);
    }

    public IActionResult CreateNews()
    {
        return View(new NewsArticle());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNews(NewsArticle article)
    {
        if (ModelState.IsValid)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdminPanel");
        }
        return View(article);
    }

    public async Task<IActionResult> EditNews(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.NewsArticles.FindAsync(id);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditNews(int id, NewsArticle article)
    {
        if (id != article.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPanel");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(article);
    }

    public async Task<IActionResult> DeleteNews(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.NewsArticles.FirstOrDefaultAsync(m => m.Id == id);
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    [HttpPost, ActionName("DeleteNews")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.NewsArticles.FindAsync(id);
        if (article != null)
        {
            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(AdminPanel));
    }

    public async Task<IActionResult> EditUser(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminPanel));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(user);
    }

    public async Task<IActionResult> DeleteUser(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (user == null || user.IsAdmin)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null && !user.IsAdmin)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(AdminPanel));
    }

    public async Task<IActionResult> EditComment(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(int id, Comment comment)
    {
        if (id != comment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminPanel));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(comment);
    }

    public async Task<IActionResult> DeleteComment(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FirstOrDefaultAsync(m => m.Id == id);
        if (comment == null)
        {
            return NotFound();
        }

        return View(comment);
    }

    [HttpPost, ActionName("DeleteComment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCommentConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(AdminPanel));
    }

    private bool ArticleExists(int id)
    {
        return _context.NewsArticles.Any(e => e.Id == id);
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}
