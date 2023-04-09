using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDeck.Enums
{
    public enum Suit
    {
        [Display(Name = "Clubs")] [Description("C")]
        Clubs,

        [Display(Name = "Spades")] [Description("S")]
        Spades,

        [Display(Name = "Hearts")] [Description("H")]
        Hearts,

        [Display(Name = "Diamonds")] [Description("D")]
        Diamonds
    }
}