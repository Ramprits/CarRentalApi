using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarRentalApi.Entities
{
  public class CampUser : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

  }
}
