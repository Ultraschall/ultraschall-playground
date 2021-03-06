﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Ultraschall.Data.Entities;
using Ultraschall.Data.EntityFramework;

namespace Ultraschall.Data.EntityFramework.Migrations
{
    [DbContext(typeof(UltraschallContext))]
    [Migration("20180424134344_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Ultraschall.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<Guid?>("ParentId");

                    b.Property<Guid?>("PodcastId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("PodcastId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("Label");

                    b.Property<string>("Language");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Contribution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EpisodeId");

                    b.Property<Guid?>("PresenceId");

                    b.Property<Guid?>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("EpisodeId");

                    b.HasIndex("PresenceId");

                    b.HasIndex("RoleId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Contributor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Contributors");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.ContributorPresence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Contributor");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("ContributorPresences");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.ContributorRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ContributorRoles");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Episode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Duration");

                    b.Property<string>("Guid");

                    b.Property<string>("Link");

                    b.Property<string>("PublicationDate");

                    b.Property<int>("Sequence");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Comment");

                    b.Property<string>("Copyright");

                    b.Property<string>("Label");

                    b.Property<string>("Language");

                    b.Property<string>("License");

                    b.Property<string>("Mime");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<string>("Label");

                    b.Property<string>("Language");

                    b.Property<double>("Lattitude");

                    b.Property<double>("Longitude");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Podcast", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Language");

                    b.Property<string>("Link");

                    b.Property<Guid?>("OwnerId");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Podcasts");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Sequence");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Shownote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<string>("Icon");

                    b.Property<string>("Label");

                    b.Property<string>("Language");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.ToTable("Shownotes");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Category", b =>
                {
                    b.HasOne("Ultraschall.Data.Entities.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("Ultraschall.Data.Entities.Podcast")
                        .WithMany("Categories")
                        .HasForeignKey("PodcastId");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Contribution", b =>
                {
                    b.HasOne("Ultraschall.Data.Entities.Episode")
                        .WithMany("Contributions")
                        .HasForeignKey("EpisodeId");

                    b.HasOne("Ultraschall.Data.Entities.ContributorPresence", "Presence")
                        .WithMany()
                        .HasForeignKey("PresenceId");

                    b.HasOne("Ultraschall.Data.Entities.ContributorRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Ultraschall.Data.Entities.Podcast", b =>
                {
                    b.HasOne("Ultraschall.Data.Entities.Contributor", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });
#pragma warning restore 612, 618
        }
    }
}
