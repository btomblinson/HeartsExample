using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDeck.Enums
{
    public enum FaceValue
    {
        [Display(Name = "Two")] [Description("2")]
        Two,

        [Display(Name = "Three")] [Description("3")]
        Three,

        [Display(Name = "Four")] [Description("4")]
        Four,

        [Display(Name = "Five")] [Description("5")]
        Five,

        [Display(Name = "Six")] [Description("6")]
        Six,

        [Display(Name = "Seven")] [Description("7")]
        Seven,

        [Display(Name = "Eight")] [Description("8")]
        Eight,

        [Display(Name = "Nine")] [Description("9")]
        Nine,

        [Display(Name = "Ten")] [Description("1")]
        Ten,

        [Display(Name = "Jack")] [Description("J")]
        Jack,

        [Display(Name = "Queen")] [Description("Q")]
        Queen,

        [Display(Name = "King")] [Description("K")]
        King,

        [Display(Name = "Ace")] [Description("A")]
        Ace
    }
}