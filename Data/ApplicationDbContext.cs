
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();

    public DbSet<Theatre> Theatres => Set<Theatre>();

    public DbSet<Screen> Screens => Set<Screen>();

    public DbSet<Seat> Seats => Set<Seat>();

    public DbSet<Show> Shows => Set<Show>();

    public DbSet<ShowSeat> ShowSeats => Set<ShowSeat>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.MovieId);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(m => m.Description)
                .HasMaxLength(500);

            entity.Property(m => m.DurationMinutes)
                .IsRequired();

            entity.Property(m => m.ReleaseDate)
                .HasColumnType("date")
                .IsRequired();

            entity.Property(m => m.Language)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(50);

            entity.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] > 0"));

            entity.HasMany(m => m.Shows)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(t => t.TheatreId);

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasMany(t => t.Screens)
                .WithOne(s => s.Theatre)
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(s => s.ScreenId);

            entity.Property(s => s.ScreenName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                .IsUnique();

            entity.HasMany(s => s.Seats)
                .WithOne(s => s.Screen)
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Shows)
                .WithOne(s => s.Screen)
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(s => s.SeatId);

            entity.Property(s => s.RowName)
                .IsRequired()
                .HasMaxLength(5);

            entity.Property(s => s.SeatNumber)
                .IsRequired();

            entity.Property(s => s.SeatType)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            }).IsUnique();

            entity.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Seats_SeatNumber",
                    "[SeatNumber] > 0"));

            entity.HasMany(s => s.ShowSeats)
                .WithOne(ss => ss.Seat)
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.BookingSeats)
                .WithOne(bs => bs.Seat)
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(s => s.ShowId);

            entity.Property(s => s.StartTime)
                .IsRequired();

            entity.Property(s => s.EndTime)
                .IsRequired();

            entity.Property(s => s.TicketPrice)
                .HasPrecision(10, 2)
                .IsRequired();

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Shows_EndTime",
                    "[EndTime] > [StartTime]");

                table.HasCheckConstraint(
                    "CK_Shows_TicketPrice",
                    "[TicketPrice] >= 0");
            });

            entity.HasMany(s => s.ShowSeats)
                .WithOne(ss => ss.Show)
                .HasForeignKey(ss => ss.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Bookings)
                .WithOne(b => b.Show)
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasKey(ss => ss.ShowSeatId);

            entity.Property(ss => ss.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                .IsUnique();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingId);

            entity.Property(b => b.BookingReference)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(b => b.BookingReference)
                .IsUnique();

            entity.Property(b => b.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(b => b.CustomerEmail)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(b => b.CustomerPhone)
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(b => b.BookingDate)
                .IsRequired();

            entity.Property(b => b.TotalAmount)
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] >= 0"));

            entity.HasMany(b => b.BookingSeats)
                .WithOne(bs => bs.Booking)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(bs => bs.BookingSeatId);

            entity.Property(bs => bs.Price)
                .HasPrecision(10, 2)
                .IsRequired();

            entity.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_BookingSeats_Price",
                    "[Price] >= 0"));
        });
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
