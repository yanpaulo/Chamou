namespace Chamou.Web.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ChamouContext : DbContext
    {
        // Your context has been configured to use a 'ChamouContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Chamou.Web.Models.Entities.ChamouContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ChamouContext' 
        // connection string in the application configuration file.
        public ChamouContext()
            : base("name=DefaultConnection")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Place> Places { get; set; }

        public virtual DbSet<GeoPoint> GeoPoints { get; set; }
    }
    
}