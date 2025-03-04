using EasyBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Data
{
    public class EasyBookingBEContext : IdentityDbContext<ApplicationUser>
    {

        public EasyBookingBEContext(DbContextOptions<EasyBookingBEContext> options) : base(options) { }

        #region DBSet

        public DbSet<Booking>? Booking { get; set; }
        public DbSet<Booking_Room>? Booking_Roos { get; set; }
        public DbSet<Booking_Status>? Booking_Status { get; set; }
        public DbSet<Feature>? Features { get; set; }
        public DbSet<Floor>? Floors { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<Payment_Status>? Payment_Status { get; set; }
        public DbSet<Room>? Room { get; set; }
        public DbSet<Room_Feature>? Room_Features { get; set; }
        public DbSet<Room_Status>? Room_Status { get; set; }
        public DbSet<Media>? Medias { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Add default data for Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1", // ID fixed
                    Name = Constants.Constants.ADMIN,
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "2", // ID fixed
                    Name = Constants.Constants.CUSTOMER,
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Id = "3", // ID fixed
                    Name = Constants.Constants.HOST,
                    NormalizedName = "HOST"
                },
                new IdentityRole
                {
                    Id = "4", // ID fixed
                    Name = Constants.Constants.SUPPORT_STAFF,
                    NormalizedName = "SUPPORT STAFF"
                }
            };

            // Add Roles to the IdentityRole table
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            
            modelBuilder.Entity<Booking_Room>()
                .HasKey(br => new {br.booking_id ,br.room_id });

            modelBuilder.Entity<Room_Feature>()
                .HasKey(rf => new {rf.room_id ,rf.feature_id });
        }
    }
}
