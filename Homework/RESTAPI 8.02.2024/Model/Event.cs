using System;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Model;

public class Event
{
    public int iD {get; set;}
    public int SpeakeriD {get; set;}
    public string? name {get; set;}
    public DateTime Date {get; set;}
    public string? Location {get; set;}
}
