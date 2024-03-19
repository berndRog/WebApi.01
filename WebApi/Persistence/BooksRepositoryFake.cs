using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WebApi.DomainModel;
using WebApi.DomainModel.Model;

namespace WebApi.Persistence;

internal class BooksRepositoryFake(
   IDataContext dataContext
): IBooksRepository {
   
   public IEnumerable<Book> Select() {
      return dataContext.Books.Values.ToList();
   }

   public Book? FindById(Guid id) { 
      dataContext.Books.TryGetValue(id, out Book? book);
      return book;
   }

   public void Add(Book book) {
      dataContext.Books.Add(book.Id, book);
   }
   
   public void Update(Book book) {
      dataContext.Books.Remove(book.Id);
      dataContext.Books.Add(book.Id, book);
   }

   public void Remove(Book book) {
      dataContext.Books.Remove(book.Id);
   }
}