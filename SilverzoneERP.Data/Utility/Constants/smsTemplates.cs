namespace SilverzoneERP.Data
{
    internal class smsTemplates
    {
        public const string new_registration = "Dear User Your verification code for registration on silverzone.org is {0}";
        public const string foget_password = "{0} is your Silverzone verification code to change password, Code valid for 10 minutes only, one time use.";
        public const string change_mobile = "{0} is your Silverzone verification code to change mobile number, Code valid for 10 minutes only, one time use.";

        public const string order_confirmation = "Hi, we have received your order {0} of Rs. {1} and it is being processed."
            + " Your order will be delivered within 7-10 working days. Track you order at {2} Thank You";
        
    }
}
