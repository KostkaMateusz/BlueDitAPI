﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bluedit.Migrations
{
    [DbContext(typeof(BlueditDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bluedit.Entities.Post", b =>
                {
                    b.Property<Guid>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ImageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PostId");

                    b.HasIndex("TopicName");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Bluedit.Entities.PostLike", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("Bluedit.Entities.ReplyBase", b =>
                {
                    b.Property<Guid>("ReplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPostReplay")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReplyId");

                    b.HasIndex("UserId");

                    b.ToTable("Replies");

                    b.HasDiscriminator<bool>("IsPostReplay");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Bluedit.Entities.ReplyLike", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("ReplyLikes");
                });

            modelBuilder.Entity("Bluedit.Entities.Topic", b =>
                {
                    b.Property<string>("TopicName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PostCount")
                        .HasColumnType("int");

                    b.Property<string>("TopicDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicName");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Bluedit.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("StandartUser");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Bluedit.Entities.Reply", b =>
                {
                    b.HasBaseType("Bluedit.Entities.ReplyBase");

                    b.Property<Guid>("ParentPostId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ParentPostId");

                    b.HasDiscriminator().HasValue(true);
                });

            modelBuilder.Entity("Bluedit.Entities.SubReplay", b =>
                {
                    b.HasBaseType("Bluedit.Entities.ReplyBase");

                    b.Property<Guid>("ParentReplyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue(false);
                });

            modelBuilder.Entity("Bluedit.Entities.Post", b =>
                {
                    b.HasOne("Bluedit.Entities.Topic", "Topic")
                        .WithMany("Posts")
                        .HasForeignKey("TopicName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bluedit.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bluedit.Entities.PostLike", b =>
                {
                    b.HasOne("Bluedit.Entities.Post", "Post")
                        .WithMany("PostLikes")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bluedit.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bluedit.Entities.ReplyBase", b =>
                {
                    b.HasOne("Bluedit.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bluedit.Entities.ReplyLike", b =>
                {
                    b.HasOne("Bluedit.Entities.ReplyBase", "Reply")
                        .WithMany("ReplyLikes")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bluedit.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reply");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bluedit.Entities.Reply", b =>
                {
                    b.HasOne("Bluedit.Entities.Post", "ParentPost")
                        .WithMany("Reply")
                        .HasForeignKey("ParentPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentPost");
                });

            modelBuilder.Entity("Bluedit.Entities.Post", b =>
                {
                    b.Navigation("PostLikes");

                    b.Navigation("Reply");
                });

            modelBuilder.Entity("Bluedit.Entities.ReplyBase", b =>
                {
                    b.Navigation("ReplyLikes");
                });

            modelBuilder.Entity("Bluedit.Entities.Topic", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Bluedit.Entities.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
