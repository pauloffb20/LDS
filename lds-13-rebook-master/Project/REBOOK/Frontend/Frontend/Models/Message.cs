using System;
using MvvmHelpers;

namespace Frontend.Models
{

    public class Message : ObservableObject
    {

        public string Text { get; set; }
        
        public int SenderId { get; set; }
        
        public string userName { get; set; }
    }
}