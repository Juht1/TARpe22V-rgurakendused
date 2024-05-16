using System;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Model;

public class Event
{
    public int id {get; set;}
    public int Speakerid {get; set;}
    public string? name {get; set;}
    public DateTime Date {get; set;}
    public string? Location {get; set;}
}
