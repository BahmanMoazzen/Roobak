using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "NewDance", menuName = "Roobak/New Dance", order = 1)]
public class DanceInfo : ScriptableObject
{

    const string IS_LOCKED = "DanceInfo_";
    public AnimationClip DanceClip;
    [SerializeField] bool isLocked;

    public string DanceName
    {
        get
        {
            return DanceClip.name;
        }
    }
    public string SKU
    {
        get
        {
            return "RBK_DANCE_" + DanceClip.name.Replace(" ", "");
        }
    }

    public string UnlockButtonLabelTranslated
    {

        get
        {
            string buttonLabel = string.Empty;
            switch (UnlockMethod)
            {
                case DanceUnlockMethods.ByUserRequest:
                    buttonLabel = BAHMANLanguageManager._Instance._Translate("Claim");
                    break;
                case DanceUnlockMethods.ByXPLevels:
                    if (BAHMANXPManager._Instance._GetCurrentLevel() >= UnlockLevel)
                    {
                        buttonLabel = BAHMANLanguageManager._Instance._Translate("Claim");
                        break;
                    }
                    else
                    {
                        switch (AlternateUnlockMethod)
                        {
                            case DanceUnlockMethods.ByUserRequest:
                                buttonLabel = BAHMANLanguageManager._Instance._Translate("Claim");
                                break;
                            case DanceUnlockMethods.ByXPLevels:
                                buttonLabel = BAHMANLanguageManager._Instance._Translate("Unlocks At Level") + " " + UnlockLevel.ToString();
                                break;
                            case DanceUnlockMethods.ByAdvertisment:
                                buttonLabel = BAHMANLanguageManager._Instance._Translate("Unlocks At Level") + " " + UnlockLevel.ToString() + " "
                                    + BAHMANLanguageManager._Instance._Translate("Or") + " "
                                    + BAHMANLanguageManager._Instance._Translate("Whatch Ad to Unlock");
                                break;
                            case DanceUnlockMethods.ByMarket:
                                buttonLabel = BAHMANLanguageManager._Instance._Translate("Unlocks At Level") + " " + UnlockLevel.ToString() + " "
                                    + BAHMANLanguageManager._Instance._Translate("Or") + " "
                                    + BAHMANLanguageManager._Instance._Translate("Purchase");
                                break;
                        }
                        break;
                    }

                case DanceUnlockMethods.ByAdvertisment:
                    switch (AlternateUnlockMethod)
                    {
                        case DanceUnlockMethods.ByUserRequest:
                            buttonLabel = BAHMANLanguageManager._Instance._Translate("Claim");
                            break;
                        case DanceUnlockMethods.ByXPLevels:
                            buttonLabel = string.Format("{0} {1} {2} {3}",
                                BAHMANLanguageManager._Instance._Translate("Whatch Ad to Unlock"), BAHMANLanguageManager._Instance._Translate("Or"),
                                BAHMANLanguageManager._Instance._Translate("Unlocks At Level"), UnlockLevel.ToString());
                            break;
                        case DanceUnlockMethods.ByAdvertisment:
                            buttonLabel = BAHMANLanguageManager._Instance._Translate("Whatch Ad to Unlock");
                            break;
                        case DanceUnlockMethods.ByMarket:
                            buttonLabel = string.Format("{0} {1} {2}", BAHMANLanguageManager._Instance._Translate("Whatch Ad to Unlock"),
                                BAHMANLanguageManager._Instance._Translate("Or"), BAHMANLanguageManager._Instance._Translate("Purchase"));
                            break;
                    }
                    break;
                case DanceUnlockMethods.ByMarket:
                    switch (AlternateUnlockMethod)
                    {
                        case DanceUnlockMethods.ByUserRequest:
                            buttonLabel = BAHMANLanguageManager._Instance._Translate("Claim");
                            break;
                        case DanceUnlockMethods.ByXPLevels:
                            buttonLabel = string.Format("{0} {1} {2} {3}", BAHMANLanguageManager._Instance._Translate("Purchase")
                                , BAHMANLanguageManager._Instance._Translate("Or"), BAHMANLanguageManager._Instance._Translate("Unlocks At Level"), UnlockLevel.ToString());
                            break;
                        case DanceUnlockMethods.ByAdvertisment:
                            buttonLabel = string.Format("{0} {1} {2}",
                                BAHMANLanguageManager._Instance._Translate("Purchase"),
                                BAHMANLanguageManager._Instance._Translate("Or"),
                                BAHMANLanguageManager._Instance._Translate("Whatch Ad to Unlock"));
                            break;
                        case DanceUnlockMethods.ByMarket:
                            buttonLabel = BAHMANLanguageManager._Instance._Translate("Purchase");
                            break;
                    }
                    break;
            }

            return buttonLabel;

        }


    }
    public int UnlockLevel;
    public string IsLockedSaveTag
    {
        get
        {
            return IS_LOCKED + DanceName;
        }
    }
    public bool IsLocked
    {
        get
        {
            return PlayerPrefs.GetInt(IsLockedSaveTag, isLocked ? 1 : 0) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(IsLockedSaveTag, value ? 1 : 0);
        }
    }
    UnityAction _unlockFail, _unlockSuccess;
    void _unlockFailRoutine()
    {

        string message = "";
        switch (AlternateUnlockMethod)
        {
            case DanceUnlockMethods.ByUserRequest:
                _unlockSuccessRoutine();
                return;
            case DanceUnlockMethods.ByXPLevels:
                if (BAHMANXPManager._Instance._GetCurrentLevel() >= UnlockLevel)
                {
                    _secondaryUnlockSuccessRoutine();
                    return;
                }
                else
                {
                    _secondaryUnlockFailedRoutine();
                    return;
                }
            case DanceUnlockMethods.ByAdvertisment:
                message = "Show Ad to Unlock Instead?";
                break;
            case DanceUnlockMethods.ByMarket:
                message = "Pay to Unlock Instead?";
                break;
        }
        string title = "Use Alternate Unlock?";
        string yes = "Yes";
        string no = "No";



        BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(
            BAHMANLanguageManager._Instance._Translate(title)
            , BAHMANLanguageManager._Instance._Translate(message)
            , BAHMANLanguageManager._Instance._Translate(yes)
            , BAHMANLanguageManager._Instance._Translate(no)
            , true, true
            , _alternateUnlockNo, _alternateUnlockYes, _alternateUnlockNo);


    }

