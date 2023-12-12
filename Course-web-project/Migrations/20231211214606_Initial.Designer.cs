﻿// <auto-generated />
using Course_web_project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Course_web_project.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20231211214606_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Course_web_project.Models.Comments", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TexturesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("text_comment")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("TexturesId");

                    b.HasIndex("UsersId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Course_web_project.Models.Ratings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Rating")
                        .HasColumnType("REAL");

                    b.Property<int>("TexturesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("TexturesId")
                        .IsUnique();

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Course_web_project.Models.Textures", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("pbr_or_seamless")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("texture_name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("texture_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Textures");
                });

            modelBuilder.Entity("Course_web_project.Models.Users", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TexturesUsers", b =>
                {
                    b.Property<int>("TexturesID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersID")
                        .HasColumnType("INTEGER");

                    b.HasKey("TexturesID", "UsersID");

                    b.HasIndex("UsersID");

                    b.ToTable("TexturesUsers");
                });

            modelBuilder.Entity("Course_web_project.Models.Comments", b =>
                {
                    b.HasOne("Course_web_project.Models.Textures", "Textures")
                        .WithMany("Comments")
                        .HasForeignKey("TexturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_web_project.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Textures");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Course_web_project.Models.Ratings", b =>
                {
                    b.HasOne("Course_web_project.Models.Textures", "Textures")
                        .WithOne("Ratings")
                        .HasForeignKey("Course_web_project.Models.Ratings", "TexturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Textures");
                });

            modelBuilder.Entity("TexturesUsers", b =>
                {
                    b.HasOne("Course_web_project.Models.Textures", null)
                        .WithMany()
                        .HasForeignKey("TexturesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_web_project.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UsersID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Course_web_project.Models.Textures", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
