using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LaptopVendorRemake.Models;

public partial class Brand
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [InverseProperty("Brand")]
    public virtual ICollection<Laptop> Laptops { get; } = new List<Laptop>();
}
