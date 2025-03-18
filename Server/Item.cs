using System;
using System.Collections.Generic;

namespace TodoApi;

/// <summary>
/// Id (int NOT NULL AUTO_INCREMENT)
/// Name (varchar (100))
/// IsComplete (tinyint(1))
/// 
/// </summary>
public partial class Item
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsComplete { get; set; }
}
