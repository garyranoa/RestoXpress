using System;
using System.Collections.Generic;
namespace UHack.Core.APIModels
{
    
    public class MemberApiResult
    {
        public MemberApiModel MemberInfo { get; set; }

    }

    public class MemberApiModel
    {
        public int Id { get; set; }
        public string UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Barcode { get; set; }
        public int ClubId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool ReceiveTextMessages { get; set; }
        public bool ReceiveEmails { get; set; }
    }

    public class SNSPlatformInfoResult
    {
        public string arn { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
    }

    public partial class RegisterStepOneResultModel
    {
        public bool IsAlreadyRegistered { get; set; }
        public bool IsPromoCode { get; set; }
        public bool IsExistOnAPI { get; set; }
        public string Barcode { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string ClubId { get; set; }
    }

    public partial class APIRegisterStepOneModel
    {
        public string Barcode { get; set; }
        public bool IsPromoCode { get; set; }
    }

    public partial class APIRegisterStepTwoResultModel
    {
        public bool IsEmailExist { get; set; }
        public bool IsUsernameExist { get; set; }
        public bool IsSuccessfullyRegistered { get; set; }
    }

    public partial class APIMemberUpdateSettingsResultModel
    {
        public int UserId { get; set; }
        public int ClubId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsEmailExist { get; set; }
        public bool IsSuccessfullyUpdated { get; set; }
    }
}
