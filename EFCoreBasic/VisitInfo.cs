using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace EFCoreBasic
{
    // DTO (сущность)
    public class Student
    {
        public long Id { get; init; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateOnly Birthday { get; set; }

        public List<Visitation>? Visitations { get; set; }
    }
    public class Visitation
    {
        public long Id { get; init; }
        public DateOnly Date { get; set; }
        // Так как выполнена ручная настройка, то поле IdStudent в классе может отсутствовать
        public long StudentId { get; set; }
        public Student Student { get; set; }
    }
}