using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Box.Security.Models.ManageViewModels
{
    public class GenerateRecoveryCodesViewModel
    {
        public string[] RecoveryCodes { get; set; }
    }
}
