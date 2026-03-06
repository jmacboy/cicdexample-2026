using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.IntTest.DTOs
{
    public class ItemCreateResponseDto
    {
        public Guid Value { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string StructuredMessage { get; set; }
        public long Type { get; set; }
    }
}