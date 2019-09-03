using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATAppApi.Models;

namespace ZATAppApi.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ViewSubAdminViewModel
    {
        string s = "";
        public ViewSubAdminViewModel(List<SubAdmin.Area> areas)
        {
            foreach (var item in areas)
            {
                if (areas.IndexOf(item) == areas.Count - 1)
                {
                    s += item.AreaName;
                    break;
                }
                s += item.AreaName + ", \n";
            }
        }
        public long Id { get; set; }
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Display(Name ="Phone Number")]
        public string Contact { get; set; }
        [Display(Name ="Areas")]
        [DataType(DataType.MultilineText)]
        public string Areas
        {
            get
            {
                return s;
            }
        }
    }
    public class AddAreaViewModel
    {
        public long SubAdminId { get; set; }
        [Display(Name ="Area Name")]
        [Required]
        public string AreaName { get; set; }
    }
    public class AreaViewModel
    {
        public int Id { get; set; }
        public long SubAdminId { get; set; }
        [Display(Name ="Area Name: ")]
        [Required]
        public string Name { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}