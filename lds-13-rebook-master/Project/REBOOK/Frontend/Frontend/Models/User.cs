using System;
using System.ComponentModel;
using MvvmHelpers;


namespace Frontend.Models
{
    public class User : ObservableObject
    {


        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string token { get; set; }
    }
}