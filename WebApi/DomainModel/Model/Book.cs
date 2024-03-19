using System;
namespace WebApi.DomainModel.Model;

public record Book {
   public Guid Id { get; set; } = Guid.NewGuid();
   public string Author { get; set; } = string.Empty;
   public string Title { get; set; } = string.Empty;
   public int Year { get; set; } = 0;

   public Book(Book book) {
      this.Id = book.Id;
      this.Author = book.Author;
      this.Title = book.Title;
      this.Year = book.Year;
   }
   
}