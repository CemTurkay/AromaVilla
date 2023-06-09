﻿namespace ApplicationCore.Entities
{
    public class Adress
    {
        public Adress()
        {

        }
        public Adress(string street, string city, string? state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }
}