﻿using System;
using System.Collections.Generic;
using System.Linq;
using BibliographicalSourcesIntegratorWarehouse.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web.Http;

namespace BibliographicalSourcesIntegratorWarehouse.Controllers
{
    public class Publication_PersonController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;

        public Publication_PersonController(ILogger<Publication_PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Publication_Person> Get()
        {
            return null;
        }
    }
}
