using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class BranchModel
    {
    public int Id { get; set; }
    public string BranchName { get; set; }
    public string BranchAbbreviation { get; set; }
    public string BranchDescription { get; set; }
    public int RouteId { get; set; }
    }
  }
