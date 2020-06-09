using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{

    public class Rootobject
    {
        public ClienteAsegurado clienteAsegurado { get; set; }
    }

    public class ClienteAsegurado
    {
        public Customerinformation customerInformation { get; set; }
        public Statusresponse statusResponse { get; set; }
    }

    public class Customerinformation
    {
        public Customerdata customerData { get; set; }
        public Policy[] policy { get; set; }
    }

    public class Customerdata
    {
        public string customerRole { get; set; }
        public Customerpersonalinformation customerPersonalInformation { get; set; }
        public Customercontactdata customerContactData { get; set; }
    }

    public class Customerpersonalinformation
    {
        public string customerDocumentType { get; set; }
        public string customerDocumentNumber { get; set; }
        public string customerFullName { get; set; }
        public string customerFirstName { get; set; }
        public string customerMiddleName { get; set; }
        public string customerLastName { get; set; }
        public string customerGender { get; set; }
        public string customerBirthDate { get; set; }
        public string customerBirthPlaceName { get; set; }
        public string customerDeathDate { get; set; }
        public string customerNationalityOrigin { get; set; }

        public string customerChildren { get; set; }
        public string customerPets { get; set; }
        public string customerShareInformation { get; set; }
        public string customerContactCellphone { get; set; }
        public string customerContactMail { get; set; }
        public string customerIncome { get; set; }
        public string customerEconomicActivity { get; set; }
        public string customerMaritalStatus { get; set; }
    }

    public class Customercontactdata
    {
        public Address address { get; set; }
        public Email email { get; set; }
        public Phone phone { get; set; }
    }

    public class Address
    {
        public string customerAddress { get; set; }
        public string customerAddressCityId { get; set; }
        public string customerAddressCity { get; set; }
        public string customerAddressDeparmentId { get; set; }
        public string customerAddressDeparment { get; set; }
        public string customerAddressCountry { get; set; }
        public string customerAddressCountryId { get; set; }
    }

    public class Email
    {
        public string customerEmail { get; set; }
    }

    public class Phone
    {
        public string customerMobile1 { get; set; }
        public string customerMobile2 { get; set; }
        public string customerPhoneCode1 { get; set; }
        public string customerPhoneNumber1 { get; set; }
        public string customerExtension1 { get; set; }
        public string customerPhoneCode2 { get; set; }
        public string customerPhoneNumber2 { get; set; }
        public string customerExtension2 { get; set; }
    }

    public class Policy
    {
        public string cardifPolicyNumber { get; set; }
        public object cardifPolicyCountryISO { get; set; }
        public object cardifPolicyCountryName { get; set; }
        public string cardifpolicyStartDate { get; set; }
        public string cardifPolicyEndDate { get; set; }
        public string cardifPolicySubscriptionDate { get; set; }
        public string cardifPolicyEffectiveCancellationDate { get; set; }
        public string cardifPolicyApplyCancellationDate { get; set; }
        public string cardifPolicyRequestCancellationDate { get; set; }
        public string cardifPolicyCancellationReason { get; set; }
        public int cardifPolicyDurationDays { get; set; }
        public string policyStatusCode { get; set; }
        public string vigilanceDesc { get; set; }
        public string migrationProductDate { get; set; }
        public string source { get; set; }
        public int idUpload { get; set; }
        public string vigilanceList { get; set; }
        public string fileUploadName { get; set; }
        public string policyBusinessLineCodeCardif { get; set; }
        public int loanAmount { get; set; }
        public object loanAmountCurrencyISO { get; set; }

        public string cardifPolicyNetPremiumAmnt { get; set; }
        public string cardifPolicyGrossPremiumAmnt { get; set; }
        public string  cardifPolicyPremiumCurrencyISO { get; set; }
        public Partnerpolicyinfo partnerPolicyInfo { get; set; }
        public Policydetails policyDetails { get; set; }

        public MortgageInfo mortgageInfo { get; set; }
        public Billing billing { get; set; }
        public Assesor assesor { get; set; }
        public Product[] product { get; set; }
        public PartnerInfo[] partnerInfo { get; set; }
    }

    public class Partnerpolicyinfo
    {
        public string partnerPolicyNumber { get; set; }
        public string partnerPolicyProdCode { get; set; }
        public string partnerTransactionNumber { get; set; }
        public string partnerApprovalNumber { get; set; }
        public string partnerContractNumber { get; set; }
        public string partnerRate { get; set; }
        public string partnerInsuranceType { get; set; }
        public string partnerPolicyMovement { get; set; }
        public string partnerPremiumType { get; set; }
        public object cardifPolicySubscriptionDate { get; set; }
        public object cardifPolicyTransationTime { get; set; }
        public object cardifPolicyTransationAmount { get; set; }
        public object cardifPolicyTransactionCurrencyISO { get; set; }
    }

    public class MortgageInfo
    {
        public string mortgageCountryCode { get; set; }
        public string mortgageCountryName { get; set; }
        public string mortgageStateCode { get; set; }
        public string mortgageStateName { get; set; }
        public string mortgageCityCode { get; set; }
        public string mortgageCityName { get; set; }
        public string mortgageCityZone { get; set; }
        public string mortgageZoneType { get; set; }
        public string mortgageAddress { get; set; }
    }

    public class Product
    {
        public int cardifProductId { get; set; }
        public string cardifProductName { get; set; }
        public string cardifProductComercialName { get; set; }
        public object productDeudorIndic { get; set; }
        public object branch { get; set; }
        public Plan[] plan { get; set; }
    }

    public class Plan
    {
        public int cardifPlanId { get; set; }
        public string cardifPlanName { get; set; }
        public Coverage[] coverage { get; set; }
    }

    public class Coverage
    {
        public int coverageId { get; set; }
        public string coverageName { get; set; }
        public string coverageType { get; set; }
        public string coverageBeginDate { get; set; }
        public int coverageNetPremiumAmnt { get; set; }
        public int coverageGrossPremiumAmnt { get; set; }
        public object coveragePremiumCurrencyISO { get; set; }
        public string coverageBusinessLineCodeCardif { get; set; }
        public int coverageMaxIncapacity { get; set; }
        public int coverageMinIncapacity { get; set; }
        public int coverageTimeBlocking { get; set; }
        public int coverageTime { get; set; }
        public int coverageTimeClaim { get; set; }
        public int coverageLoanInstallmentAmount { get; set; }
        public int coverageMaxClaimInstallment { get; set; }
        public int coverageMaxClaimByYear { get; set; }
        public string waitingPeriod { get; set; }
        public string lackPeriod { get; set; }
        public int prescriptionDays { get; set; }
        public int prescriptionYears { get; set; }
        public int coverageMinAge { get; set; }
        public int coverageMaxAge { get; set; }
        public int insuredValue { get; set; }
        public int maxInsuredValue { get; set; }
        public object insuredValueCurrencyISO { get; set; }
        public string continuityJob { get; set; }
        public int continuousWorkingDays { get; set; }
        public int workingPermanencyDays { get; set; }
        public bool eventLimitFlag { get; set; }
        public string gender { get; set; }
        public bool smokerFlag { get; set; }
        public bool rentFlag { get; set; }
    }

    public class Policydetails
    {
        public Generaldetails generalDetails { get; set; }
        public Creditcard creditCard { get; set; }
    }

    public class Generaldetails
    {
        public string statusDue { get; set; }
        public int monthPastDue { get; set; }
        public string policyCondicionalVersion { get; set; }
        public string policyLastPremBillingDate { get; set; }
        public string lastCollectionEffectiveDate { get; set; }
        public string lastCollectionAttemptDate { get; set; }
        public string lastCollectionEndDate { get; set; }
        public object lastCollectionStatus { get; set; }
    }

    public class Creditcard
    {
        public string creditCardSoldDate { get; set; }
        public int creditCardSoldAmnt { get; set; }
        public string policyTypePremPartnrType { get; set; }
    }

    public class Billing
    {
        public int elapsedTimeOnMonths { get; set; }
        public int quoteValue { get; set; }
        public string billingPeriodicity { get; set; }
        public string firstBillsDate { get; set; }
        public string lastBillsDate { get; set; }
        public int efectiveBills { get; set; }
        public float ipc { get; set; }
        public string payerFullName { get; set; }
        public object billingCurrencyISO { get; set; }
        public Payment payment { get; set; }
        public Financial financial { get; set; }
        public Reversion[] reversion { get; set; }
    }

    public class Payment
    {
        public string lastCollectionEffectiveDate { get; set; }
        public string lastCollectionAttemptDate { get; set; }
        public string lastCollectionEndDate { get; set; }
        public string lastCollectionState { get; set; }
    }

    public class Financial
    {
        public string companyCardType { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
    }

    public class Reversion
    {
        public int reverseBills { get; set; }
        public string calcultdRefundRefDateField { get; set; }
        public string reasonDescField { get; set; }
        public string sentReversionDate { get; set; }
        public string confirmReversionDate { get; set; }
        public string referenceReversion { get; set; }
        public string refundEffectiveDate { get; set; }
    }

    public class Assesor
    {
        public string assessorId { get; set; }
        public string assessorName { get; set; }
        public string assessorLastName { get; set; }
        public string assessorDocumentType { get; set; }
        public string assessorDocumentNumber { get; set; }
    }

    public class PartnerInfo
    {
        public int partnerId { get; set; }
        public string partnerName { get; set; }
        public string partnerDocumentType { get; set; }
        public string partnerDocumentNumber { get; set; }
        public string subPartnerId { get; set; }
        public string subPartnerName { get; set; }
        public string channel { get; set; }
        public string salesChannelId { get; set; }
        public string partnerSubsidiaryId { get; set; }
        public string partnerSubsidiaryName { get; set; }
        public object partnerOfficeId { get; set; }
        public string partnerOfficeName { get; set; }
        public string partnerRed { get; set; }
        public string partnerOfficeZone { get; set; }
    }

    public class Statusresponse
    {
        public string status { get; set; }
        public string description { get; set; }
    }

}