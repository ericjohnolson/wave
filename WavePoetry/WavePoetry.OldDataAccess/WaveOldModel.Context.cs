﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WavePoetry.OldDataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class waveoldEntities : DbContext
    {
        public waveoldEntities()
            : base("name=waveoldEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<admabout> admabouts { get; set; }
        public DbSet<admaudio> admaudios { get; set; }
        public DbSet<admauthor> admauthors { get; set; }
        public DbSet<admcatalog> admcatalogs { get; set; }
        public DbSet<admcontact> admcontacts { get; set; }
        public DbSet<admevent> admevents { get; set; }
        public DbSet<admhome> admhomes { get; set; }
        public DbSet<admlink> admlinks { get; set; }
        public DbSet<atrole> atroles { get; set; }
        public DbSet<authorresource> authorresources { get; set; }
        public DbSet<author> authors { get; set; }
        public DbSet<bedazzler> bedazzlers { get; set; }
        public DbSet<bedazzlet> bedazzlets { get; set; }
        public DbSet<blurb> blurbs { get; set; }
        public DbSet<bounding> boundings { get; set; }
        public DbSet<contactarc> contactarcs { get; set; }
        public DbSet<contactcat> contactcats { get; set; }
        public DbSet<contactfinal> contactfinals { get; set; }
        public DbSet<contactgalley> contactgalleys { get; set; }
        public DbSet<contactnote> contactnotes { get; set; }
        public DbSet<contact> contacts { get; set; }
        public DbSet<contestbook> contestbooks { get; set; }
        public DbSet<lineitem> lineitems { get; set; }
        public DbSet<phone> phones { get; set; }
        public DbSet<review> reviews { get; set; }
        public DbSet<special> specials { get; set; }
        public DbSet<taxship> taxships { get; set; }
        public DbSet<titleresource> titleresources { get; set; }
        public DbSet<title> titles { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<waveorder> waveorders { get; set; }
    }
}
