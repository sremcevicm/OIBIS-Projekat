using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class MyDbContext : DbContext
    {
        public DbSet<Popust> popusts { get; set; }
        public DbSet<Rezervacija> rezervacijas { get; set; }
        public DbSet<Projekcija> projekcijas { get; set; }

        public DbSet<Korisnik> korisniks { get; set; }

        public MyDbContext() : base("name=MyDbContext")
        {
        }

    }
}
