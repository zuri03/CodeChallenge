using System;

namespace CodeChallenge.Models;

public class Compensation 
{
    public String Employee { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? EffectiveDate { get; set; }
}