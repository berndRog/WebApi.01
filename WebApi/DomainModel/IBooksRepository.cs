using System;
using System.Collections.Generic;
using WebApi.DomainModel.Model;
namespace WebApi.DomainModel;

public interface IBooksRepository {
   IEnumerable<Book> Select();
   Book? FindById(Guid id);
   void Add(Book book);
   void Update(Book book);
   void Remove(Book book);
}