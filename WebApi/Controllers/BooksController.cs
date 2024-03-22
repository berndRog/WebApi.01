using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WebApi.DomainModel;
using WebApi.DomainModel.Model;
using WebApi.Persistence;

namespace WebApi.Controllers; 
[Route("products/books")]
[ApiController]
public class BooksController : ControllerBase {

   private readonly IDataContext _dataContext;
   private readonly IBooksRepository _repository;

   public BooksController() {
      _dataContext  = new DataContextFake();
      // Dependency injetion
      _repository = new BooksRepositoryFake(_dataContext);
   }
   
   // Get all books
   // http://localhost:5100/products/book
   [HttpGet("books")]
   public ActionResult<IEnumerable<Book>> Get() {
      return Ok(_repository.Select());  
   }

   // Get book by Id
   // http://localhost:5100/products/books/{id}
   [HttpGet("{id}")]
   public ActionResult<Book?> GetBookById(Guid id) {
      switch (_repository.FindById(id)) {
         case Book book: return Ok(book);
         case null:      return NotFound($"Owner with given Id not found");
      }
   }

   // Create a new book
   // http://localhost:5100/products/books
   [HttpPost("")]
   public ActionResult<Book> CreateBook(
      [FromBody] Book book
   ) {
      // check if book.Id is set, else generate new Id
      if(book.Id == Guid.Empty) book.Id = Guid.NewGuid();
      // check if book with given Id already exists   
      if(_repository.FindById(book.Id) != null) 
         return BadRequest($"CreateBook: Book with the given id already exists");
      
      // add book to repository
      _repository.Add(book); 
      // save to datastore
      _dataContext.SaveAllChanges();
      
      // return created book      
      var uri = new Uri($"{Request.Path}/{book.Id}", UriKind.Relative);
      return Created(uri: uri, value: book);     
   }
   
   // Update a book
   // http://localhost:5100/products/books/{id}
   [HttpPut("{id:Guid}")] 
   public ActionResult<Book> UpdateBook(
      [FromRoute] Guid id,
      [FromBody]  Book updBook
   ) {
      if(id != updBook.Id) 
         return BadRequest($"UpdateBook: Id in the route and body do not match.");
      
      Book? book = _repository.FindById(id);
      if (book == null)
         return NotFound($"UpdateBook: Book with given id not found.");

      // Update person
      book = new Book(updBook);
      
      // save to repository and write to database 
      _repository.Update(book);
      _dataContext.SaveAllChanges();

      return Ok(book); //mapper.Map<PersonDto>(owner));
   }

   // Delete a book
   // http://localhost:5100/products/books/{id}
   [HttpDelete("{id:Guid}")]
   public ActionResult<Book> DeleteBook(
      [FromRoute] Guid id
   ) {

      // fetch owner from repository
      var book = _repository.FindById(id);
      if(book == null)
         return NotFound($"DeleteBook: Book with given id not found.");

      // delete owner from repository and write to dataContext
      _repository.Remove(book);
      _dataContext.SaveAllChanges();

      return NoContent();
   }
}