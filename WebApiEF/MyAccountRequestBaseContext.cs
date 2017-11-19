using System.Data.Entity;

namespace WebApiEF
{
    public class MyAccountRequestBaseContext : DbContext
    {
        virtual public DbSet<MyAccountRequestBase> MyAccountRequestBases { get; set; }
    }
}