﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APIGenericRepo.Models;

[Table("Instructor")]
public partial class Instructor
{
    [Key]
    public int Ins_Id { get; set; }

    [StringLength(50)]
    public string Ins_Name { get; set; }

    [StringLength(50)]
    public string Ins_Degree { get; set; }

    [Column(TypeName = "money")]
    public decimal? Salary { get; set; }

    public int? Dept_Id { get; set; }

    [InverseProperty("Dept_ManagerNavigation")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [ForeignKey("Dept_Id")]
    [InverseProperty("Instructors")]
    public virtual Department Dept { get; set; }

    [InverseProperty("Ins")]
    public virtual ICollection<Ins_Course> Ins_Courses { get; set; } = new List<Ins_Course>();
}