namespace laptops.classes
{
    public class LaptopClass
    {
        public required string link { get; set; }
        public string? name { get; set; }
        public float UserRating { get; set; } // Fixed property name to follow C# naming conventions  
        public int price { get; set; }
        public string? SalesPackage { get; set; } // Fixed property name to remove space and resolve errors  

        // Fixed the property declaration by renaming "Model Number" to "ModelNumber" to remove the space  
        public string? ModelNumber { get; set; }

        // Fixed the property declaration by renaming "Part Number" to "PartNumber" to remove the space  
        public string? PartNumber { get; set; }

        // Fixed the property declaration by renaming "Model Name" to "ModelName" to remove the space  
        public string? ModelName { get; set; }

        public string? Series { get; set; }
        public string? Color { get; set; }
    }
}
