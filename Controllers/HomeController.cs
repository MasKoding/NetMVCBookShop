using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bookshop.Models;
using Microsoft.AspNetCore.Identity;
using bookshop.DTO;

namespace bookshop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IHomeRepository _homeRepository;

    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger,IHomeRepository homeRepository,UserManager<IdentityUser> userManager)
    {
        _homeRepository = homeRepository;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string sterm="",int genreId=0)
    {
        IEnumerable<Book> books = await _homeRepository.GetBooks(sterm,genreId);
        IEnumerable<Genre> genres = await _homeRepository.GetGenres();

        var user = await _userManager.FindByEmailAsync("admin@gmail.com");


        string userId = "";
        if(user != null)
        {
            userId = await _userManager.GetUserIdAsync(user);
        }

        BookDisplayModel bookModel = new BookDisplayModel
        {
            Genres = genres,
            Books = books,
            STerm = sterm,
            GenreId = genreId
        };
        ViewBag.Books = books;
        ViewBag.Genres = genres;
        ViewBag.UserId = userId;
        return View(bookModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

