﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Releaser.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RDI_ProductionEntities : DbContext
    {
        public RDI_ProductionEntities()
            : base("name=RDI_ProductionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DOC_METADATA> DOC_METADATA { get; set; }
        public virtual DbSet<DOC> DOCS { get; set; }
        public virtual DbSet<ItemFile> ItemFiles { get; set; }
        public virtual DbSet<ItemPriority> ItemPriorities { get; set; }
        public virtual DbSet<ItemStatu> ItemStatus { get; set; }
        public virtual DbSet<ItemStatusModule> ItemStatusModules { get; set; }
        public virtual DbSet<ItemStatusNextValid> ItemStatusNextValids { get; set; }
        public virtual DbSet<ItemSubscriber> ItemSubscribers { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<ItemTypeModule> ItemTypeModules { get; set; }
        public virtual DbSet<RDIItem> RDIItems { get; set; }
        public virtual DbSet<RDIItemGroup> RDIItemGroups { get; set; }
        public virtual DbSet<RDIItemGroupMember> RDIItemGroupMembers { get; set; }
        public virtual DbSet<RDIItemHistory> RDIItemHistories { get; set; }
        public virtual DbSet<RDIItemPageTitle> RDIItemPageTitles { get; set; }
        public virtual DbSet<RDIItemTag> RDIItemTags { get; set; }
        public virtual DbSet<AllUser> AllUsers { get; set; }
    }
}
