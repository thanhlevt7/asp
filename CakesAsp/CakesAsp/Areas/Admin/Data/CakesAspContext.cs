﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CakesAsp.Areas.Admin.Models;

namespace CakesAsp.Areas.Admin.Data
{
    public class CakesAspContext : DbContext
    {
        public CakesAspContext (DbContextOptions<CakesAspContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

   

        public DbSet<Cart> Carts { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Product> Products { get; set; }



        
        public DbSet<Voucher> Vouchers { get; set; }

        
    }
}