    void _alternateUnlockYes()
    {
        switch (AlternateUnlockMethod)
        {
            case DanceUnlockMethods.ByXPLevels:
                if (BAHMANXPManager._Instance._GetCurrentLevel() >= UnlockLevel)
                {
                    _secondaryUnlockSuccessRoutine();
                }
                else
                {
                    _secondaryUnlockFailedRoutine();
                }
                break;
            case DanceUnlockMethods.ByAdvertisment:
                BAHMANAdManager._Instance._ShowAd(AdTypes.Rewarded, _secondaryUnlockSuccessRoutine, _secondaryUnlockFailedRoutine);
                break;
            case DanceUnlockMethods.ByMarket:
                BAHMANAdManager._Instance._BuySKU(SKU, _secondaryUnlockSuccessRoutine, _secondaryUnlockFailedRoutine);
                break;
            case DanceUnlockMethods.ByUserRequest:
                _secondaryUnlockSuccessRoutine();
                break;

        }
    }

    void _alternateUnlockNo()
    {
        _secondaryUnlockFailedRoutine();
    }

    void _unlockSuccessRoutine()
    {
        IsLocked = false;
        _unlockSuccess?.Invoke();
    }

    void _secondaryUnlockFailedRoutine()
    {
        _unlockFail?.Invoke();
    }
    void _secondaryUnlockSuccessRoutine()
    {
        IsLocked = false;
        _unlockSuccess?.Invoke();
    }

