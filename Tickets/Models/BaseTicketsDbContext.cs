﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tickets.Models
{
    public partial class BaseTicketsDbContext : DbContext
    {
        public BaseTicketsDbContext()
        {
        }

        public BaseTicketsDbContext(DbContextOptions<BaseTicketsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventSeat> EventSeat { get; set; }
        public virtual DbSet<Row> Row { get; set; }
        public virtual DbSet<Seat> Seat { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<TicketPurchase> TicketPurchase { get; set; }
        public virtual DbSet<TicketPurchaseSeat> TicketPurchaseSeat { get; set; }
        public virtual DbSet<Venue> Venue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=BaseTicketsDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasColumnName("event_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VenueName)
                    .HasColumnName("venue_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.VenueNameNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.VenueName)
                    .HasConstraintName("FK__Event__venue_nam__1B0907CE");
            });

            modelBuilder.Entity<EventSeat>(entity =>
            {
                entity.Property(e => e.EventSeatId).HasColumnName("event_seat_id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.EventSeatPrice)
                    .HasColumnName("event_seat_price")
                    .HasColumnType("money");

                entity.Property(e => e.SeatId).HasColumnName("seat_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventSeat)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventSeat__event__1ED998B2");

                entity.HasOne(d => d.Seat)
                    .WithMany(p => p.EventSeat)
                    .HasForeignKey(d => d.SeatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventSeat__seat___1DE57479");
            });

            modelBuilder.Entity<Row>(entity =>
            {
                entity.Property(e => e.RowId).HasColumnName("row_id");

                entity.Property(e => e.RowName)
                    .IsRequired()
                    .HasColumnName("row_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Row)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK__Row__section_id__15502E78");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.SeatId).HasColumnName("seat_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.RowId).HasColumnName("row_id");

                entity.HasOne(d => d.Row)
                    .WithMany(p => p.Seat)
                    .HasForeignKey(d => d.RowId)
                    .HasConstraintName("FK__Seat__row_id__182C9B23");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.Property(e => e.SectionName)
                    .IsRequired()
                    .HasColumnName("section_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VenueName)
                    .HasColumnName("venue_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.VenueNameNavigation)
                    .WithMany(p => p.Section)
                    .HasForeignKey(d => d.VenueName)
                    .HasConstraintName("FK__Section__venue_n__1273C1CD");
            });

            modelBuilder.Entity<TicketPurchase>(entity =>
            {
                entity.HasKey(e => e.PurchaseId)
                    .HasName("PK__TicketPu__87071CB93369B17C");

                entity.Property(e => e.PurchaseId)
                    .HasColumnName("purchase_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConfirmationCode)
                    .HasColumnName("confirmation_code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentAmount)
                    .HasColumnName("payment_amount")
                    .HasColumnType("money");

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasColumnName("payment_method")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TicketPurchaseSeat>(entity =>
            {
                entity.HasKey(e => new { e.EventSeatId, e.PurchaseId })
                    .HasName("PK__TicketPu__B5CCA47E48E4C54A");

                entity.Property(e => e.EventSeatId).HasColumnName("event_seat_id");

                entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");

                entity.Property(e => e.SeatSubtotal)
                    .HasColumnName("seat_subtotal")
                    .HasColumnType("money");

                entity.HasOne(d => d.EventSeat)
                    .WithMany(p => p.TicketPurchaseSeat)
                    .HasForeignKey(d => d.EventSeatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketPur__event__239E4DCF");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.TicketPurchaseSeat)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketPur__purch__22AA2996");
            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(e => e.VenueName)
                    .HasName("PK__Venue__3D6847F21FE08889");

                entity.Property(e => e.VenueName)
                    .HasColumnName("venue_name")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Capacity).HasColumnName("capacity");
            });
        }
    }
}
