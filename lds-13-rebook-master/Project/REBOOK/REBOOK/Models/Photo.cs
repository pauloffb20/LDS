using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBOOK.Models
{
    public class Photo
    {
        public Photo(String filePath)
        {
            this.FilePath = filePath;
        }
        
        public Photo(){}

        public int Id { get; set; }
        public String FilePath { get; set; }


        
    }
}