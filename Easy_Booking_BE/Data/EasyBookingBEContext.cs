using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Data
{
    public class EasyBookingBEContext : DbContext
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

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking_Room>()
                .HasKey(br => new {br.booking_id ,br.room_id });

            modelBuilder.Entity<Room_Feature>()
                .HasKey(rf => new {rf.room_id ,rf.feature_id });

            base.OnModelCreating(modelBuilder);
        }
        
        
    }
}
