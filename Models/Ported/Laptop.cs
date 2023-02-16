using System;
using System.Collections.Generic;

namespace LaptopVendorRemake.Models;

public partial class Laptop
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public double Price { get; set; }

    public int Year { get; set; }

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;
}
