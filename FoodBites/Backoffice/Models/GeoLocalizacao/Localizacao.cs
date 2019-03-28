﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Petiscos;

namespace Backoffice.Models.GeoLocalizacao
{
    public class Localizacao
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


    }
}
