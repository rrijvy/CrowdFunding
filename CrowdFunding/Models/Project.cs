﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdFunding.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required, Display(Name = "Project Short Discription")]
        public string ProjectShortDescription { get; set; }

        [Display(Name = "Projects Details Description")]
        public string DetailDescription { get; set; }
        
        [Required, Display(Name = "Project title")]
        public string ProjectTitle { get; set; }

        [Display(Name = "Is this project running?")]
        public bool IsRunning { get; set; }

        [Display(Name = "Is this project completed?")]
        public bool IsCompleted { get; set; }
        
        [Display(Name = "Needed fund")]
        public double NeededFund { get; set; }

        [Required, DataType(DataType.Date), Display(Name ="Starting date")]
        public DateTime StartingDate { get; set; }

        [Required, DataType(DataType.Date), Display(Name ="Ending date")]
        public DateTime EndingDate { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        [Display(Name = "Company name")]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int ProjectCategoryId { get; set; }

        [ForeignKey("ProjectCategoryId")]
        public ProjectCategory ProjectCategory { get; set; }

        public List<Investment> Investments { get; set; }

        public List<InvestmentType> InvestmentTypes { get; set; }

    }
}
