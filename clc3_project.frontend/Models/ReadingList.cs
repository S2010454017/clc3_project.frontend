﻿
namespace CLC3_Project.Model
{
    public class ReadingList
    {
        public string? Id { get; set; }
        public string Owner { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
