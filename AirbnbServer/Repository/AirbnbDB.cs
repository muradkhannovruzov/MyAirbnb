using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbServer.Repository
{
    public class AirbnbDB : DbContext
    {
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Amenity> Amenities { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Messaging> Messaging { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        public AirbnbDB():base("AirbnbDB")
        {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Home>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Messaging>().
                 HasKey(x => x.Id).
                 HasIndex(x => x.Id);


            modelBuilder.Entity<Messages>().
                 HasKey(x => x.Id).
                 HasIndex(x => x.Id);


            modelBuilder.Entity<City>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Comment>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Account>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Country>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Amenity>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Image>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Publication>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);

            modelBuilder.Entity<Reservation>().
                HasKey(x => x.Id).
                HasIndex(x => x.Id);
        }
    }
}
