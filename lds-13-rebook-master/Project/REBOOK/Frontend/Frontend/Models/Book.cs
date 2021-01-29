using System;

namespace Frontend.Models
{
    
    public enum Stars
    {
        Undefined,
        One,
        Two,
        Three,
        Four,
        Five
    }
    public class Book
    {
        public Book()
        {
            PhotoPath = "http://10.0.2.2:5000/pic/Book/picture" + Id +".png";
            Console.WriteLine(Id);
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Stars State { get; set; }
        public string Photo { get; set; }
        public int OwnerId { get; set; }

        public string PhotoPath { get; set; }


        public void changeState(int index)
        {
            switch (index)
            {
                case(0):
                    State = Stars.One;
                    break;
                    
                case(1):
                    State = Stars.Two;
                    break;
                    
                case(2):
                    State = Stars.Three;
                    break;
                    
                case(3):
                    State = Stars.Four;
                    break;
                    
                case(4):
                    State = Stars.Five;
                    break;
            }
        }
        
    }
}