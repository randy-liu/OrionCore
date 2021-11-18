using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Orion.Api.Tests
{
	public class OrionApiDbContext : DbContext
	{

		public static OrionApiDbContext CreateUseSqlServer()
		{
			var builder = new DbContextOptionsBuilder<OrionApiDbContext>();
			builder.UseSqlServer("Data Source=localhost;Initial Catalog=Orion_API_Tests;Integrated Security=True");
			return new OrionApiDbContext(builder.Options);
		}

		public static OrionApiDbContext CreateUseNpgsql() 
		{
			var builder = new DbContextOptionsBuilder<OrionApiDbContext>();
			builder.UseNpgsql("Host=localhost;Database=Orion_API_Tests;Username=postgres;Password=p@ssw0rd");
			return new OrionApiDbContext(builder.Options);
		}



		/*-----------------------------------------------------*/


		public OrionApiDbContext(DbContextOptions<OrionApiDbContext> options) : base(options) { }


		public DbSet<InvoiceIssue> InvoiceIssue { get; set; }
		public DbSet<InvoiceIssueItems> InvoiceIssueItems { get; set; }
		public DbSet<InventoryTemp> InventoryTemp { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		}
	}


	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrionApiDbContext>
	{
		public OrionApiDbContext CreateDbContext(string[] args)
		{
			return OrionApiDbContext.CreateUseNpgsql();
		}
	}



	/*===============================================================*/


	[Table(nameof(InvoiceIssue))]
	public class InvoiceIssue
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int InvoiceId { get; set; }

		[MaxLength(2), Required]
		public string InvoicePrefix { get; set; }

		public int? InvoiceNum { get; set; }

		public DateTime InvoiceDate { get; set; }

		[MaxLength(24), Required]
		public string DeliveryCustCode { get; set; }

		[MaxLength(128), Required]
		public string DeliveryCustName { get; set; }

		public decimal? Total { get; set; }

		public int CreateBy { get; set; }

		public DateTime CreateDate { get; set; }

		public int ModifyBy { get; set; }

		public DateTime? ModifyDate { get; set; }

		public List<InvoiceIssueItems> InvoiceIssueItems { get; set; } = new List<InvoiceIssueItems>();

	}

	[Table(nameof(InvoiceIssueItems))]
	public class InvoiceIssueItems
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ItemId { get; set; }

		public int InvoiceId { get; set; }

		[MaxLength(20), Required]
		public string DeliveryNum { get; set; }

		[MaxLength(15)]
		public string PurchaseNum { get; set; }

		public int Qty { get; set; }

		public decimal Price { get; set; }

		public decimal TotalPrice { get; set; }

		[ForeignKey(nameof(InvoiceId))]
		public InvoiceIssue InvoiceIssue { get; set; }

	}

	[Table(nameof(InventoryTemp))]
	public class InventoryTemp 
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int InventoryId { get; set; }

		[MaxLength(32), Required]
		public string MaterialCode { get; set; }

		[MaxLength(32), Required]
		public string BranchFactory { get; set; }

		[MaxLength(32), Required]
		public string ZoneCode { get; set; }

		[MaxLength(32), Required]
		public string BatchCode { get; set; }

		public decimal Quantity { get; set; }

		public DateTime ModifyDate { get; set; }

	}

}
