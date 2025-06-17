using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
    }
}