    public DanceUnlockMethods UnlockMethod;
    public DanceUnlockMethods AlternateUnlockMethod;
    public void Unlock(UnityAction iSuccess, UnityAction iFailed)
    {
        _unlockFail = iFailed;
        _unlockSuccess = iSuccess;
        switch (UnlockMethod)
        {
            case DanceUnlockMethods.ByXPLevels:
                if (BAHMANXPManager._Instance._GetCurrentLevel() >= UnlockLevel)
                {
                    _unlockSuccessRoutine();
                }
                else
                {
                    _unlockFailRoutine();
                }
                break;
            case DanceUnlockMethods.ByAdvertisment:
                BAHMANAdManager._Instance._ShowAd(AdTypes.Rewarded, _unlockSuccessRoutine, _unlockFailRoutine);
                break;
            case DanceUnlockMethods.ByMarket:
                BAHMANAdManager._Instance._BuySKU(SKU, _unlockSuccessRoutine, _unlockFailRoutine);
                break;
            case DanceUnlockMethods.ByUserRequest:
                _unlockSuccessRoutine();
                break;
        }
    }

    public string UnlockButtonLabel
    {
        get
        {
            string buttonLabel = string.Empty;
            switch (UnlockMethod)
            {
                case DanceUnlockMethods.ByUserRequest:
                    buttonLabel = "Claim";
                    break;
                case DanceUnlockMethods.ByXPLevels:
                    if (BAHMANXPManager._Instance._GetCurrentLevel() >= UnlockLevel)
                    {
                        buttonLabel = "Claim";
                        break;
                    }
                    else
                    {
                        switch (AlternateUnlockMethod)
                        {
                            case DanceUnlockMethods.ByUserRequest:
                                buttonLabel = "Claim";
                                break;
                            case DanceUnlockMethods.ByXPLevels:
                                buttonLabel = "Unlocks At Level " + UnlockLevel.ToString();
                                break;
                            case DanceUnlockMethods.ByAdvertisment:
                                buttonLabel = "Unlocks At Level " + UnlockLevel.ToString() + " " + "Or" + " " + "Whatch Ad to Unlock";
                                break;
                            case DanceUnlockMethods.ByMarket:
                                buttonLabel = "Unlocks At Level " + UnlockLevel.ToString() + " " + "Or" + " " + "Purchase";
                                break;
                        }
                        break;
                    }

                case DanceUnlockMethods.ByAdvertisment:
                    switch (AlternateUnlockMethod)
                    {
                        case DanceUnlockMethods.ByUserRequest:
                            buttonLabel = "Claim";
                            break;
                        case DanceUnlockMethods.ByXPLevels:
                            buttonLabel = string.Format("{0} {1} {2}", "Whatch Ad to Unlock", "Or", "Unlocks At Level" + UnlockLevel.ToString());
                            break;
                        case DanceUnlockMethods.ByAdvertisment:
                            buttonLabel = "Whatch Ad to Unlock";
                            break;
                        case DanceUnlockMethods.ByMarket:
                            buttonLabel = string.Format("{0} {1} {2}", "Whatch Ad to Unlock", "Or", "Purchase");
                            break;
                    }
                    break;
                case DanceUnlockMethods.ByMarket:
                    switch (AlternateUnlockMethod)
                    {
                        case DanceUnlockMethods.ByUserRequest:
                            buttonLabel = "Claim";
                            break;
                        case DanceUnlockMethods.ByXPLevels:
                            buttonLabel = string.Format("{0} {1} {2}", "Purchase", "Or", "Unlocks At Level" + UnlockLevel.ToString());
                            break;
                        case DanceUnlockMethods.ByAdvertisment:
                            buttonLabel = string.Format("{0} {1} {2}", "Purchase", "Or", "Whatch Ad to Unlock");
                            break;
                        case DanceUnlockMethods.ByMarket:
                            buttonLabel = "Purchase";
                            break;
                    }
                    break;
            }
            return buttonLabel;
        }

    }
}
public enum DanceUnlockMethods { ByUserRequest, ByXPLevels, ByAdvertisment, ByMarket }