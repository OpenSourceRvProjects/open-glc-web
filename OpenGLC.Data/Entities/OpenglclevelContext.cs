﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OpenGLC.Data.Entities;

public partial class OpenglclevelContext : DbContext
{
	public OpenglclevelContext()
	{
	}

	public OpenglclevelContext(DbContextOptions<OpenglclevelContext> options)
		: base(options)
	{
	}

	public virtual DbSet<MealEvent> MealEvents { get; set; }

	public virtual DbSet<MealEventItem> MealEventItems { get; set; }

	public virtual DbSet<MealItem> MealItems { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{ 
		//Scaffold-DbContext "Server=.\SQLExpress;Database=OpenGLC-DB;Trusted_Connection=True;Encrypt=False" -Context OpenglclevelContext Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -F
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MealEvent>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_MealEvent");

			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.CreationDate).HasColumnType("datetime");
			entity.Property(e => e.MealDate).HasColumnType("datetime");

			entity.HasOne(d => d.User).WithMany(p => p.MealEvents)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__MealEvent__UserI__3F466844");
		});

		modelBuilder.Entity<MealEventItem>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_MealEventItem");

			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.MealItemId).HasColumnName("MealItemID");

			entity.HasOne(d => d.MealEvent).WithMany(p => p.MealEventItems)
				.HasForeignKey(d => d.MealEventId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__MealEvent__MealE__3D5E1FD2");

			entity.HasOne(d => d.MealItem).WithMany(p => p.MealEventItems)
				.HasForeignKey(d => d.MealItemId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__MealEvent__MealI__3E52440B");
		});

		modelBuilder.Entity<MealItem>(entity =>
		{
			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.UserId).HasColumnName("UserID");

			entity.HasOne(d => d.User).WithMany(p => p.MealItems)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__MealItems__UserI__403A8C7D");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.Property(e => e.Id).ValueGeneratedNever();
			entity.Property(e => e.UserNumber).ValueGeneratedOnAdd();
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
