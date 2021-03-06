﻿using System;

namespace Files.Tutorial.Models
{
    public abstract class FileModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string FileType { get; set; }

        public string Extension { get; set; }

        public string Description { get; set; }

        public string UploadedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}