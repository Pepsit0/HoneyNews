using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var news = await _context.NewsArticles.ToListAsync();
        return View(news);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var newsArticle = await _context.NewsArticles
            .Include(n => n.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (newsArticle == null)
        {
            return NotFound();
        }

        return View(newsArticle);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int newsArticleId, string author, string content)
    {
        var comment = new Comment
        {
            NewsArticleId = newsArticleId,
            Author = author,
            Content = content,
            CommentDate = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = newsArticleId });
    }
}
