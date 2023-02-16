using System;
using System.Collections.Generic;

namespace LaptopVendorRemake.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Laptop> Laptops { get; } = new List<Laptop>();
}
