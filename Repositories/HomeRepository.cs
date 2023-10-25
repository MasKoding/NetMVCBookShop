using System;
using System.Linq;

namespace bookshop.Repositories
{
	public class HomeRepository :IHomeRepository
	{

		private readonly ApplicationDbContext _db;

		public HomeRepository(ApplicationDbContext db)
		{
			this._db = db;
		}
		public async Task<IEnumerable<Book>> GetBooks(string sTerm="",int genreId = 0)
		{

			sTerm = sTerm.ToLower();
			IEnumerable<Book> books = await (from book in _db.Books
						 join genre in _db.Genres
						 on book.GenreId equals genre.Id
						 where string.IsNullOrWhiteSpace(sTerm) || (book!=null && book.BookName.ToLower().StartsWith(sTerm))
						 || (book!=null && book.BookName.ToLower().EndsWith(sTerm))


						 select new Book
						 {
							 Id = book.Id,
							 Image = book.Image,
							 AuthorName = book.AuthorName,
							 BookName = book.BookName,
							 GenreId = book.GenreId,
							 Price = book.Price,
							 GenreName = genre.GenreName
						 }
						 ).ToListAsync();


			if (genreId > 0)
			{
				books = books.Where(a => a.GenreId == genreId).ToList();
			}

			return books;

			
		}



        public async Task<IEnumerable<Genre>> GetGenres()
        {
			IEnumerable<Genre> genres = await _db.Genres.ToListAsync();

			return genres;
			
        }

		public async Task<IEnumerable<Book>> GetBooksFromGenre(string sterm)
		{
			var books = await(from b in _db.Books
						 join g in _db.Genres
						 on b.GenreId equals g.Id
						 where string.IsNullOrWhiteSpace(sterm)
						 select new Book
						 {
							 Id = b.Id,
							 GenreId = g.Id,
							 AuthorName = b.AuthorName,
							 GenreName = g.GenreName
						 }
						 ).ToListAsync();

			return books;
		}

    }
}

