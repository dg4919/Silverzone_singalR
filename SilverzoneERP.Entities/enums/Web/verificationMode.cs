namespace SilverzoneERP.Entities
{
    // for all enums we suffix with type > so that we can know this is a enum type
    public enum verificationMode  // we r using value of enums in the tables so its a part of entity
    {              // use to detect verification is to chek by mobile/Email
      email,
      mobile
    }
}
