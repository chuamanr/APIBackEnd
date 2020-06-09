using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{

    public class Rootobject2
    {
        public ClienteAsegurado2 clienteAsegurado { get; set; }
    }

    public class ClienteAsegurado2
    {
        public Customerinformation2 customerInformation { get; set; }
        public Statusresponse statusResponse { get; set; }
    }

    public class Customerinformation2
    {
        public Customerdata customerData { get; set; }
        public Policy2[] policy { get; set; }
    }


    public class Policy2
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
        public Product2 product { get; set; }
        public PartnerInfo partnerInfo { get; set; }
    }

    public class Product2
    {
        public int cardifProductId { get; set; }
        public string cardifProductName { get; set; }
        public string cardifProductComercialName { get; set; }
        public object productDeudorIndic { get; set; }
        public object branch { get; set; }
        public Plan[] plan { get; set; }
    }
}