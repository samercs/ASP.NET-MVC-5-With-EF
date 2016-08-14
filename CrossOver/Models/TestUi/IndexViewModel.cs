using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CrossOver.Core.Attribute;

namespace CrossOver.Models.TestUi
{
    public class IndexViewModel
    {
        [DisplayName("Language")]
        [AutoCompleteUiHint("GetAutoComplete", "TestUi")]
        public int Name { get; set; }
        
    }
}