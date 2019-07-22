using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BnetApplication.Models
{
    
    public class Signin
    {
        [Display(Name ="User Name")]
        [Required]
        public string  UserName { get; set; }

        [Display(Name = "Pass Word")]
        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name = "User Type")]
        [Required]
        public string UserType { get; set; }
    }

    public class ForgotPasswoard
    {
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "User Type")]
        [Required]
        public string UserType { get; set; }
    }

    public class ChangePassWoard
    {
        [Display(Name = "Current PassWoard")]
        [Required]
        public string CurrentPass { get; set; }

        [Display(Name = "New PassWoard")]
        [Required]
        [DataType(DataType.Password)]
        public string NewPass { get; set; }

        [Display(Name = "Confirm PassWoard")]
        [Required]
        [Compare("NewPass")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }

    }



    public class Group
    {
        [Display(Name = "ID")]
        public string id { get; set; }

        [Display(Name = "Group Name")]
        [Required]
        public string GroupName { get; set; }

        [Display(Name = "Group Privilege")]
        [Required]
        public string groupPrivilege { get; set; }

        [Display(Name = "Group Account")]
        [Required]
        public string groupAccount { get; set; }

        [Display(Name = "Group Status")]
        [Required]
        public string groupStatus { get; set; }

        [Display(Name = "group Skill")]
        [Required]
        public string groupSkill { get; set; }
    }

    public class Account
    {

        [Display(Name = "AccountID")]
        public string AccountID { get; set; }

        [Display(Name = "Account Name")]
        [Required]
        public string AccountName { get; set; }

        [Display(Name = "Account Status")]
        [Required]
        public string Status { get; set; }
    }

    public class Skill
    {
        [Display(Name = "Skill ID")]
        public string SkillID { get; set; }

        [Display(Name = "Skill Name")]
        [Required]
        public string SkillName { get; set; }

        [Display(Name = "Skill Status")]
        public string Status { get; set; }

        [Display(Name = "Account Name")]
        [Required]
        public string AccountName { get; set; }
    }

    public class ProblemType
    {
        [Display(Name = "Problem ID")]
        public string ProblemTypeID { get; set; }

        [Display(Name = "Problem Name")]
        public string ProblemName { get; set; }

        [Display(Name = "Problem Status")]
        public string ProblemStatus { get; set; }
    }

    public class RouterType
    {
        [Display(Name = "Router ID")]
        public string ID { get; set; }
        [Display(Name = "Router Name")]
        public string RouterName { get; set; }
    }

    public class QuestionType
    {
        [Display(Name = "Question ID")]
        public string ID { get; set; }
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }
    }

    public class User
    {
        [Display(Name = "User ID")]
        public string UserID  { get; set; }

        [Display(Name = "User Name")]
        [Required]
        public string userName { get; set; }

        [Display(Name = "User Pass")]
        [DataType(DataType.Password)]
        [Required]
        public string userPass { get; set; }

        [Display(Name = "Confirm Pass")]
        [Compare("userPass")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }
        [Display(Name = "User Privilege")]
        [Required]
        public string userPrivilege { get; set; }

        [Display(Name = "user Authenticate Type")]
        [Required]
        public string userAuthType { get; set; }

       
        [Display(Name = "User Skill")]
        [Required]
        public string userSkill { get; set; }

        [Display(Name = "User Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string userEmail { get; set; }

        [Display(Name = "User Mobile")]
        [Required]
        public string userMobile { get; set; }

        [Display(Name = "User IP")]
        [Required]
        public string userIP { get; set; }

        [Display(Name = "User Status")]
        [Required]
        public string userStatus { get; set; }

        [Display(Name = "Idate")]
        public DateTime Idate { get; set; }
    }

}