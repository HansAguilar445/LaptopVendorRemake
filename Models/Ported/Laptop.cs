using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LaptopVendorRemake.Models;

[Index("BrandId", Name = "IX_Laptops_BrandId")]
public partial class Laptop
{
    [Key]
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    [Column(TypeName = "decimal(7, 2)")]
    public decimal Price { get; set; }

    public int Year { get; set; }

    public int BrandId { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Laptops")]
    public virtual Brand Brand { get; set; } = null!;
}
