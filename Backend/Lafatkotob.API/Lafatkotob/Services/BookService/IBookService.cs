﻿using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.BookService
{
    public interface IBookService
    {
        Task<BookModel> GetById(int id);
        Task<List<BookModel>> GetAll();
        Task<ServiceResponse<BookModel>> Delete(int id);
        Task<ServiceResponse<BookModel>> Post(RegisterBook model, IFormFile imageFile);
        Task<ServiceResponse<UpdateBookModel>> Update(int id, UpdateBookModel model, IFormFile imageFile = null);
        Task<ServiceResponse<List<BookModel>>> GetBooksFilteredByGenres(List<int> genreIds);
        Task<ServiceResponse<List<GenreModel>>> GetGenresByBookId(int bookId);
        
        }
}
