﻿// <auto-generated />
using System;
using CrowdFunding.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrowdFunding.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181117212837_InvestmentIsApproved")]
    partial class InvestmentIsApproved
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CrowdFunding.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int>("CountryId");

                    b.Property<DateTime>("DateofBirth");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FName");

                    b.Property<string>("Image");

                    b.Property<string>("LName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NID");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("ParmanantAddress");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PresentAddress");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("CrowdFunding.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("CompanyTypeId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("EntrepreneurId");

                    b.Property<string>("Liesence");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNo");

                    b.Property<string>("RegNo");

                    b.Property<string>("Video");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.HasIndex("CompanyTypeId");

                    b.HasIndex("EntrepreneurId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CrowdFunding.Models.CompanyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("CompanyTypes");
                });

            modelBuilder.Entity("CrowdFunding.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CrowdFunding.Models.Funded", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<int>("InvestmentId");

                    b.Property<string>("InvestorId");

                    b.Property<bool>("IsLive");

                    b.Property<int>("ProjectId");

                    b.Property<double>("RaisedAmount");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentId");

                    b.HasIndex("InvestorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Fundeds");
                });

            modelBuilder.Entity("CrowdFunding.Models.Investment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<string>("InvestmentRegNo");

                    b.Property<int>("InvestmentTypeId");

                    b.Property<string>("InvestorId");

                    b.Property<bool>("IsApproved");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentTypeId");

                    b.HasIndex("InvestorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("CrowdFunding.Models.InvestmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProjectId");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("investmentTypes");
                });

            modelBuilder.Entity("CrowdFunding.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<string>("DetailDescription");

                    b.Property<DateTime>("EndingDate");

                    b.Property<string>("Image1");

                    b.Property<string>("Image2");

                    b.Property<string>("Image3");

                    b.Property<bool>("IsCompleted");

                    b.Property<bool>("IsRunning");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("NeededFund");

                    b.Property<int>("ProjectCategoryId");

                    b.Property<string>("ProjectShortDescription")
                        .IsRequired();

                    b.Property<string>("ProjectTitle")
                        .IsRequired();

                    b.Property<DateTime>("StartingDate");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ProjectCategoryId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CrowdFunding.Models.ProjectCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("ProjectCategory");
                });

            modelBuilder.Entity("CrowdFunding.Models.VerifiedCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("VerifiedCompanies");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CrowdFunding.Models.Entrepreneur", b =>
                {
                    b.HasBaseType("CrowdFunding.Data.ApplicationUser");

                    b.Property<string>("EntrepreneurCustomizedId");

                    b.ToTable("Entrepreneur");

                    b.HasDiscriminator().HasValue("Entrepreneur");
                });

            modelBuilder.Entity("CrowdFunding.Models.Investor", b =>
                {
                    b.HasBaseType("CrowdFunding.Data.ApplicationUser");

                    b.Property<string>("InvestorCustomizedId");

                    b.ToTable("Investor");

                    b.HasDiscriminator().HasValue("Investor");
                });

            modelBuilder.Entity("CrowdFunding.Data.ApplicationUser", b =>
                {
                    b.HasOne("CrowdFunding.Models.Country", "Country")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CrowdFunding.Models.Company", b =>
                {
                    b.HasOne("CrowdFunding.Models.CompanyType", "CompanyType")
                        .WithMany("Companies")
                        .HasForeignKey("CompanyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CrowdFunding.Models.Entrepreneur", "Entrepreneur")
                        .WithMany()
                        .HasForeignKey("EntrepreneurId");
                });

            modelBuilder.Entity("CrowdFunding.Models.Funded", b =>
                {
                    b.HasOne("CrowdFunding.Models.Investment", "Investment")
                        .WithMany()
                        .HasForeignKey("InvestmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CrowdFunding.Models.Investor", "Investor")
                        .WithMany()
                        .HasForeignKey("InvestorId");

                    b.HasOne("CrowdFunding.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CrowdFunding.Models.Investment", b =>
                {
                    b.HasOne("CrowdFunding.Models.InvestmentType", "InvestmentType")
                        .WithMany("Investments")
                        .HasForeignKey("InvestmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CrowdFunding.Models.Investor", "Investor")
                        .WithMany()
                        .HasForeignKey("InvestorId");

                    b.HasOne("CrowdFunding.Models.Project", "Project")
                        .WithMany("Investments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CrowdFunding.Models.InvestmentType", b =>
                {
                    b.HasOne("CrowdFunding.Models.Project", "Project")
                        .WithMany("InvestmentTypes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CrowdFunding.Models.Project", b =>
                {
                    b.HasOne("CrowdFunding.Models.Company", "Company")
                        .WithMany("Projects")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CrowdFunding.Models.ProjectCategory", "ProjectCategory")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CrowdFunding.Models.VerifiedCompany", b =>
                {
                    b.HasOne("CrowdFunding.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CrowdFunding.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CrowdFunding.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CrowdFunding.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CrowdFunding.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
