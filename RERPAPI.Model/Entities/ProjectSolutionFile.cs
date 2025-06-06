﻿using System;
using System.Collections.Generic;

namespace RERPAPI.Model.Entities;

public partial class ProjectSolutionFile
{
    public int ID { get; set; }

    public int? ProjectSolutionID { get; set; }

    public string? FileNameOrigin { get; set; }

    public string? Extension { get; set; }

    public string? OriginPath { get; set; }

    public string? ServerPath { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
