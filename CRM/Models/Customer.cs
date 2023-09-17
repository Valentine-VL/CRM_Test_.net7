using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models;

public class Customer
{
    public int Id { get; set; }
    [Display(Name = "Name")]
    public string? Name { get; set; }
    
    [Display(Name = "Surname")]
    public string? Surname { get; set; }
    
    [Display(Name = "Date Registered")]
    [DataType(DataType.Date)]
    public DateTime DateRegistered { get; set; }
    public string? Adress { get; set; }
    public string? PostCode { get; set; }
    public string? PhoneNumber { get; set; }
    
    public virtual ICollection<Call> Calls { get; set; }
}

public class Call
{
    public int Id { get; set; }
    public int CustomerID { get; set; }
    public virtual Customer Customer { get; set; }
    
    public string? Subject { get; set; }
    public string? Description { get; set; }
    
    [Display(Name = "Date of Call")]
    [DataType(DataType.Date)]
    public DateTime DateOfCall { get; set; }
    
    [Display(Name = "Time of Call")]
    [DataType(DataType.Time)]
    public DateTime TimeOfCall { get; set; }
    
}