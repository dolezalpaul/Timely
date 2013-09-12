﻿using System;
using System.Collections.Generic;

namespace Moravia.Timely
{
    public class Entity
    {
        public int id { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public int version { get; set; }
    }
}