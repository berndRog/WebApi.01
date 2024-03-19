using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using WebApi.DomainModel;
using WebApi.DomainModel.Model;
namespace WebApi.Persistence;

internal class DataContextFake : IDataContext {
   private readonly string _filePath =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
      + $"/WebApi01";

   public Dictionary<Guid, Book> Books { get; } 

   public DataContextFake() {
      if (!File.Exists(_filePath)) {
         Books = new Dictionary<Guid, Book>();
      }
      else {
         try {
            // Read JSON from file
            string json = File.ReadAllText(_filePath, Encoding.UTF8) ??
               throw new ArgumentNullException("File.ReadAllText(filePath, Encoding.UTF8)");
            // Deserialize JSON to Books
            Books = JsonSerializer.Deserialize<Dictionary<Guid, Book>>(json)
               ?? throw new Exception("JsonSerializer.Deserialize is null)");
         }
         catch (Exception e) {
            Console.WriteLine(e.Message);
         }
      }

   }

   public bool SaveAllChanges() {
      try {
         // Serialize Books to JSON
         string json = JsonSerializer.Serialize(
            Books,
            new JsonSerializerOptions { WriteIndented = true }
         );
         // Write JSON string to file
         File.WriteAllText(_filePath, json, Encoding.UTF8);
         return true;
      }
      catch (Exception e) {
         Console.WriteLine(e.Message);
         return false;
      }
   }

}