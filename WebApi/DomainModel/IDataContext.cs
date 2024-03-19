using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Matching;
using WebApi.DomainModel.Model;

namespace WebApi.DomainModel;

public interface IDataContext {
   Dictionary<Guid, Book> Books { get; }
   bool SaveAllChanges();
}